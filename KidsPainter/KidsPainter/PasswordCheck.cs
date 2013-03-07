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
    class PasswordCheck
    {
        private string checkResult;
        public async Task<string> check()
        {
             try
            {
                StorageFile file2 = await ApplicationData.Current.LocalFolder.GetFileAsync("IfPdExisted.xml");
                IRandomAccessStream stream2 = await file2.OpenReadAsync();
                using (IInputStream inStream = stream2.GetInputStreamAt(0))
                {
                    DataContractSerializer serializer2 = new DataContractSerializer(typeof(string));
                    checkResult = (string)serializer2.ReadObject(inStream.AsStreamForRead());
                }
                 return checkResult;
            }
            catch
            {
                checkResult = "";
                return checkResult;
            }
        }
    }
}
