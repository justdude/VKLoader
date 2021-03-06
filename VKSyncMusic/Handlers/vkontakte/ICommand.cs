﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vkontakte
{
    public interface ICommand<T> where T : class
    {
        IList<T> Execute();
        void ExecuteNonQuery();
    }
}
