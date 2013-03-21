using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.Serialization;
using System.IO;
using Windows.UI.Xaml.Media;
using Parse;


namespace KidsPainter
{
    class ComponentsControl
    {
        public FlagFactory factory;

        public Grid TopGrid = new Grid();
        public Grid BottomGrid = new Grid();
        public Grid LeftGrid = new Grid();
        public Grid RightGrid = new Grid();
        public Grid MailGrid = new Grid();

        public Grid QuestionGrid = new Grid();
        public Grid Painter = new Grid();
        public ListView ColorList = new ListView();
        public ListView MailList = new ListView();
        public RichEditBox MailBox = new RichEditBox();
        public GridView PictureView = new GridView();

        private Button Button_new = new Button();
        private Button Button_new_Copy = new Button();
        private Button Button_waterBrush = new Button();
        private Button Button_waterBrush_Copy = new Button();
        private Button Button_oilPastel = new Button();
        private Button Button_oilPastel_Copy = new Button();
        private Button Button_eraser = new Button();
        private Button Button_eraser_Copy = new Button();
        private Button Button_takePhoto = new Button();
        private Button Button_takePhoto_Copy = new Button();
        private Button Button_pictureLib = new Button();
        private Button Button_pictureLib_Copy = new Button();
        private Button Button_writeText = new Button();
        private Button Button_writeText_Copy = new Button();
        private Button Button_sendEmail = new Button();
        private Button Button_sendEmail_Copy = new Button();

        MediaElement MusicPlayer = new MediaElement();

        public ComponentsControl()
        {
            
        }


        public async void Initial()
        {
            
            StorageFolder Folder = ApplicationData.Current.LocalFolder;
            try
            {
                await Folder.CreateFolderAsync("Picture");

            }
            catch (System.Exception ex) { }

            try
            {
                await Folder.CreateFolderAsync("Temp");
            }
            catch (System.Exception ex) { }

            /*
            * 初始化界面尺寸,控件位置
            */

            this.Painter.Height = factory.PainterHeight; //改成整个屏幕的
            this.Painter.Width = factory.PainterWidth;
            this.Painter.Margin = new Thickness(0, 0, 0, 0);

            this.TopGrid.Height = 162;
            this.TopGrid.Width = factory.PainterWidth;
            this.TopGrid.Margin = new Thickness(0,0,0,0);

            this.LeftGrid.Height = factory.PainterHeight;
            this.LeftGrid.Width = 35;
            this.LeftGrid.Margin = new Thickness(0,0,0,0);

            this.RightGrid.Height = factory.PainterHeight;
            this.RightGrid.Width = 22;
            this.RightGrid.Margin = new Thickness(factory.PainterWidth-22,0,0,0);

            this.BottomGrid.Height = 32;
            this.BottomGrid.Width = factory.PainterWidth;
            this.BottomGrid.Margin = new Thickness(0, factory.PainterHeight - 32, 0, 0);


            double interval = factory.ScreenWidth / 13; ;
            this.Button_new.Margin = new Thickness(3 * interval, 35, 0, 0);
            this.Button_new_Copy.Margin = this.Button_new.Margin;
            this.Button_waterBrush.Margin = new Thickness(4 * interval, 35, 0, 0);
            this.Button_waterBrush_Copy.Margin = this.Button_waterBrush.Margin;
            this.Button_oilPastel.Margin = new Thickness(5 * interval, 35, 0, 0);
            this.Button_oilPastel_Copy.Margin = this.Button_oilPastel.Margin;
            this.Button_eraser.Margin = new Thickness(6 * interval, 35, 0, 0);
            this.Button_eraser_Copy.Margin = this.Button_eraser.Margin;
            this.Button_takePhoto.Margin = new Thickness(7 * interval, 35, 0, 0);
            this.Button_takePhoto_Copy.Margin = this.Button_takePhoto.Margin;
            this.Button_pictureLib.Margin = new Thickness(8 * interval, 35, 0, 0);
            this.Button_pictureLib_Copy.Margin = this.Button_pictureLib.Margin;
            this.Button_writeText.Margin = new Thickness(9 * interval, 35, 0, 0);
            this.Button_writeText_Copy.Margin = this.Button_writeText.Margin;
            this.Button_sendEmail.Margin = new Thickness(10 * interval, 35, 0, 0);
            this.Button_sendEmail_Copy.Margin = this.Button_sendEmail.Margin;

            this.MailGrid.Margin = new Thickness((this.Painter.Width - this.MailGrid.Width) / 2,
                (this.Painter.Height - this.MailGrid.Height) / 2, 0, 0);

            this.QuestionGrid.Margin = new Thickness((this.Painter.Width - this.QuestionGrid.Width) / 2,
                (this.Painter.Height - this.QuestionGrid.Height) / 2, 0, 0);

            this.PictureView.Margin = new Thickness((this.Painter.Width - this.PictureView.Width) / 2,
                this.Painter.Height - this.PictureView.Height - 20, 0, 0);

            this.ColorList.Margin = new Thickness(this.Painter.Width - 80-this.ColorList.Width - 30, 190, 0, 0);

            this.MailList.Margin = new Thickness((this.Painter.Width - this.MailList.Width) / 2,
                (this.Painter.Height - this.MailList.Height) / 2, 0, 0);
            /*
            this.ColorList.Visibility = Visibility.Collapsed;
            this.MailGrid.Visibility = Visibility.Collapsed;
            this.MailBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
            this.QuestionGrid.Visibility = Visibility.Collapsed;
            this.MailList.Visibility = Visibility.Collapsed;
            this.PictureView.Visibility = Visibility.Collapsed;
            */

            //背景音乐
            MusicPlayer.Play();
        }

        public async void TakePhoto()
        {
            ColorList.Visibility = Visibility.Collapsed;
            MailGrid.Visibility = Visibility.Collapsed;
            PictureView.Visibility = Visibility.Collapsed;
            QuestionGrid.Visibility = Visibility.Collapsed;

            PhotoTaker phototaker = new PhotoTaker();
            PhotoTaker phTaker = new PhotoTaker();
            Task<ReturnElements> task = phTaker.takePhoto(factory.ScreenWidth, factory.ScreenHeight);
            ReturnElements returnEle = await task;
            Image image = new Image();
            image.Source = returnEle.bitmapImage;
            image.Stretch = Stretch.Fill;
            Painter.Children.Add(image);
        }

        public async void DecideSendPicture(DrawShape draw)
        {
            QuestionGrid.Visibility = Visibility.Collapsed;

            Task<ReturnElements> task = draw.GetScreen();
            ReturnElements ret = await task;

            if (ret.success)
            {
                //将流上传到服务器上
                var test = new ParseObject("test");
                test["HEHHEHEH"] = "test";

                test.SaveAsync();



                //Stream bmpImage = WindowsRuntimeStreamExtensions.AsStreamForRead(imageStream.GetInputStreamAt(0));
                ParseFile imgFile = new ParseFile("Painter.bmp", ret.imagestream);

                await imgFile.SaveAsync();

                var paintersObj = new ParseObject("Paints");
                paintersObj["imageFile"] = imgFile;
                paintersObj["user"] = ParseUser.CurrentUser.Username;
                paintersObj["nickName"] = ParseUser.CurrentUser["nickName"];

                await paintersObj.SaveAsync();

                Message.ShowToast("发布成功");

                factory.strContent = "";
                MailBox.Document.Selection.StartPosition = 0;
                MailBox.Document.GetText(Windows.UI.Text.TextGetOptions.None, out factory.strContent);

                // 获得图片，发送邮件,图片为MyPainter.png，邮件文本可通过MailBox.Document.GetText()来得到
                factory.HasPicture = true;
                factory.IsGetScreenSucccess = false;
                CreateMailList();
            }
            else
            {
                Message.ShowToast("获取您的涂鸦作品失败！请按截屏键！");
            }
        }

        public void DecideSendJustMessage()
        {
            //不想发送图片，单纯发送文本
            QuestionGrid.Visibility = Visibility.Collapsed;
            factory.strContent = "";
            MailBox.Document.Selection.StartPosition = 0;
            MailBox.Document.GetText(Windows.UI.Text.TextGetOptions.None, out factory.strContent);

            //检测文本是否为空,从MailBox得到的文本，至少有一个'\r'字符
            bool Flag = true;
            if (factory.strContent.Length == 1 || factory.strContent.Length == 0)
                Flag = true;
            else
            {
                for (int i = 0; i < factory.strContent.Length; i++)
                {
                    if (factory.strContent[i] != ' ')
                    {
                        Flag = false;
                        break;
                    }
                }
            }

            //文本为空
            if (Flag)
                Message.ShowToast("对不起，您的邮件内容为空。");
            else
            {
                //发送文本邮件
                CreateMailList();
                factory.HasPicture = false;
            }
        }

        public void SetPicSelected()
        {
            switch (factory.DragPic.Name)
            {
                case "cat":
                    PictureView.SelectedIndex = 0;
                    break;
                case "candy":
                    PictureView.SelectedIndex = 1;
                    break;
                case "Mario":
                    PictureView.SelectedIndex = 2;
                    break;
                case "pikachu":
                    PictureView.SelectedIndex = 3;
                    break;
                case "mushroom":
                    PictureView.SelectedIndex = 4;
                    break;
                case "star":
                    PictureView.SelectedIndex = 5;
                    break;
                case "dog":
                    PictureView.SelectedIndex = 6;
                    break;
                case "cow":
                    PictureView.SelectedIndex = 7;
                    break;
                case "dragon":
                    PictureView.SelectedIndex = 8;
                    break;
                case "monkey":
                    PictureView.SelectedIndex = 9;
                    break;
                case "moon":
                    PictureView.SelectedIndex = 10;
                    break;
                case "mouse":
                    PictureView.SelectedIndex = 11;
                    break;
                case "ninja":
                    PictureView.SelectedIndex = 12;
                    break;
                case "pig":
                    PictureView.SelectedIndex = 13;
                    break;
            }

            PictureView.Visibility = Visibility.Collapsed;
        }

        private String ColorListFromIndexToName()
        {
            switch (ColorList.SelectedIndex)
            {
                case 0:
                    return "Blue";
                case 1:
                    return "Yellow";
                case 2:
                    return "Green";
                case 3:
                    return "Red";
                case 4:
                    return "Blone";
                case 5:
                    return "Skyblue";
                case 6:
                    return "Purpuse";
                case 7:
                    return "Gray";
                case 8:
                    return "Black";
            }

            return "";
        }

        

        public async void CreateMailList()
        {
            MailList.Items.Clear();
            MailList.Visibility = Visibility.Visible;
            List<string[]> storeList = new List<string[]>();
            string[] storeString = new string[3];

            try
            {
                StoreSetting storeSetting = new StoreSetting();
                Task<int> task = storeSetting.GetStoreListCount();
                int count = await task;
                if (count == 0)
                {
                    Message.ShowToast("通讯录为空，请设置！");
                    return;
                }

                storeList = storeSetting.getStoreList();
                for (int i = 0; i < count; i++)
                {
                    MyUserControl item = new MyUserControl(storeList[i][0], storeList[i][2]);
                    MailList.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Message.ShowToast("通讯录为空，请设置！");
            }
        }

        public void VisibleComponent(String name)
        {
            MailGrid.Visibility = Visibility.Collapsed;
            QuestionGrid.Visibility = Visibility.Collapsed;
            ColorList.Visibility = Visibility.Collapsed;
            MailList.Visibility = Visibility.Collapsed;
            PictureView.Visibility = Visibility.Collapsed;

            if (name.Equals("none"))
                return;
            else if (name.Equals("MailGrid"))
                MailGrid.Visibility = Visibility.Visible;
            else if (name.Equals("QuestionGrid"))
                QuestionGrid.Visibility = Visibility.Visible;
            else if (name.Equals("ColorList"))
                ColorList.Visibility = Visibility.Visible;
            else if (name.Equals("MailList"))
                MailList.Visibility = Visibility.Visible;
            else if (name.Equals("PictureView"))
                PictureView.Visibility = Visibility.Visible;
        }

        public void MailChoose(MyUserControl item)
        {
            String email = item.getEmail();
            if (email.Equals(""))
                return;
            else
            {
                factory.strSendTo = email;
                MailList.Visibility = Visibility.Collapsed;

                SendMail send = new SendMail();
                send.sendMail(factory.strSendTo, factory.strContent, factory.HasPicture);
                MailBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                Message.ShowToast("邮件正在发送中....");
            }
        }

        public void MakeColorItemSelected(string name)
        {
            switch (name)
            {
                case "Blue":
                    ColorList.SelectedIndex = 0;
                    break;
                case "Yellow":
                    ColorList.SelectedIndex = 1;
                    break;
                case "Green":
                    ColorList.SelectedIndex = 2;
                    break;
                case "Red":
                    ColorList.SelectedIndex = 3;
                    break;
                case "Blone":
                    ColorList.SelectedIndex = 4;
                    break;
                case "Skyblue":
                    ColorList.SelectedIndex = 5;
                    break;
                case "Purpuse":
                    ColorList.SelectedIndex = 6;
                    break;
                case "Gray":
                    ColorList.SelectedIndex = 7;
                    break;
                case "Black":
                    ColorList.SelectedIndex = 8;
                    break;
            }

            factory.CurrentPenColor = name;
            VisibleComponent("none");
        }

        public void Clear()
        {
            this.Painter.Children.Clear();
            this.Painter.Children.Add(MailGrid);
            this.Painter.Children.Add(PictureView);
            this.Painter.Children.Add(QuestionGrid);
            this.Painter.Children.Add(ColorList);
        }

        public void ChangePenStyle(int type)
        {
            factory.CurrentPenColor = ColorListFromIndexToName();
            factory.CurrentPenStyle = type;
            if (type != 3)
            {
                if (ColorList.Visibility == Visibility.Visible)
                    ColorList.Visibility = Visibility.Collapsed;
                else
                    ColorList.Visibility = Visibility.Visible;
                PictureView.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (PictureView.Visibility == Visibility.Visible)
                    PictureView.Visibility = Visibility.Collapsed;
                else
                    PictureView.Visibility = Visibility.Visible;
                ColorList.Visibility = Visibility.Collapsed;
            }
            MailGrid.Visibility = Visibility.Collapsed;
            QuestionGrid.Visibility = Visibility.Collapsed;
        }

        public void SetFlagFactory(FlagFactory factory)
        {
            this.factory = factory;
        }

        public void SetMailGrid(Grid MailGrid)
        {
            this.MailGrid = MailGrid;
        }

        public void SetQuestionGrid(Grid QuestionGrid)
        {
            this.QuestionGrid = QuestionGrid;
        }

        public void SetPainter(Grid Painter)
        {
            this.Painter = Painter;
        }

        public void SetColorList(ListView ColorList)
        {
            this.ColorList = ColorList;
        }

        public void SetMailList(ListView MailList)
        {
            this.MailList = MailList;
        }

        public void SetMailBox(RichEditBox MailBox)
        {
            this.MailBox = MailBox;
        }

        public void SetPictureView(GridView PictureView)
        {
            this.PictureView = PictureView;
        }

        public void SetButton_new(Button original, Button copy)
        {
            this.Button_new = original;
            this.Button_new_Copy = copy;
        }

        public void SetButton_waterBrush(Button original, Button copy)
        {
            this.Button_waterBrush = original;
            this.Button_waterBrush_Copy = copy;
        }

        public void SetButton_oilPastel(Button original, Button copy)
        {
            this.Button_oilPastel = original;
            this.Button_oilPastel_Copy = copy;
        }

        public void SetButton_eraser(Button original, Button copy)
        {
            this.Button_eraser = original;
            this.Button_eraser_Copy = copy;
        }

        public void SetButton_takephoto(Button original, Button copy)
        {
            this.Button_takePhoto = original;
            this.Button_takePhoto_Copy = copy;
        }

        public void SetButton_pictureLib(Button original, Button copy)
        {
            this.Button_pictureLib = original;
            this.Button_pictureLib_Copy = copy;
        }

        public void SetButton_writeText(Button original, Button copy)
        {
            this.Button_writeText = original;
            this.Button_writeText_Copy = copy;
        }

        public void SetButton_sendEmail(Button original, Button copy)
        {
            this.Button_sendEmail = original;
            this.Button_sendEmail_Copy = copy;
        }

    }
}
