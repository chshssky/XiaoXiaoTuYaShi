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
using Windows.Storage.Pickers;
using Windows.Storage.FileProperties;
using Windows.Storage;
using Painter;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.ApplicationModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace KidsPainter
{

    class PhotoTaker
    {
        private CameraCaptureUI dialog;
        public PhotoTaker()
        {
            dialog = new CameraCaptureUI();

        }
        public async Task<ReturnElements> takePhoto(double width, double height)
        {
            try
            {
                dialog = new CameraCaptureUI();
                Size aspectRatio = new Size(width, height);
                dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;
                dialog.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Png;

                StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);


                    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        ReturnElements ReElements = new ReturnElements();
                        ReElements.bitmapImage.SetSource(fileStream);
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
                            folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Picture");
                        await file.MoveAsync(folder);
                        ReElements.filename = file.Name;
                        return ReElements;

                }
            }
            catch (System.Exception ex)
            {
                ReturnElements appEle = new ReturnElements();
                appEle.bitmapImage = null;
                appEle.filename = "cannot find";
                return appEle;
            }
        }

    }
}
