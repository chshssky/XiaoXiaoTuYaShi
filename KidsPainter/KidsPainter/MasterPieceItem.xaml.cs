using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace KidsPainter
{
    public sealed partial class MasterPieceItem : UserControl
    {
        public MasterPieceItem()
        {
            this.InitializeComponent();
        }

        async public void InitialItem(String date)
        {
            this.MyDate.Text = date;
        }
        async public void InitialPicture(BitmapImage image)
        {
            this.MyImage.Source = image;
        }
    }
}
