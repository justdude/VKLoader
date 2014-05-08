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

using System.ComponentModel;
namespace VKMusicSync
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

            //CategorisedImages = new List<MyCategory>
           /* {
                new MyCategory { CategoryId=1, Name="Images 1", Images = new List<MyImage>
                    {
                        new MyImage(@"http://cs7009.vk.me/c540107/v540107872/4c7b/ZRtLhHSHKz8.jpg") { ImageId=1 },
                        new MyImage(@"http://cs7009.vk.me/c540107/v540107872/4c7b/ZRtLhHSHKz8.jpg") { ImageId=2 }
                    }
                },
                new MyCategory { CategoryId=2, Name="Images 2", Images = new List<MyImage>
                    {
                        new MyImage(@"http://cs7009.vk.me/c540107/v540107872/4c7b/ZRtLhHSHKz8.jpg") { ImageId=3 },
                        new MyImage(@"http://cs7009.vk.me/c540107/v540107872/4c7b/ZRtLhHSHKz8.jpg") { ImageId=4 },
                        new MyImage(@"http://cs7009.vk.me/c540107/v540107872/4c7b/ZRtLhHSHKz8.jpg") { ImageId=3 },
                        new MyImage(@"http://cs7009.vk.me/c540107/v540107872/4c7b/ZRtLhHSHKz8.jpg") { ImageId=4 },
                        new MyImage(@"http://cs7009.vk.me/c540107/v540107872/4c7b/ZRtLhHSHKz8.jpg") { ImageId=3 },
                        new MyImage(@"http://cs7009.vk.me/c540107/v540107872/4c7b/ZRtLhHSHKz8.jpg") { ImageId=4}
                    }
                }
            };*/

             BackgroundWorker backgroundWorker = new BackgroundWorker();
             backgroundWorker.DoWork += this.DoWork;
             backgroundWorker.RunWorkerCompleted += this.DoneWork;
             CategorisedImages = new List<MyCategory>();
             this.DoWork(null, null);
             //backgroundWorker.RunWorkerAsync(CategorisedImages);
             DataContext = this;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            PhotosAndAlbumBinder container = new PhotosAndAlbumBinder();
            container.Bind(Program.vk.GetAllAlbums(Program.vk.UserId, false));
            CategorisedImages = container.getAlbums();
        }

        private void DoneWork(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }


    }

    public class MyCategory
    {
        public string Name { get; set; }
        public List<MyImage> Images { get; set; }
    }

    public class MyImage
    {
        public string ImageId { get; set; }

        public MyImage(string path)
        {
            _path = path;
            _source = new Uri(path);
            _image = BitmapFrame.Create(_source);
        }

        public override string ToString()
        {
            return _source.ToString();
        }

        private string _path;

        private Uri _source;
        public string Source { get { return _path; } }

        private BitmapFrame _image;
        public BitmapFrame Image { get { return _image; } set { _image = value; } }
    }

}
