using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;

namespace KidsPainter
{
    class StorePassword
    {
        private bool storeSuccess;
        public async Task<bool> store(string password)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("IfPdExisted.xml", CreationCollisionOption.ReplaceExisting);
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
                using (IOutputStream outStream = stream.GetOutputStreamAt(0))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(string));
                    serializer.WriteObject(outStream.AsStreamForWrite(), password);
                    await outStream.FlushAsync();
                }
                storeSuccess = true;
                return storeSuccess;
            }
            catch 
            {
                storeSuccess = false;
                return storeSuccess;
            }
        }
    }
}
