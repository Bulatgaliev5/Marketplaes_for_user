using Marketplaes02.BD;
using Marketplaes02.Model;
using MySqlConnector;
using System.ComponentModel;

namespace Marketplaes02.ViewModel
{
    public class ViewModelImagesGoods : INotifyPropertyChanged
    {
        int Kartochka_ID_goods;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Список Good
        /// </summary>
        private IList<ImagesGoods> _ImagesGoodsList;
        public IList<ImagesGoods> ImagesGoodsList
        {
            get => _ImagesGoodsList;
            set
            {
                _ImagesGoodsList = value;
                OnPropertyChanged("ImagesGoodsList");
            }
        }
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
        public ViewModelImagesGoods()
        {
            Kartochka_ID_goods = Preferences.Default.Get("Kartochka_ID_goods", 0);

            Load();

        }


        /// <summary>
        /// Метод загрузки изделтй Load
        /// </summary>
        public async void Load()
        {

            await ImagesGoodsSelectSQL(Kartochka_ID_goods);

        }
        /// <summary>
        /// Метод Получения изделий из БД
        /// </summary>
        /// <returns></returns>

        public async Task<bool> ImagesGoodsSelectSQL(int id)
        {
            string
              sql = "SELECT * FROM imagesgoods WHERE  ID_goods=@id";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id", id));
            await conn.GetConnectBD();
            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();
            Class.FileBase fileBase = new Class.FileBase();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Синхронное отключение от БД
                await conn.GetCloseBD();
                // Возращение false
                return false;
            }
                         
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {

              

                ImagesGoodsList.Add(new ImagesGoods()
                {
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                    ImageGoods = await fileBase.LoadImageAsync(reader["Image"].ToString()),

                    ImageID = Convert.ToInt32(reader["ImageID"]),
  
                });


            }
            OnPropertyChanged("ImagesGoodsList");

            await conn.GetCloseBD();
            return true;
        }



    }
}
