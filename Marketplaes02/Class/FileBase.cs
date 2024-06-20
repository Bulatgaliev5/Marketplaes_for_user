using System.Diagnostics;
using System.Net;



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



        public string LoadImageFromFtpAndSaveAsync(string fileName)
        {

            WebRequest request = WebRequest.Create("ftp://" + _host + ":" + _port + "/Bulat_files/" + fileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(_username, _password);
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            try
            {
                WebResponse response =  request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                using (var fileStream = File.OpenWrite(filePath))
                {
                     responseStream.CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при загрузке изображения: " + ex.Message);
                return null;
            }
            return  filePath;
        }

        public async Task<ImageSource> LoadImageAsync(string fileName)
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                if (File.Exists(filePath))
                {
                    return ImageSource.FromFile(filePath);
                }
                else
                {
                    // Файл отсутствует в кеше, загружаем его из FTP и сохраняем в файловую систему
                    filePath =  LoadImageFromFtpAndSaveAsync(fileName);
                    return ImageSource.FromFile(filePath);
                }
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Уведомление", ex.Message, "ОK");

            }
            return null;
        }
    }
}
