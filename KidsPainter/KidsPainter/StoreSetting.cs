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
    class StoreSetting
    {
        private List<string[]> storeList = new List<string[]>();
        private string[] storeString = new string[3];
        private string password;


        public List<string[]> getStoreList()
        {
            return storeList;
        }

        public async void  CheckStoreList()
        {
            try
            {
                StorageFile file2 = await ApplicationData.Current.LocalFolder.GetFileAsync("storeParent.xml");
                IRandomAccessStream stream2 = await file2.OpenReadAsync();
                using (IInputStream inStream = stream2.GetInputStreamAt(0))
                {
                    DataContractSerializer serializer2 = new DataContractSerializer(typeof(List<string[]>));
                    storeList = (List<string[]>)serializer2.ReadObject(inStream.AsStreamForRead());
                    
                }
            }
            catch
            {
            }
        }

        public async Task<int> GetStoreListCount()
        {
            StorageFile file2 = await ApplicationData.Current.LocalFolder.GetFileAsync("storeParent.xml");
            IRandomAccessStream stream2 = await file2.OpenReadAsync();
            using (IInputStream inStream = stream2.GetInputStreamAt(0))
            {
                DataContractSerializer serializer2 = new DataContractSerializer(typeof(List<string[]>));
                storeList = (List<string[]>)serializer2.ReadObject(inStream.AsStreamForRead());

                return storeList.Count;
            }
        }

        public async void store(string pName, string pEmail, string PicName)
        {
            CheckStoreList();
            storeString[0] = pName;
            storeString[1] = pEmail;
            storeString[2] = PicName;
            storeList.Add(storeString);
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("storeParent.xml", CreationCollisionOption.ReplaceExisting);
            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            using (IOutputStream outStream = stream.GetOutputStreamAt(0))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<string[]>));
                serializer.WriteObject(outStream.AsStreamForWrite(), storeList);
                await outStream.FlushAsync();
            }
        }
  }
}
