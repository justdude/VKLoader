using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;


namespace VKSyncMusic.Handlers
{
    public class ColorJam
    {
        public static List<SolidColorBrush> AllCollors { get; private set; }

        public static SolidColorBrush DesignBackground { get; private set; }
        public static SolidColorBrush SelectedColor { get; set; }

        static ColorJam()
        {

            Type ColorType = typeof(System.Windows.Media.Colors);
            PropertyInfo[] arrPiColors = ColorType.GetProperties(BindingFlags.Public | BindingFlags.Static);
            AllCollors = new List<SolidColorBrush>();

            foreach (PropertyInfo pi in arrPiColors)
            {
                var clrKnownColor = (Color)pi.GetValue(null, null);
                AllCollors.Add( new SolidColorBrush(clrKnownColor));
            }


            DesignBackground = SelectedColor;
            
        }
    }
}
