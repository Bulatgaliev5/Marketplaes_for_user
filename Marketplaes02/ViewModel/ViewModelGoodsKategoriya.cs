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
using static Android.Util.EventLogTags;

namespace Marketplaes02.ViewModel
{
    public class ViewModelGoodsKategoriya : INotifyPropertyChanged
    {
        int id_kategoriya;
        /// <summary>
        /// Список Good
        /// </summary>
        private IList<GoodsKategoriya> _GoodsKategoriyalist;
        public IList<GoodsKategoriya> GoodsKategoriyalist
        {
            get => _GoodsKategoriyalist;
            set
            {
                _GoodsKategoriyalist = value;
                OnPropertyChanged("GoodsKategoriyalist");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));


        }
        public ViewModelGoodsKategoriya()
        {
            Load();
        }


        /// <summary>
        /// Метод загрузки изделтй Load
        /// </summary>
        public async void Load()
        {
            id_kategoriya = Preferences.Default.Get("id_kategoriya", 0);
            await LoadGoods(id_kategoriya);
        }
        /// <summary>
        /// Метод Получения изделий из БД
        /// </summary>
        /// <returns></returns>
        private async Task<bool> LoadGoods(int id_kategoriya)
        {
            // Строка запроса
            string
                sql = "SELECT * FROM goods WHERE id_kategoriya=@id_kategoriya";

            // Объявление переменной на основе класс подключения:
            // >    Connector conn
            // Инициализация переменной:
            // >    = new Connector()


            ConnectBD con = new ConnectBD();

            // Объявление объекта команды:
            // >    MySqlCommand cmd
            // Инициализация объекта команды:
            // >    new MySqlCommand(sql, conn.GetConn());
            MySqlCommand
             cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id_kategoriya", id_kategoriya));

            // Синхронное подключение к БД
            await con.GetConnectBD();

            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = cmd.ExecuteReader();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Список товаров опусташается
                GoodsKategoriyalist.Clear();
                // Синхронное отключение от БД
                await con.GetCloseBD();
                // Возращение false
                return false;
            }
            GoodsKategoriyalist = new ObservableCollection<GoodsKategoriya>();
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {

                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)
                GoodsKategoriyalist.Add(new GoodsKategoriya()
                {
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                    Name = reader["Name"].ToString(),
                    Price = Convert.ToSingle(reader["Price"]),
                    Image = reader["ImageGood"].ToString(),
                    Price_with_discount = Convert.ToSingle(reader["Price_with_discount"]),
                    Discount = Convert.ToInt32(reader["Discount"]),
                    Description = reader["Description"].ToString(),
                });

                // await Task.Delay(1000);
            }
            OnPropertyChanged("GoodsKategoriyalist");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
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
            if (await cmd.ExecuteNonQueryAsync() == 1)
            {
                await conn.GetCloseBD();
                return true;

            }
            await conn.GetCloseBD();
            // App.Current.Properties["ID_goods"] = id;
            return false;
        }
    }
}
