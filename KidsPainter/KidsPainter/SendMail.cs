using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;



namespace KidsPainter
{
    class SendMail
    {
        private string hostIP = "94.249.188.107";
        private string port = "7799";
        public void setHostIP(string ip)
        {
            hostIP = ip;
        }
        public async void sendMail(string strSendTo,string strContent , Boolean hasPicture)
        {
            HostName hostName = new HostName(hostIP);
            StreamSocket socket = new StreamSocket();
            List<string[]> storeList = new List<string[]>();
            try
            {
                await socket.ConnectAsync(hostName, port);
            }
            catch (Exception exception)
            {
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
            }
            StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Temp");
            StorageFile pic = await folder.GetFileAsync("MyPainter.png");
            IBuffer buffer = await FileIO.ReadBufferAsync(pic);
            DataWriter writer = new DataWriter(socket.OutputStream);
            writer.WriteUInt32(writer.MeasureString(strSendTo));
            writer.WriteString(strSendTo);
            writer.WriteUInt32(writer.MeasureString(strContent));
            writer.WriteString(strContent);
            if (hasPicture)
            {
                writer.WriteUInt32((uint)buffer.Length);
                writer.WriteBuffer(buffer);
            }
            else
            {
                writer.WriteUInt32(0);
                writer.WriteString("");
            }
            await writer.StoreAsync();
            writer.DetachStream();
            writer.Dispose();
            socket.Dispose();
        }
    }
}
