using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace VK
{
    /// <summary>
    /// Логика взаимодействия для AlbumForm.xaml
    /// </summary>
    public partial class AlbumForm : Window
    {
        public List<MyCategory> CategorisedImages { get; set; }

        MyImage _SelectedImage;
        public MyImage SelectedImage
        {
            get
            {
                return _SelectedImage;
            }
            set
            {
                if (_SelectedImage != value)
                {
                    _SelectedImage = value;
                    if (value != null)
                    {
                        var win = new Window(); // This would be your enlarged view control, inherited from Window.
                        win.Show();
                    }
                }
            }
        }

        public AlbumForm()
        {
            InitializeComponent();

            CategorisedImages = new List<MyCategory> 
            {
                new MyCategory { CategoryId=1, Name="Images 1", Images = new List<MyImage>
                    {
                        new MyImage { ImageId=1, ImagePath="http://blah.com/images/image1a.png" },
                        new MyImage { ImageId=2, ImagePath="http://blah.com/images/image1b.png" }
                    }
                },
                new MyCategory { CategoryId=2, Name="Images 2", Images = new List<MyImage>
                    {
                        new MyImage { ImageId=3, ImagePath="http://blah.com/images/image2a.png" },
                        new MyImage { ImageId=4, ImagePath="http://blah.com/images/image2b.png" },
                        new MyImage { ImageId=3, ImagePath="http://blah.com/images/image2a.png" },
                        new MyImage { ImageId=4, ImagePath="http://blah.com/images/image2b.png" },
                        new MyImage { ImageId=3, ImagePath="http://blah.com/images/image2a.png" },
                        new MyImage { ImageId=4, ImagePath="http://blah.com/images/image2b.png" }
                    }
                }
            };
            DataContext = this;
        }


    }

    public class MyCategory
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<MyImage> Images { get; set; }
    }

    public class MyImage
    {
        public int ImageId { get; set; }
        public string ImagePath { get; set; }
    }

}
