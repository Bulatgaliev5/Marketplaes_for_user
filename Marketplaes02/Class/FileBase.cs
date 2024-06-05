using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;


namespace Marketplaes02.Class
{
    public class FileBase
    {
        private string _host;
        private int _port;
        private string _username;
        private string _password;




        public FileBase()
        {
            _host = "37.18.74.116";
            _port = 21;
            _username = "p101_f_ilnar";
            _password = "Qwerty123";
        }

        public FileBase(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;


        }


        public void UploadFile(string localPath, string remotePath)
        {

            FtpWebRequest
                request = (FtpWebRequest)WebRequest.Create("ftp://" + _host + ":" + _port + "/" + remotePath);

            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(_username, _password);

            byte[]
                buffer = new byte[1024];
            FileStream
                localStream = File.OpenRead(localPath);
            Stream
                requestStream = request.GetRequestStream();
            long
                totalBytesUploaded = 0;
            int
                bytesRead;

            System.Timers.Timer
                timer = new System.Timers.Timer(100); // Интервал обновления в миллисекундах

            timer.Start();

            while ((bytesRead = localStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                requestStream.Write(buffer, 0, bytesRead);
                totalBytesUploaded += bytesRead;
            }

            timer.Stop();
            requestStream.Close();
        }

        public async Task DownloadFileAsync(string remotePath, string localPath)
        {
            FtpWebRequest
                request = (FtpWebRequest)WebRequest.Create("ftp://" + _host + ":" + _port + "/" + remotePath);

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(_username, _password);
            FtpWebResponse
                response = (FtpWebResponse)await request.GetResponseAsync();
            Stream
                responseStream = response.GetResponseStream();
            FileStream
                localStream = File.OpenWrite(localPath);
            byte[]
                buffer = new byte[1024];
            long
                totalBytesDownloaded = 0;
            int
                bytesRead;
            System.Timers.Timer timer = new System.Timers.Timer(100); // Интервал обновления в миллисекундах
            timer.Start();

            while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                localStream.Write(buffer, 0, bytesRead);
                totalBytesDownloaded += bytesRead;
            }

            timer.Stop();
            localStream.Close();
        }

        public async Task<ImageSource> LoadImageFromFtpAsync(string remotePath)
        {
            var imageManagerDiskCache = Path.Combine(FileSystem.CacheDirectory, "image_manager_disk_cache");

            if (Directory.Exists(imageManagerDiskCache))
            {
                foreach (var imageCacheFile in Directory.EnumerateFiles(imageManagerDiskCache))
                {
                    File.Delete(imageCacheFile);
                }
            }
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + _host + ":" + _port + "/Bulat_files/" + remotePath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(_username, _password);

            ImageSource streamImageSource = null;

            try
            {
                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                Stream responseStream = response.GetResponseStream();
               
                    MemoryStream memoryStream = new MemoryStream();
                    await responseStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    // Создаем новый MemoryStream и копируем в него содержимое
                    MemoryStream copyStream = new MemoryStream(memoryStream.ToArray());
                    streamImageSource = (StreamImageSource)ImageSource.FromStream(() => copyStream);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при загрузке изображения: " + ex.Message);
                return null;
            }

            return streamImageSource;
        }



        public string GetShareableImageLink(string remotePath)
        {
            string
                shareLink = "ftp://" + _username + ":" + _password + "@" + _host + ":" + _port + "/Bulat_files/" + WebUtility.UrlEncode(remotePath);

            return shareLink;
        }


    }
}
