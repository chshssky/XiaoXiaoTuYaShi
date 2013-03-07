using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using Painter;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace KidsPainter
{
    public sealed partial class MyUserControl : UserControl
    {
        private String call = "";
        private String email = "";

        public MyUserControl(String call,String path)
        {
            this.InitializeComponent();

            this.call = call;
            InitialItem(call,path);
        }

        async public void InitialItem(String call,String path)
        {
            this.Mycall.Text = call;

            StorageFolder folder =  await ApplicationData.Current.LocalFolder.GetFolderAsync("Picture");
            
            StorageFile file = await folder.GetFileAsync(getName(path));
            IRandomAccessStream imageStream = await file.OpenReadAsync();
            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.SetSource(imageStream);
            this.Myimage.Source = bitmapimage;
        }

        private String getName(String path)
        {
            String name = "";

            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '\\')
                    break;

                name = path[i] + name;
            }

            return name;
        }

        public String getEmail()
        {
            Search();
            return email;
        }

        private async void Search()
        {

            List<string[]> storeList = new List<string[]>();
            string[] storeString = new string[3];
            string test;

            try
            {
                StorageFile file2 = await ApplicationData.Current.LocalFolder.GetFileAsync("storeParent.xml");
                IRandomAccessStream stream2 = await file2.OpenReadAsync();
                using (IInputStream inStream = stream2.GetInputStreamAt(0))
                {
                    DataContractSerializer serializer2 = new DataContractSerializer(typeof(List<string[]>));
                    storeList = (List<string[]>)serializer2.ReadObject(inStream.AsStreamForRead());

                    int count = storeList.Count;

                    for (int i = 0; i < count; i++)
                    {
                        if (storeList[i][0].Equals(call))
                        {
                            email = storeList[i][1];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
