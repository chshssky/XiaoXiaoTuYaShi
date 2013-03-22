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
using Painter;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.Serialization;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Parse;
using System.Net.Http;
using Windows.UI.Xaml.Media.Imaging;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace KidsPainter
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public sealed partial class Main : Page
    {
        private string email;
        private AgreementControl agCon = new AgreementControl();
        public Main()
        {
            this.InitializeComponent();
            //Ifagree();
            //Ifagree();
            
            /*
            List<int> number = new List<int>();
            number.Add(3);
            number.Add(4);
            number.Add(5);
            number.Add(3);
            number.Add(4);
            timeline.ItemsSource = number;
             * */
            //loadTest();
            loadPaints();

            loadImg();
            

        }

        private async void loadTest()
        {
            //test
            var query = from Job in ParseObject.GetQuery("Job")
            where Job.Get<string>("name") == "test"
            select Job;
            IEnumerable<ParseObject> results = await query.FindAsync();
            var file = results.ElementAt<ParseObject>(0).Get<ParseFile>("file");
            String text = await new HttpClient().GetStringAsync(file.Url);
            Message.ShowDialog(text);
        }

        private async void loadPaints()
        {


            String user = ParseUser.CurrentUser.Username;

            var query = from Paints in ParseObject.GetQuery("Paints")
                        where Paints.Get<string>("user") == user
                        select Paints;
            IEnumerable<ParseObject> results = await query.FindAsync();

            int count = results.Count<ParseObject>();
            Message.ShowToast("" + count);

            for (int i = 0; i < count; i++)
            {
                MasterPieceItem item = new MasterPieceItem();

                DateTime? postTime = results.ElementAt<ParseObject>(i).CreatedAt;
                //差8个小时
                DateTime postTim = postTime.Value.AddHours(8);

                item.InitialItem("发布于: " + postTim);

               // var paintImage = results.ElementAt<ParseObject>(i).Get<ParseFile>("imageFile");
                //Stream imgStream = await new HttpClient().GetStreamAsync(paintImage.Url);

                txblName.Text = ParseUser.CurrentUser.Get<string>("nickName");

                InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream();

                //await imgStream.CopyToAsync(ras.AsStreamForWrite());

                BitmapImage bmpImg = new BitmapImage();
               // bmpImg.SetSource(ras);


                item.InitialPicture(new BitmapImage(new Uri("ms-appx:/Assets/Logo.png")));//bmpImg);

                timeline.Items.Add(item);
            }
        }

        private async void loadImg()
        {
            var portrait = ParseUser.CurrentUser.Get<ParseFile>("portraitImg");

            //Message.ShowDialog("" + portrait.Url);

            Stream imgStream = await new HttpClient().GetStreamAsync(portrait.Url);
            txblName.Text = ParseUser.CurrentUser.Get<string>("nickName");

            InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream();

            await imgStream.CopyToAsync(ras.AsStreamForWrite());

            BitmapImage bmpImg = new BitmapImage();
            bmpImg.SetSource(ras);

            imgPortrait.Source = new BitmapImage(new Uri("ms-appx:/Assets/Logo.png")); ;//;bmpImg;//

        }


        public async void Ifagree()
        {
            agCon.checkAgreement();
            int checkResult = agCon.checkResult;
            if (checkResult == 0)
            {
                string agreementContent = "尊敬的用户,\r我们的软件是为您的孩子提供一个画画，并且使用email发送绘画作品的平台。在使用之前，你们可以在通讯录中设置孩子的朋友圈（包括父母、亲人）的邮箱。这是一款发挥孩子创造力，帮助孩子沟通交流的软件。由于我们的软件设计到互联网的信息交流，需要你们提供孩子朋友圈的邮箱和称呼、头像以便孩子使用，无需邮箱密码。我们郑重声明，我们将使用这些信息用于你们和孩子之间的邮件往来，绝对不会透露用户隐私，不会泄露用户信息。\r如果您同意使用我们的软件，请点击同意，表示同意我们之间的协议，谢谢您对我们的支持。   \r来自：同济大学 ;\r有问题请联系：eatinglovesun@163.com";
                MessageDialog md = new MessageDialog(agreementContent, "用户协议");
                md.Commands.Add(new UICommand("同意", new UICommandInvokedHandler(this.closeDialog)));
                md.Commands.Add(new UICommand("不同意", new UICommandInvokedHandler(this.closeApp)));
                await md.ShowAsync();
            }
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //dynamic par = e.Parameter;
            //email = par.A;
            //base.OnNavigatedTo(e);
        }
        private void closeDialog(IUICommand command)
        {
            agCon.WriteIntoFile(1);
        }
        private void closeApp(IUICommand command)
        {
            agCon.WriteIntoFile(0);
            Application.Current.Exit();
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage), new { A = email });
            }
        }
        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
        /*    PasswordCheck psCheck = new PasswordCheck();
            Task<string> task = psCheck.check();
            string checkResult = await task;
            if (checkResult.Equals(""))
                this.Frame.Navigate(typeof(SetPassword));*/
           if (this.Frame != null)
           {
               this.Frame.Navigate(typeof(BasicPage1));
           }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Login_zyt));
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
                this.Frame.Navigate(typeof(ChangeInfo_zyt));
        }

        private void btnFriendList_Click(object sender, RoutedEventArgs e)
        {

        }

        /*
         * 此设置用于更改相关信息：登陆邮箱，密码，昵称，照片等等。
         */
    }
}
