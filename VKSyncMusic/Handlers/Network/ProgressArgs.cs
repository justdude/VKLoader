using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.ComponentModel;

namespace VKSyncMusic.Handlers
{

    public class ProgressArgs
    {
        /// <summary>
        /// Возвращает максимальное число полученных байтов.
        /// </summary>
        public double BytesReceived
        {
            get;
            private set;
        }
        /// <summary>
        /// Возвращает процент выполнения асинхронной задачи
        /// </summary>
        public double ProgressPercentage
        {
            get;
            private set;
        }
        /// <summary>
        /// bозвращает общее количество байтов в операции по загрузке данных
        /// </summary>
        public double TotalBytesToReceive
        {
            get;
            private set;
        }

        public object UserState
        {
            get;
            private set;
        }

        public ProgressArgs()
        {

        }

        public ProgressArgs(double received, double progressPercentage, double totalBytes, object userState)
        {
            this.BytesReceived = received;
            this.ProgressPercentage = progressPercentage;
            this.TotalBytesToReceive = totalBytes;
            this.UserState = userState;
        }

    }

}
