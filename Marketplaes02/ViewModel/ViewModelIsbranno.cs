using Marketplaes02.BD;
using Marketplaes02.Model;
using Microsoft.Maui;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Marketplaes02.ViewModel
{
    /// <summary>
    /// Класс для работы с избранными товарами
    /// </summary>
    public class ViewModelIsbrannoe : INotifyPropertyChanged
    {
        int ID_user;

        private IList<Isbrannoe> _Isbrannoelist;
        public IList<Isbrannoe> Isbrannoelist
        {
            get => _Isbrannoelist;
            set
            {
                _Isbrannoelist = value;
                OnPropertyChanged("Isbrannoelist");
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));


        }
        public ViewModelIsbrannoe()
        {
            VisibleCollectionViewEmptyView = false;
            VisibleNullList = true;
            Load();
        }

        public async void Load()
        {
            
            ID_user = Preferences.Default.Get("UserID", 0);
            await LoadGoods(ID_user);
            if (Isbrannoelist != null)
            {
                await ImageIsbrannoeLoad();
                
            }
            else
            {
               VisibleCollectionViewEmptyView = true;
            }
            VisibleNullList = false;
        }
        public async Task<bool> ImageIsbrannoeLoad()
        {

           
            int i = 0;
            for (; i < Isbrannoelist.Count; i++)
            {

              bool res = await SQLImageIsbrannoeLoad(ID_user, Isbrannoelist[i].ID_goods);
                if (res)
                {
                    Isbrannoelist[i].ImageIsbrannoe = "isbrannoe_true.png";
                }
                else
                {

                    Isbrannoelist[i].ImageIsbrannoe = "isbrannoe.png";
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
            OnPropertyChanged("Isbrannoelist");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return true;
        }
        public async Task<bool> AddSQLImageIsbrannoe(int ID_user, int ID_goods)
        {

            string
                sql = "INSERT INTO isbrannoe_goods (ID_goods, ID_user) VALUES (@ID_goods, @ID_user)";


            ConnectBD con = new ConnectBD();
            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());

            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", ID_goods));
            await con.GetConnectBD();
            await  cmd.ExecuteNonQueryAsync();
            OnPropertyChanged("Isbrannoelist");
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

           
            OnPropertyChanged("Isbrannoelist");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return false;
        }

        private async Task<bool> LoadGoods(int ID_user)
        {
            // Строка запроса
            string
                sql = "SELECT i.ID_goods, i.ID_user, g.Name, g.Price, g.ImageGood, g.Price_with_discount, g.Discount " +
                "FROM isbrannoe_goods i " +
                "JOIN goods g ON i.ID_goods = g.ID_goods " +
                "JOIN users u ON i.ID_user = u.ID " +
                "WHERE i.ID_user=@ID_user " +
                "ORDER BY i.ID_goods DESC";



            ConnectBD con = new ConnectBD();


            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            // Синхронное подключение к БД
            await con.GetConnectBD();

            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                
                // Синхронное отключение от БД
                await con.GetCloseBD();
                // Возращение false
                return false;
            }
            Isbrannoelist = new ObservableCollection<Isbrannoe>();
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {

                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)
                Isbrannoelist.Add(new Isbrannoe()
                {
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                    Name = reader["Name"].ToString(),
                    Price = Convert.ToSingle(reader["Price"]),
                    Image = await new Class.FileBase().LoadImageAsync(reader["ImageGood"].ToString()),
                    Price_with_discount = Convert.ToSingle(reader["Price_with_discount"]),
                    Discount = Convert.ToInt32(reader["Discount"]),
                });

            }
            OnPropertyChanged("Isbrannoelist");

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

            return false;
        }





    }
}
