using Marketplaes02.BD;
using Marketplaes02.Model;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;
using Marketplaes02.Class;
using Microsoft.Maui.Storage;

namespace Marketplaes02.ViewModel
{
    public class ViewModel_Goods : INotifyPropertyChanged
    {
        int ID_user;
        Class.FileBase fileBase = new Class.FileBase();
        /// <summary>
        /// Список Good
        /// </summary>
        private IList<Goods> _Goods;
        public IList<Goods> Goods
        {
            get => _Goods;
            set
            {
                _Goods = value;
                OnPropertyChanged("Goods");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));


        }
        private ICommand refreshCommand;
        public ICommand RefreshCommand { get; }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        public ViewModel_Goods()
        {
            

            RefreshCommand = new Command(async () => await RefreshItemsAsync());
            Load();
          
        }
        private async Task RefreshItemsAsync()
        {
           
            IsRefreshing = true;


            await Load();


            IsRefreshing = false;
        }
        private bool _VisibleNullList;

        public bool VisibleNullList
        {
            get => _VisibleNullList;
            set
            {
                _VisibleNullList = value;
                OnPropertyChanged("VisibleNullList");
            }
        }

        private bool _VisibleCollectionViewEmptyView;

        public bool VisibleCollectionViewEmptyView
        {
            get => _VisibleCollectionViewEmptyView;
            set
            {
                _VisibleCollectionViewEmptyView = value;
                OnPropertyChanged("VisibleCollectionViewEmptyView");
            }
        }
      
        /// <summary>
        /// Метод загрузки изделтй Load
        /// </summary>
        public async Task Load()
        {
            VisibleCollectionViewEmptyView = false;
            VisibleNullList = true;
            ID_user = Preferences.Default.Get("UserID", 0);
            bool  res =  await LoadGoods();
            await ImageIsbrannoeLoad();

                if (Goods != null)
                {

                    VisibleCollectionViewEmptyView = true;
                }
                else
                {
                    

                }
                VisibleNullList = false;
        }
        public async Task<bool> ImageIsbrannoeLoad()
        {

           
            int i = 0;
            for (; i < Goods.Count; i++)
            {

              bool res = await SQLImageIsbrannoeLoad(ID_user, Goods[i].ID_goods);
                if (res)
                {
                    Goods[i].ImageIsbrannoe = "isbrannoe_true.png";
                }
                else
                {

                   Goods[i].ImageIsbrannoe = "isbrannoe.png";
                }

            }
          
            return true;
        }
        private async Task<bool> SQLImageIsbrannoeLoad(int ID_user, int ID_goods)
        {
            // Строка запроса
            string
                sql = "SELECT * FROM isbrannoe_goods where ID_user=@ID_user AND ID_goods=@ID_goods";


            ConnectBD con = new ConnectBD();
            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());

            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
         cmd.Parameters.Add(new MySqlParameter("@ID_goods", ID_goods));
            await con.GetConnectBD();

            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                
                await con.GetCloseBD();
               
                return false;
            }
         
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {


            }
            OnPropertyChanged("Goods");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return true;
        }
        public async Task<bool> AddSQLImageIsbrannoe(int ID_user, int ID_goods)
        {
            // Строка запроса
            //INSERT INTO `dp_bulat_base`.`isbrannoe_goods` (`ID_goods`, `ID_user`) VALUES (57, 27);
            string
                sql = "INSERT INTO isbrannoe_goods (ID_goods, ID_user) VALUES (@ID_goods, @ID_user)";


            ConnectBD con = new ConnectBD();
            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());

            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", ID_goods));
            await con.GetConnectBD();
            await cmd.ExecuteNonQueryAsync();
            OnPropertyChanged("Goods");
            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return true;
        }
        public async Task<bool> SQLImageIsbrannoeDelete(int ID_user, int ID_goods)
        {
            // Строка запроса
            string
                sql = "DELETE FROM isbrannoe_goods where ID_user=@ID_user AND ID_goods=@ID_goods";


            ConnectBD con = new ConnectBD();
            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());

            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", ID_goods));
            await con.GetConnectBD();

            if (await cmd.ExecuteNonQueryAsync() == 1)
            {
                await con.GetCloseBD();
                return true;

            }

           
            OnPropertyChanged("Goods");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return false;
        }
        /// <summary>
        /// Метод Получения изделий из БД
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoadGoods()
        {

            string
                sql = "SelectGoods";

            ConnectBD con = new ConnectBD();

            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.CommandType = CommandType.StoredProcedure;
           // cmd.Parameters.Add(new MySqlParameter("userID", ID_user));
            // Синхронное подключение к БД
            await con.GetConnectBD();

            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Список товаров опусташается
                Goods.Clear();
                // Синхронное отключение от БД
                await con.GetCloseBD();
                // Возращение false
                return false;
            }
            Goods = new ObservableCollection<Goods>();
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {

                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)
                Goods.Add(new Goods()
                {
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                    Name = reader["Name"].ToString(),
                    Price = Convert.ToSingle(reader["Price"]),
                    Image = await  fileBase.LoadImageFromFtpAsync(reader["ImageGood"].ToString()),
                    Price_with_discount = Convert.ToSingle(reader["Price_with_discount"]),
                    Discount = Convert.ToInt32(reader["Discount"]),
                });
            }
            OnPropertyChanged("Goods");

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
