using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VkDay.Model;
using VkEasyPhones.Constants;
using System.Reflection;
using System.Windows;

//using ExcelInt = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;
//using GemBox.Spreadsheet;

using System.Globalization;
using System.Threading;
using SpreadsheetLight;

namespace VkEasyPhones.Helpers
{
	public class CExcelHelper
	{
		static CExcelHelper()
		{
			// If using Professional version, put your serial key below.
			//SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
		}

		//public static void WriteToExcelInterop(string path, IEnumerable<Profile> data)
		//{
		//	CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;
			
		//	Thread.CurrentThread.CurrentCulture =
		//	new System.Globalization.CultureInfo("en-US");

		//	try
		//	{
		//		if (System.IO.File.Exists(path))
		//			System.IO.File.Delete(path);

		//		Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();


		//		object nullVal = System.Reflection.Missing.Value;

		//		if (xlApp == null)
		//		{
		//			MessageBox.Show("Excel is not properly installed!!");
		//			return;
		//		}

		//		ExcelInt.Workbook xlWorkBook;
		//		ExcelInt.Worksheet xlWorkSheet;

		//		xlWorkBook = xlApp.Workbooks.Add(nullVal);
		//		xlWorkSheet = (ExcelInt.Worksheet)xlWorkBook.Worksheets.get_Item(1);

		//		int startRow = 2;
		//		int startColl = 2;

		//		string[] colls = null;
		//		object[,] rows = null;

		//		List<string> names = new List<string>();

		//		List<Profile> dataList = data.ToList();

		//		rows = FillByReflection<Profile>(names, dataList);

		//		colls = names.ToArray();
		//		Microsoft.Office.Interop.Excel.Range excelcells;
		//		excelcells = xlWorkSheet.get_Range("A1", "A1");

		//		for (int j = startColl; j < colls.Count() + startColl; j++)
		//		{
		//			//var range = excelcells.get_Offset(startRow - 1, j);
		//			xlWorkSheet.Cells[startRow - 1, j].Value = colls[j - startColl];
		//		}

		//		var startCell = xlWorkSheet.get_Range("A1", "A1");//(Range)xlWorkSheet.Cells[startRow, startColl];
		//		//var endCell = (Range)xlWorkSheet.Cells[data.Count() + startRow, colls.Count() + startColl];
		//		//var writeRange = xlWorkSheet.Range[startCell, endCell];

		//		//writeRange.Value2 = rows;

		//		for (int i = startColl; i < colls.Length + startColl; i++)
		//		{
		//			for (int j = startRow; j < data.Count() + startRow; j++)
		//			{
		//				startCell.get_Offset(j, i).Value = rows[i - startColl, j - startRow] as string;
		//			}
		//		}

		//		xlWorkBook.SaveAs(path, ExcelInt.XlFileFormat.xlWorkbookNormal, nullVal, nullVal, nullVal, nullVal, ExcelInt.XlSaveAsAccessMode.xlExclusive, nullVal, nullVal, nullVal, nullVal, nullVal);

		//		xlWorkBook.Close(true, nullVal, nullVal);
		//		xlApp.Quit();

		//		releaseObject(xlWorkSheet);
		//		releaseObject(xlWorkBook);
		//		releaseObject(xlApp);
		//	}
		//	catch(Exception)
		//	{
		//		throw;
		//	}
		//	Thread.CurrentThread.CurrentCulture = oldCI;
		//}


		public static void WriteToExcelSpreadSheets(string path, IEnumerable<Profile> data)
		{
			try
			{
				if (System.IO.File.Exists(path))
					System.IO.File.Delete(path);

				SLDocument doc = new SLDocument();

				int startRow = 2;
				int startColl = 2;

				string[] colls = null;
				object[,] rows = null;

				List<string> names = new List<string>();

				List<Profile> dataList = data.ToList();

				rows = FillByReflection<Profile>(names, dataList);

				colls = names.ToArray();

				for (int j = startColl; j < colls.Count() + startColl; j++)
				{
					//var range = excelcells.get_Offset(startRow - 1, j);
					doc.SetCellValue(startRow - 1, j, colls[j - startColl] as string);
				}

				for (int i = startColl; i < colls.Length + startColl; i++)
				{
					for (int j = startRow; j < data.Count() + startRow; j++)
					{
						doc.SetCellValue(j, i, rows[i - startColl, j - startRow] as string);
					}
				}

				doc.SaveAs(path);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private static void releaseObject(object obj)
		{
			try
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
				obj = null;
			}
			catch (Exception ex)
			{
				obj = null;
				MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
			}
			finally
			{
				GC.Collect();
			}
		}

		//public static void WriteToExcel(string path, IEnumerable<Profile> data)
		//{
		//	ExcelFile ef = new ExcelFile();
		//	ExcelWorksheet ws = ef.Worksheets.Add("List " + Translates.VkSearch);

		//	int startRow = 3;
		//	int startColl = 1;

		//	string[] colls = null;
		//	object[,] rows = null;

		//	List<string> names = new List<string>();

		//	List<Profile> dataList = data.ToList();

		//	rows = FillByReflection<Profile>(names, dataList);

		//	colls = names.ToArray();

		//	for (int j = startColl; j < colls.Count() + startColl; j++)
		//	{
		//		ws.Cells[startRow - 1, j].Value = colls[j - startColl];
		//	}

		//	for (int i = startColl; i < colls.Length + startColl; i++ )
		//	{
		//		for (int j = startRow; j < data.Count() + startRow; j++ )
		//		{
		//			ws.Cells[j, i].Value = rows[i - startColl, j - startRow];
		//		}
		//	}

		//	ws.PrintOptions.FitWorksheetWidthToPages = 1;

		//	ef.Save(path);
		//}

		private static object[,] FillByReflection<T>(List<string> names, List<T> dataList)
		{
			Type type = typeof(T);

			PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			object[,] rows = new object[properties.Length, dataList.Count];


			for (int i = 0; i < properties.Length; i++)
			{
				for (int j = 0; j < dataList.Count(); j++)
				{
					object item = properties[i].GetValue(dataList[j], null);
					rows[i, j] =  item == null? null : item.ToString();
				}
				names.Add(properties[i].Name);
			}
			return rows;
		}
	}
}
