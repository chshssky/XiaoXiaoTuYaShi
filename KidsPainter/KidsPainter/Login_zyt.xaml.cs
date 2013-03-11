using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Parse;


// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace KidsPainter
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class Login_zyt : KidsPainter.Common.LayoutAwarePage
    {
        public Login_zyt()
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

        private async void btnYes_Click(object sender, RoutedEventArgs e)
        {
            string emptyHint = "您好，邮箱地址或者密码不能为空";
            string wrongEmHint = "您好，您输入的邮箱地址格式不正确";
            string rightHint = "您好，输入正确";
            string errorHint = "登陆失败";
            string email = txBoEm.Text;
            if (txBoEm.Text.Equals("") || pdBox.Password.Equals(""))
                txBlShow.Text = emptyHint;
            else if (!Regex.IsMatch(txBoEm.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                txBlShow.Text = wrongEmHint;
               
            else
            {
                // login ~
                try
                {
                    await ParseUser.LogInAsync(txBoEm.Text, pdBox.Password);
                    // Login was successful.
                    //txBlShow.Text = "登陆成功~~";
                    Message.ShowDialog("!!!!!!!!!!!!!!!!!success");
                    if (this.Frame != null)
                        this.Frame.Navigate(typeof(Login_zyt), new { A = email });  //跳转过去时，同时要把用户名参数传过去

                }
                catch (Exception error)
                {
                    // The login failed. Check the error to see why.
                    txBlShow.Text = errorHint;
                    Message.ShowDialog("!!!!!!!!!!!!!!!!!failed");
                }

            }

           

        }

        private void btnToRegister_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
                this.Frame.Navigate(typeof(Register_zyt));
        }
    }
}
