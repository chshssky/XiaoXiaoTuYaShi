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
using KidsPainter;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Parse;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace KidsPainter
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    /// 
    class RegisterResult
    {
        public int ifSuccess;
    }
    public sealed partial class Register_zyt : KidsPainter.Common.LayoutAwarePage
    {
        public Register_zyt()
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

        private async void btnChoose_Click(object sender, RoutedEventArgs e) //从本地选择照片
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
                    imgChildPhoto.Source = bitmapImage;
                    this.DataContext = file; // 将storageFile设置为照片页面的DataContext

                    // mruToken = Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList.Add(file);
                    StorageFolder folder = null;
                    bool Failed = false;
                    try
                    {
                        folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Picture");
                    }
                    catch (System.Exception ex)
                    {
                        Failed = true;
                    }
                    if (Failed || folder == null)
                    {
                        folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Picture");
                    }
                    await file.CopyAsync(folder);
                }
            }
        }

        private async void btnTakePhoto_Click(object sender, RoutedEventArgs e)  // 使用摄像头拍照
        {
            PhotoTaker phTaker = new PhotoTaker();
            Task<ReturnElements> task = phTaker.takePhoto(imgChildPhoto.Width, imgChildPhoto.Height);
            ReturnElements returnEle = await task;
          //  Image image = new Image();
           // image.Source = returnEle.bitmapImage;
            imgChildPhoto.Source = returnEle.bitmapImage;

            txBlPath.Text = returnEle.filename; 
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            txBoParentEmail.Text = "";
            txBlPath.Text = "";
            txBoName.Text = "";    //点击取消按钮，是要把东西都清空还是返回登陆？清空的话，图片不能清空否则有点小问题
        }

        private async void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (txBoParentEmail.Text.Equals("") || txBoName.Text.Equals("") || txBlPath.Text.Equals(""))
                txBlShow.Text = "注册信息不完整"; // 判断是否填写不完整
            else if (!psdBox1.Password.Equals(psdBox2.Password)) //两次密码不同
                txBlShow.Text = "两次密码输入不同，请确认";
            else if (!Regex.IsMatch(txBoParentEmail.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                txBlShow.Text = "邮箱地址格式错误";
            else
            {
                Task<RegisterResult> rrTask = register(txBoParentEmail.Text, txBoName.Text, psdBox1.Password);
                RegisterResult registerRes = await rrTask;
                if (registerRes.ifSuccess == 1)
                {
                    Message.ShowToast("register OK");
                    this.Frame.Navigate(typeof(Login_zyt));  //这里跳转到到登陆界面还是直接帮其登陆、跳转到主界面还有待商榷
                }
            }
        }

        private async Task<RegisterResult> register(String mail, String nick_name, String password)
        {
            RegisterResult rr = new RegisterResult();
            try
            {
                StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Picture");

                StorageFile file = await folder.GetFileAsync(txBlPath.Text);
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                Stream pic = WindowsRuntimeBufferExtensions.AsStream(buffer);

                ParseFile portrait = new ParseFile(nick_name + "sPhoto.bmp", pic);
                await portrait.SaveAsync();


                var user = new ParseUser()
                {
                    Username = mail,
                    Password = password,
                    Email = mail
                };
                
                // other fields can be set just like with ParseObject
                user["portraitImg"] = portrait;
                user["nickName"] = nick_name;

                await user.SignUpAsync();
                
                rr.ifSuccess = 1;
                return rr;
            }
            catch (Exception error)
            {
                rr.ifSuccess = 0;
                Message.ShowDialog("用户名重复！");
                return rr;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
                this.Frame.Navigate(typeof(Login_zyt));  //跳转过去时，同时要把用户名参数传过去
        }

        /*点击此按钮时，先把邮箱地址发送到服务器上，服务器往该邮箱发送验证码同时把验证码发送
         *到我这里，当我确定验证码和其他信息无误之后，再把必要信息发送到服务器上存储。
         */


    }
}
