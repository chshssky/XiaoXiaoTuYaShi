using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.Serialization;
using System.IO;
namespace KidsPainter
{
    class AgreementControl
    {
        public int checkResult=1;
        public string fileName = "IfAgree.xml";
        private int flag=0;   //flag =0 表示用户还没同意协议，flag = 1 表示同意
        public async void WriteIntoFile(int a)     // 把用户选择的结果记录入文件，下次打开程序的时候验证
        {
            flag = a;
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            using (IOutputStream outStream = stream.GetOutputStreamAt(0))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(int));
                serializer.WriteObject(outStream.AsStreamForWrite(), flag);
                await outStream.FlushAsync();
            }
        }
        public async void checkAgreement()  //检测上次是否同意协议，或者是否第一次运行该程序
        {
            try
            {
                StorageFile file2 = await ApplicationData.Current.LocalFolder.GetFileAsync("IfAgree.xml");
                IRandomAccessStream stream2 = await file2.OpenReadAsync();
                using (IInputStream inStream = stream2.GetInputStreamAt(0))
                {
                    DataContractSerializer serializer2 = new DataContractSerializer(typeof(int));
                    checkResult = (int)serializer2.ReadObject(inStream.AsStreamForRead());
                }
              //  stream2.Dispose();//ApplicationData.Current.LocalFolder.
            }
            catch
            {
                checkResult = 0;
            }
        }
    }
}
