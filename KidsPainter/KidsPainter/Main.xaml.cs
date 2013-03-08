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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace KidsPainter
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public sealed partial class Main : Page
    {
        private AgreementControl agCon = new AgreementControl();
        public Main()
        {
            this.InitializeComponent();
            Ifagree();
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
                this.Frame.Navigate(typeof(MainPage));
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
    }
}
