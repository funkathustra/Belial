using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Belial.Messages
{
    public class Track
    {
        public string FileKey { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Name { get; set; }
        public BitmapImage Image {get; set; }
        public string ImageSrc { get; set; }
    }
}
