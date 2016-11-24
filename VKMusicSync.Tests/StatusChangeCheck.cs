using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SoftComputer.Common;
using SoftComputer.TotalQC.BusinessServices.InventoryServices.Interface.BusinessEntities;
using SoftComputer.TotalQC.Infrastructure.Interface;
using SoftComputer.TotalQC.Inventory.LotRecords.Properties;
using SoftComputer.TotalQC.LotActions.OpenClose;

namespace SoftComputer.TotalQC.Inventory.LotRecords.LotRecordWorkItem.Services.SaveChecks
{
	/// <summary>
	/// Process several actions when opening or closing a lot.
	/// Check the existent lots for same qc item, etc.
	/// </summary>
	class StatusChangeCheck : BaseSaveCheck
	{
		/// <summary>
		/// Determines whether the processing could be continued after current action call.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if processing could be continued; otherwise, <c>false</c>.
		/// </returns>
		public override bool CanContinueProcess()
		{
			LotRecordInfo record = SetupModelBase.ActiveRecord;
			int newStatus = record.StatusControl;

			if (SetupModelBase.OriginalRecord.StatusControl == newStatus ||
				(newStatus != TQCDEF.ACTIVE_YES && newStatus != TQCDEF.ACTIVE_NO))
			{
				return true;
			}

			bool isActive = newStatus == TQCDEF.ACTIVE_YES;

			if (IsOpenCloseActionRequired(record, LotCheckResult.RelatedLots))
			{
				BridgeToOpenCloseLots(LotCheckResult.RelatedLots, !isActive);
			}
			else
			{
				if (!isActive)
				{
					List<BaseOpenCloseLotInfo> neverActivedLots = GetNeverActivedLots(LotCheckResult.RelatedLots);
					if (neverActivedLots.Count > 0)
					{
						BridgeToOpenCloseLots(neverActivedLots, true);
					}
				}
			}

			if (isActive)
			{
				if (HasTestingLots(LotCheckResult.RelatedLots) &&
					UserNotAgree(Resources.TestingLotsForTheSameItemExists))
				{
					return false;
				}
				if (LotCheckResult.HasNeverActivatedLotPriorToCurrent &&
					UserNotAgree(Resources.PreviousNeverAcivatedLotsExists))
				{
					return false;
				}
			}

			return true;
		}

		#region Private methods

		private void BridgeToOpenCloseLots(List<BaseOpenCloseLotInfo> lots, bool isOpenAction)
		{
			if ((isOpenAction && !SetupModelBase.IsOpenLotPermitted) ||
				(!isOpenAction && !SetupModelBase.IsCloseLotPermitted))
			{
				return;
			}

			string message = isOpenAction
								 ? Resources.OpenNotActiveRecordsQuestion
								 : Resources.CloseActiveRecordsQuestion;

			if (MessageProvider.ShowQuestion(message) == DialogResult.Yes)
			{
				if (isOpenAction)
				{
					StartWorkitem<OpenActionController>("OpenActionDialogWorkitemName", lots);
				}
				else
				{
					StartWorkitem<CloseActionController>("CloseActionDialogWorkitemName", lots);
				}
			}
		}

		private void StartWorkitem<TController>(string workItemName, List<BaseOpenCloseLotInfo> lots)
			where TController : LotActionBaseController
		{
			ControlledWorkItem<TController> workItem =
				ServicesContainer.WorkItem.WorkItems.Contains(workItemName)
					? ServicesContainer.WorkItem.WorkItems.Get<ControlledWorkItem<TController>>(workItemName)
					: ServicesContainer.WorkItem.WorkItems.AddNew<ControlledWorkItem<TController>>(workItemName);

			workItem.RunController(
				delegate(TController controller)
				{
					controller.RunController(lots, CMNDEF.NULL_AA, true, true);
				});
		}

		private bool IsOpenCloseActionRequired(LotRecordInfo record, List<BaseOpenCloseLotInfo> lots)
		{
			if (record.StatusControl != TQCDEF.ACTIVE_YES &&
				record.ReceivedDate.HasValue)
			{
				DateTime currentLotReceivedDate = record.ReceivedDate.Value;
				return lots.Exists(info => info.StatusControl != TQCDEF.ACTIVE_TESTING &&
										   info.ReceivedDate.HasValue &&
										   info.ReceivedDate.Value > currentLotReceivedDate);
			}

			return lots.Exists(info => info.StatusControl != TQCDEF.ACTIVE_TESTING);
		}

		private bool HasTestingLots(List<BaseOpenCloseLotInfo> lots)
		{
			return lots.Exists(info => info.StatusControl == TQCDEF.ACTIVE_TESTING);
		}

		private List<BaseOpenCloseLotInfo> GetNeverActivedLots(List<BaseOpenCloseLotInfo> lots)
		{
			return lots.FindAll(info => info.IsNeverActivated);
		}

		private bool UserNotAgree(string question)
		{
			return MessageProvider.ShowQuestion(question) != DialogResult.Yes;
		}

		#endregion
	}
}
