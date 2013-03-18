using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Painter;
using Windows.Graphics.Imaging;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.ApplicationModel;
using Windows.Storage.Pickers;
using System.Runtime.InteropServices;
using KidsPainter;
using System.Diagnostics;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.ApplicationModel.DataTransfer;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace Painter
{

    public sealed partial class MainPage : Page
    {
        string email;
        FlagFactory factory = new FlagFactory();
        ComponentsControl control = new ComponentsControl();
        DrawShape draw = new DrawShape();

        public MainPage()
        {
            this.InitializeComponent();
            InitialSoftware();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) 
        {
            dynamic par = e.Parameter;
            email = par.A;
            base.OnNavigatedTo(e);
        }

        private void InitialSoftware()
        {
            this.ColorList.Visibility = Visibility.Collapsed;
            this.MailGrid.Visibility = Visibility.Collapsed;
            this.MailBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
            this.QuestionGrid.Visibility = Visibility.Collapsed;
            this.MailList.Visibility = Visibility.Collapsed;
            this.PictureView.Visibility = Visibility.Collapsed;

            control.SetFlagFactory(factory);

            control.SetColorList(ColorList);
            control.SetMailGrid(MailGrid);
            control.SetMailBox(MailBox);
            control.SetMailList(MailList);
            control.SetPainter(Painter);
            control.SetQuestionGrid(QuestionGrid);
            control.SetPictureView(PictureView);

            control.SetButton_new(Button_new, Button_new_Copy);
            control.SetButton_waterBrush(Button_waterBrush, Button_waterBrush_Copy);
            control.SetButton_oilPastel(Button_oilPastel, Button_oilPastel_Copy);
            control.SetButton_pictureLib(Button_pictureLib, Button_pictureLib_Copy);
            control.SetButton_sendEmail(Button_sendEmail, Button_sendEmail_Copy);
            control.SetButton_takephoto(Button_takePhoto, Button_takePhoto_Copy);
            control.SetButton_writeText(Button_writeText, Button_writeText_Copy);
            control.SetButton_eraser(Button_eraser, Button_eraser_Copy);

            control.Initial();
            draw.SetComponentControl(control);
        }

         private void TakePhoto(object sender, RoutedEventArgs e)
        {
            Storyboard_takephoto.Begin();
            control.TakePhoto();
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            draw.HandlePressed(e.GetCurrentPoint(this).Position,e.Pointer);
            base.OnPointerPressed(e);
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            draw.HandleMoved(e.GetCurrentPoint(this).Position,e.Pointer);
            base.OnPointerMoved(e);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            draw.HandleReleased(e.GetCurrentPoint(this).Position, e.Pointer);
            base.OnPointerReleased(e);
        }

        private void ChangeOilPastel(object sender, RoutedEventArgs e)
        {
            Storyboard_oilpastel.Begin();
            control.ChangePenStyle(1);
        }

        private void ChangeWaterBrush(object sender, RoutedEventArgs e)
        {
            Storyboard_waterbrush.Begin();
            control.ChangePenStyle(2);
        }

        private void PutPicture(object sender, RoutedEventArgs e)
        {
            Storyboard_picturelib.Begin();
            control.ChangePenStyle(3);
        }

        private void DoEraser(object sender, RoutedEventArgs e)
        {
            Storyboard_eraser.Begin();
            factory.ChooseEraser();
            control.VisibleComponent("none");
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {

            if (this.Frame != null)
                this.Frame.Navigate(typeof(Main), new { A = email });

        }

        private void ColorItemClick(object sender, ItemClickEventArgs e)
        {
            Image image = (Image)e.ClickedItem;
            control.MakeColorItemSelected(image.Name);

        }

        private void PictureEnsure(object sender, ItemClickEventArgs e)
        {
            factory.DragPic = (Image)e.ClickedItem;
            control.SetPicSelected();
        }

        private void WriteText(object sender, RoutedEventArgs e)
        {
            Storyboard_writetext.Begin();
            control.VisibleComponent("MailGrid");
        }

        private void TextOkClick(object sender, RoutedEventArgs e)
        {
            control.VisibleComponent("none");
        }

        private void SendEmail(object sender, RoutedEventArgs e)
        {
            Storyboard_sendemail.Begin();
            control.VisibleComponent("QuestionGrid");
        }

        private void DragStart(object sender, DragItemsStartingEventArgs e)
        {
            factory.DragPic = (Image)e.Items.ElementAt(0);
            factory.IsDragingPic = true;
        }

        private void Click_ButtonYes(object sender, RoutedEventArgs e)
        {
            control.DecideSendPicture(draw);
        }

        private void Click_ButtonNo(object sender, RoutedEventArgs e)
        {
            control.DecideSendJustMessage();
        }

        private void Click_ButtonNew(object sender, RoutedEventArgs e)
        {
            Storyboard_new.Begin();
            control.Clear();
        }

        private void Clickover_sendemail(object sender, object e)
        {
            Storyboard_sendemail.Stop();
        }

        private void Clickover_writetext(object sender, object e)
        {
            Storyboard_writetext.Stop();
        }

        private void Clickover_picturelib(object sender, object e)
        {
            Storyboard_picturelib.Stop();
        }

        private void Clickover_takephoto(object sender, object e)
        {
            Storyboard_takephoto.Stop();
        }

        private void Clickover_eraser(object sender, object e)
        {
            Storyboard_eraser.Stop();
        }

        private void Clickover_waterbrush(object sender, object e)
        {
            Storyboard_waterbrush.Stop();
        }

        private void Clickover_oilpastel(object sender, object e)
        {
            Storyboard_oilpastel.Stop();
        }

        private void Clickover_new(object sender, object e)
        {
            Storyboard_new.Stop();
        }

        private void MailChoose(object sender, ItemClickEventArgs e)
        {
            MyUserControl item = (MyUserControl)e.ClickedItem;
            control.MailChoose(item);
        }

    }
}