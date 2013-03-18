using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace KidsPainter
{
    class FlagFactory
    {
        public double ScreenWidth;
        public double ScreenHeight;
        public String CurrentPenColor;
        public int CurrentPenStyle;
        public int CurrentOpacity;
        public bool MoveOut;              //鼠标移出画板范围
        public bool Pressed;
        public bool ReleaseAtOut;
        public bool IsDragingPic;
        public bool IsGetScreenSucccess;
        public string strContent;
        public string strSendTo;
        public bool HasPicture = false;
        public WriteableBitmap BitmapOutput;
        public Dictionary<uint, Polyline> pointerDictionary;
        public Image DragPic;

        public double PainterWidth;
        public double PainterHeight;

        public FlagFactory()
        {
            /*
             * CurrentPenStyle = 0   不画东西
            * CurrentPenStyle = 1   油画棒风格
            * CurrentPenStyle = 2   水彩风格
            * CurrentPenStyle = 3   印章风格
            * CurrentPenStyle = 4   橡皮擦
             */

            CurrentPenStyle = 0;
            CurrentPenColor = "";
            CurrentOpacity = 100;

            ScreenHeight = Window.Current.Bounds.Height;
            ScreenWidth = Window.Current.Bounds.Width;
            pointerDictionary = new Dictionary<uint, Polyline>();
            MoveOut = false;
            Pressed = false;
            ReleaseAtOut = false;
            IsDragingPic = false;
            DragPic = null;
            IsGetScreenSucccess = false;
            strContent = "";
            PainterHeight = ScreenHeight; //改成整个屏幕的
            PainterWidth = ScreenWidth;
            BitmapOutput = new WriteableBitmap((int)PainterWidth,(int)PainterHeight);
        }

        public Color GetCurrentColor()
        {
            switch (CurrentPenColor)
            {
                case "White":
                    return Color.FromArgb(255, (byte)255, (byte)255, (byte)255);
                case "Red":
                    return Color.FromArgb(255, (byte)255, (byte)0, (byte)0);
                case "Green":
                    return Color.FromArgb(255, (byte)0, (byte)255, (byte)0);
                case "Blue":
                    return Color.FromArgb(255, (byte)0, (byte)120, (byte)255);
                case "Gray":
                    return Color.FromArgb(255, (byte)200, (byte)200, (byte)200);
                case "Black":
                    return Color.FromArgb(255, (byte)0, (byte)0, (byte)0);
                case "Blone":
                    return Color.FromArgb(255, (byte)250, (byte)210, (byte)40);
                case "Skyblue":
                    return Color.FromArgb(255, (byte)0, (byte)255, (byte)255);
                case "Purpuse":
                    return Color.FromArgb(255, (byte)155, (byte)0, (byte)155);
                case "Yellow":
                    return Color.FromArgb(255, (byte)255, (byte)255, (byte)0);
            }

            return Color.FromArgb(255, (byte)255, (byte)255, (byte)255);
        }

        public bool IsWaterbrushOrEraser()
        {
            if (CurrentPenStyle == 2 || CurrentPenStyle == 4)
                return true;
            else
                return false;
        }

        public bool CanPutStamp()
        {
            if (CurrentPenStyle == 3 && DragPic != null)
                return true;
            else
                return false;
        }

        public bool IsOilpastel()
        {
            if(CurrentPenStyle == 1)
                return true;
            else 
                return false;
        }

        public bool CannotDraw()
        {
            if (CurrentPenColor.Equals("") || !Pressed)
                return true;
            else
                return false;
        }

        public void ChooseEraser()
        {
            CurrentPenStyle = 4;
            CurrentPenColor = "White";
        }
    }
}
