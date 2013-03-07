using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace KidsPainter
{
    class DrawShape
    {
        private ComponentsControl control;

        public DrawShape()
        {

        }

        public void SetComponentControl(ComponentsControl control)
        {
            this.control = control;
        }

        public async Task<ReturnElements> GetScreen()
        {
            ReturnElements retelement = new ReturnElements();

            // Get the bitmap and display it.
            var dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Bitmap))
            {
                IRandomAccessStreamReference imageReceived = null;
                try
                {
                    imageReceived = await dataPackageView.GetBitmapAsync();

                    if (imageReceived != null)
                    {
                        using (IRandomAccessStream imageStream = await imageReceived.OpenReadAsync())
                        {
                            StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Temp");
                            if (folder == null)
                                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Temp", CreationCollisionOption.ReplaceExisting);

                            StorageFile file = await folder.CreateFileAsync("MyPainter.png", CreationCollisionOption.ReplaceExisting);
                            Windows.Storage.Streams.Buffer buffer = new Windows.Storage.Streams.Buffer((uint)imageStream.Size);
                            await imageStream.ReadAsync(buffer, (uint)imageStream.Size, InputStreamOptions.None);
                            await FileIO.WriteBufferAsync(file, buffer);
                            control.factory.IsGetScreenSucccess = true;
                            retelement.success = true;
                            return retelement;
                        }
                    }
                }
                catch (Exception ex)
                {
                    control.factory.IsGetScreenSucccess = false;
                    retelement.success = false;
                    return retelement;
                }

            }
            control.factory.IsGetScreenSucccess = false;
            retelement.success = false;
            return retelement;
        }

        public bool IsOnPainter(Point p)
        {
            if (p.X > control.Painter.Margin.Left && p.X < control.Painter.Margin.Left + control.Painter.ActualWidth
               && p.Y > control.Painter.Margin.Top && p.Y < control.Painter.Margin.Top + control.Painter.ActualHeight)
                return true;
            else
                return false;
        }

        public void DrawOilPastel(Point position)
        {
            Random rand = new Random();
            int count = 100 + rand.Next() % 50;

            FlagFactory factory = control.factory;
            Color color = factory.GetCurrentColor();
            SolidColorBrush brush = new SolidColorBrush(color);
            brush.Opacity = factory.CurrentOpacity;
            for (int i = 0; i < count; i++)
            {
                double x = position.X + rand.Next() % 30;
                double y = position.Y + rand.Next() % 30;

                Ellipse ellipse = new Ellipse();

                int radius = 3 + rand.Next() % 2;
                ellipse.Fill = brush;
                ellipse.Width = radius;
                ellipse.Height = radius;
                ellipse.Opacity = factory.CurrentOpacity;
                ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                ellipse.VerticalAlignment = VerticalAlignment.Top;
                ellipse.Margin = new Thickness(x, y, 0, 0);

                control.Painter.Children.Add(ellipse);
            }
        }

        public void HandleMoved(Point _position,Pointer _pointer)
        {
            FlagFactory factory = control.factory;
            Pointer pointer = _pointer;
            if (factory.CannotDraw())
                return;

            Point IllusionPoint = _position;
            Point point = new Point();
            point.X = IllusionPoint.X - control.Painter.Margin.Left;
            point.Y = IllusionPoint.Y - control.Painter.Margin.Top;

            if (!IsOnPainter(IllusionPoint))
            {

                if (!factory.MoveOut)
                {
                    factory.MoveOut = true;
                    uint id = pointer.PointerId;

                    if (factory.pointerDictionary.ContainsKey(id))
                        factory.pointerDictionary.Remove(id);
                }

                return;
            }

            if (factory.MoveOut)
            {
                factory.MoveOut = false;
                if (factory.Pressed && !factory.ReleaseAtOut)
                   HandlePressed(_position,_pointer);
                return;
            }

            if (factory.IsWaterbrushOrEraser())
            {
                uint id = pointer.PointerId;

                if (factory.pointerDictionary.ContainsKey(id))
                    factory.pointerDictionary[id].Points.Add(point);
            }

            else if (factory.CurrentPenStyle == 1 && factory.Pressed)
            {
                DrawOilPastel(point);
            }
        }

        public void HandlePressed(Point _position, Pointer _pointer)
        {
            FlagFactory factory = control.factory;
            Point IllusionPoint = _position;
            if (!IsOnPainter(IllusionPoint))
                return;

            if (factory.CanPutStamp())
            {
                Image image = new Image();
                image.Source = factory.DragPic.Source;
                image.VerticalAlignment = VerticalAlignment.Top;
                image.HorizontalAlignment = HorizontalAlignment.Left;
                image.Stretch = Stretch.None;
                image.Margin = new Thickness(IllusionPoint.X - control.Painter.Margin.Left, IllusionPoint.Y - control.Painter.Margin.Top, 0, 0);
                control.Painter.Children.Add(image);
            }

            if (factory.CurrentPenColor.Equals(""))
                return;

            Point point = new Point();
            point.X = IllusionPoint.X - control.Painter.Margin.Left;
            point.Y = IllusionPoint.Y - control.Painter.Margin.Top;
            if (factory.IsWaterbrushOrEraser())
            {
                uint id = _pointer.PointerId;
                if (!factory.pointerDictionary.ContainsKey(id))
                {
                    Color color = factory.GetCurrentColor();

                    SolidColorBrush brush = new SolidColorBrush(color);
                    brush.Opacity = factory.CurrentOpacity;

                    Polyline polyline = new Polyline
                    {
                        Stroke = brush,
                        StrokeThickness = 20,
                    };
                    polyline.Points.Add(point);
                    control.Painter.Children.Add(polyline);
                    factory.pointerDictionary.Add(id, polyline);
                }
            }

            else if (factory.IsOilpastel())
            {
                DrawOilPastel(point);
            }

            factory.Pressed = true;
        }

        public void HandleReleased(Point position,Pointer pointer)
        {
            FlagFactory factory = control.factory;
            if (factory.IsWaterbrushOrEraser())
            {
                uint id = pointer.PointerId;

                if (factory.pointerDictionary.ContainsKey(id))
                    factory.pointerDictionary.Remove(id);
            }

            if (!IsOnPainter(position))
                factory.ReleaseAtOut = true;
            else
                factory.ReleaseAtOut = false;

            factory.Pressed = false;
        }
    }
}
