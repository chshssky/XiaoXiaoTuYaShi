using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace KidsPainter
{
    class Message
    {
        public async static void ShowDialog(String Content)
        {

            MessageDialog messageDialog = new MessageDialog(Content);
            await messageDialog.ShowAsync();
        }

        public static void ShowToast(String Content)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText04);
            XmlElement xe = toastXml.CreateElement("title");

            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements.Item(0).AppendChild(toastXml.CreateTextNode(Content));
            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
