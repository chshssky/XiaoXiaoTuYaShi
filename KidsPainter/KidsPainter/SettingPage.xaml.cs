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
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage.FileProperties;
using Windows.Storage;
using Painter;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.ApplicationModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace KidsPainter
{
    public sealed partial class BasicPage1 : KidsPainter.Common.LayoutAwarePage
    {
        public BasicPage1()
        {
            this.InitializeComponent();
        }


        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }


        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }


        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (tbxParentName.Text.Equals("")|| tbxEmail .Text.Equals("") || txbName.Text.Equals(""))
            {
                gridComplete.Visibility = Visibility.Visible;// 提醒要 输入完整之类的 。。。。。。
            }
            else
            {
                StoreSetting storeSetting = new StoreSetting();
                storeSetting.store(tbxParentName.Text,tbxEmail.Text,txbName.Text);
                if (this.Frame != null)
                    this.Frame.Navigate(typeof(Main));
            }
        }

        private async void btnTakePhoto_Click(object sender, RoutedEventArgs e)
        {
            // takePhoto();
            PhotoTaker phTaker = new PhotoTaker();
            Task<ReturnElements> task = phTaker.takePhoto(parentImage.Width, parentImage.Height);
            ReturnElements returnEle = await task;
            Image image = new Image();
            image.Source = returnEle.bitmapImage;
            parentImage.Source = returnEle.bitmapImage;
            txbName.Text = returnEle.filename;            
        }
    }
}
