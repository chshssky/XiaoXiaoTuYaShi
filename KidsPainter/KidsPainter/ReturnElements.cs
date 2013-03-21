using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace KidsPainter
{
    class ReturnElements
    {
        public BitmapImage bitmapImage = new BitmapImage();
        public string filename = "cannot find";
        public bool success = false;
        public Stream imagestream = null;
    }
}
