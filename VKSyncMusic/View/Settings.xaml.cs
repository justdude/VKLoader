﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Elysium;
using VKSyncMusic.ModelView;

namespace VKSyncMusic.View
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Elysium.Controls.Window
    {
        public Settings()
        {
            InitializeComponent();
            var modelview = new SettingsModelView();
            this.DataContext = modelview;
        }
    }
}
