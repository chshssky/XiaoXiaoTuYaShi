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
using System.Threading.Tasks;
using Parse;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace KidsPainter
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class ChangeInfo_zyt : KidsPainter.Common.LayoutAwarePage
    {
        public ChangeInfo_zyt()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="navigationParameter">最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的参数值。
        /// </param>
        /// <param name="pageState">此页在以前会话期间保留的状态
        /// 字典。首次访问页面时为 null。</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="pageState">要使用可序列化状态填充的空字典。</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void btnGetNum_Click(object sender, RoutedEventArgs e)
        {
            
        }
        /*
         * 把新邮箱发送到服务器，让服务器往此邮箱发送验证码
         */
        private void btnYes1_Click(object sender, RoutedEventArgs e) //修改邮箱的确定键
        {
            if (txBoOrgEmail.Text.Equals("") || txBoNewEmail.Text.Equals("") || psdBox.Password.Equals(""))
                txBlShow1.Text = "输入不完整，请检查";
            else
                txBlShow1.Text = "成功";

            //ParseUser.CurrentUser

        }
        /*
       *修改邮箱部分，查看各种输入是否合法 
       */


        private void btnYes2_Click(object sender, RoutedEventArgs e) //修改密码的确定键
        {

        }
        /*
         * 把数据提交给服务器，获得返回值，判断是否修改成功。修改成功则跳转到主界面
         */

        private async void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped ||
                 Windows.UI.ViewManagement.ApplicationView.TryUnsnap() == true)
            {
                Windows.Storage.Pickers.FileOpenPicker openPicker = new Windows.Storage.Pickers.FileOpenPicker();
                openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;

                // Filter to include a sample subset of file types.
                openPicker.FileTypeFilter.Clear();
                openPicker.FileTypeFilter.Add(".bmp");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".jpg");

                // Open the file picker.
                Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();

                // file is null if user cancels the file picker.
                if (file != null)
                {
                    // Open a stream for the selected file.
                    Windows.Storage.Streams.IRandomAccessStream fileStream =
                        await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    // Set the image source to the selected bitmap.
                    Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage =
                        new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                    bitmapImage.SetSource(fileStream);
                    imgPhoto.Source = bitmapImage;
                    this.DataContext = file; // 将storageFile设置为照片页面的DataContext

                    // mruToken = Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList.Add(file);
                }
            }




        }

        private async void btnTakePhoto_Click(object sender, RoutedEventArgs e)
        {
            PhotoTaker phTaker = new PhotoTaker();
            Task<ReturnElements> task = phTaker.takePhoto(imgPhoto.Width, imgPhoto.Height);
            ReturnElements returnEle = await task;
            Image image = new Image();
            image.Source = returnEle.bitmapImage;
            imgPhoto.Source = returnEle.bitmapImage;
            txBlPath.Text = returnEle.filename; 
        }
        
      

    }
}
