using Marketplaes02.BD;
using Marketplaes02.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02.ViewModel
{
    public class ViewModelKartochkaGood: KartochkaGood,  INotifyPropertyChanged
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
        public ViewModelKartochkaGood()
        {
            Kartochka_ID_goods = Preferences.Default.Get("Kartochka_ID_goods",0);
           
            Load();

        }


        /// <summary>
        /// Метод загрузки изделтй Load
        /// </summary>
        public async void Load()
        {
            await GoodsSelectSQL(Kartochka_ID_goods);
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
                 reader = cmd.ExecuteReader();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Синхронное отключение от БД
                await conn.GetCloseBD();
                // Возращение false
                return false;
            }
            ImagesGoodsList = new ObservableCollection<ImagesGoods>();
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (reader.Read())
            {

                ImagesGoodsList.Add(new ImagesGoods()
                {
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                    ImageID = Convert.ToInt32(reader["ImageID"]),
                    ImageGoods = reader["Image"].ToString(),
                });

                
            }
            OnPropertyChanged("ImagesGoodsList");

            await conn.GetCloseBD();
           
            return true;
        }


        public async Task<bool> GoodsSelectSQL(int id)
        {
            string
              sql = "SELECT * FROM goods WHERE  ID_goods=@id";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id", id));
            await conn.GetConnectBD();
            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = cmd.ExecuteReader();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Синхронное отключение от БД
                await conn.GetCloseBD();
                // Возращение false
                return false;
            }

            // Цикл while выполняется, пока есть строки для чтения из БД
            while (reader.Read())
            {


                ID_goods = Convert.ToInt32(reader["ID_goods"]);
                Name = reader["Name"].ToString();
                Price = Convert.ToSingle(reader["Price"]);
                //Image = reader["Image"].ToString();
                Description = reader["Description"].ToString();
                Price_with_discount = Convert.ToSingle(reader["Price_with_discount"]);



                // await Task.Delay(1000);
            }
            OnPropertyChanged("ID_goods");
            OnPropertyChanged("Name");
            OnPropertyChanged("Price");
            //OnPropertyChanged("Image");
            OnPropertyChanged("Description");
            OnPropertyChanged("Price_with_discount");
            await conn.GetCloseBD();
          
            return true;
        }
    }
}
