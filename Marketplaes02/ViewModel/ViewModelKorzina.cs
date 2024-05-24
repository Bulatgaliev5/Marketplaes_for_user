using Marketplaes02.BD;
using Marketplaes02.Model;
using Microsoft.Maui;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;


namespace Marketplaes02.ViewModel
{
    public class ViewModelKorzina :  INotifyPropertyChanged
    {

        public ViewModelKorzina()
        {
            VisibleCollectionViewEmptyView = false;
            VisibleNullList = true;
            Load();
            UpdatePlusCountCommand = new Command<int>(UpdatePlusCount);
          //  UpdateMinusCountCommand = new Command(UpdateMinusCount);

        }
        private Korzina _SelectKorzina;
        public Korzina SelectKorzina
        {
            get => _SelectKorzina;
            set
            {
                _SelectKorzina = value;
                OnPropertyChanged("SelectKorzina");

            }
        }
        public async void UpdatePlusCount(int id_goods)
        {

            SelectKorzina = Korzinalist.FirstOrDefault(g => g.ID_goods == id_goods);
            
            if (SelectKorzina != null)
            {
                if (SelectKorzina.V_nalichiioods > SelectKorzina.Count)
                {
                    SelectKorzina.Count++;

                    await UpdateCountKorzinaGood(id_goods, UserID);
                    await LoadKorzinaGoodPrice(id_goods, UserID);
                }

            }



            // Price = Price * Count;
            // Price_with_discount = Price_with_discount * Count;
        }


        public async void UpdateMinusCount(int id_goods)
        {
//Count--;

            await UpdateCountKorzinaGood(id_goods, UserID);
            await LoadKorzinaGoodPrice(id_goods, UserID);

            // Price = Price * Count;
            //Price_with_discount = Price_with_discount * Count;
        }
        public async Task<bool> LoadKorzinaGoodPrice(int id_good, int ID_user)
        {
            string
             sql = "SELECT k.Total_price,k.ID_goods, k.Total_Price_with_discount, u.ID AS User_ID " +
             "FROM korzina k " +
             "JOIN users u ON k.ID_user = u.ID " +
             "WHERE ID_user = @ID_user and k.ID_goods =@id_good";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id_good", id_good));

            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            await conn.GetConnectBD();
            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Синхронное отключение от БД
                await conn.GetCloseBD();
                // Возращение false
                return false;
            }
            while (await reader.ReadAsync())
            {


             //   Price = Convert.ToSingle(reader["Total_price"]);
               // Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]);
                // await Task.Delay(1000);
            }

            await conn.GetCloseBD();
            OnPropertyChanged("Price");
            OnPropertyChanged("Price_with_discount");
            return true;

        }

        public async Task<bool> UpdateCountKorzinaGood(int id_good, int ID_user)
        {
            string
             sql = "UPDATE korzina SET Count=@Count WHERE  ID_goods=@id and ID_user=@ID_user";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id", id_good));
            cmd.Parameters.Add(new MySqlParameter("@Count", SelectKorzina.Count));
            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            await conn.GetConnectBD();


            cmd.ExecuteNonQuery();

            await conn.GetCloseBD();
            OnPropertyChanged("Price");
            OnPropertyChanged("Price_with_discount");
            OnPropertyChanged("SelectKorzina");
            OnPropertyChanged("Count");
            OnPropertyChanged("Korzinalist");
            return true;

        }

        public ICommand UpdatePlusCountCommand { get; set; }
        public ICommand UpdateMinusCountCommand { get; set; }
        private bool _btnbuy;
        public bool btnbuy
        {
            get => _btnbuy;
            set
            {
                _btnbuy = value;
                OnPropertyChanged("btnbuy");

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

        int UserID;
        private IList<Korzina> _Korzinalist;
        public IList<Korzina> Korzinalist
        {
            get => _Korzinalist;
            set
            {
                _Korzinalist = value;
                OnPropertyChanged("Korzinalist");
            }
        }


        public void CleanListData(int id)
        {

            foreach (Korzina good in Korzinalist)
            {
                if (good.ID_goods != id)
                    continue;
                Korzinalist.Remove(good);
                Load();
                return;
            }



        }
        public async Task<bool> DeleteGoodSQL(int id_goods, int UserID)
        {
          string
             sql = "DELETE FROM korzina WHERE  ID_goods=@id AND ID_user=@UserID";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id", id_goods));
            cmd.Parameters.Add(new MySqlParameter("@UserID", UserID));
            await conn.GetConnectBD();
            if (await cmd.ExecuteNonQueryAsync() == 1)
            {
                await conn.GetCloseBD();
                return true;

            }
            await conn.GetCloseBD();
            return false;

        }

        public async void Load()
        {
            VisibleNullList = true;
            UserID = Preferences.Default.Get("UserID", 0);
            await LoadKorzinalist(UserID);
            if (Korzinalist != null)
            {
                btnbuy = true;
            }
            else
            {
                btnbuy = false;
                VisibleNullList = false;
                VisibleCollectionViewEmptyView = true;
            }
            VisibleNullList = false;



        }

        public async void UpdateCount()
        {

            SaveList("Sostavzakazalist", Korzinalist);
        }

        public async Task<bool> SQL_Delete_Korzina(int ID_user, int i)
        {
            ConnectBD con = new ConnectBD();
            string sql = "DELETE FROM `korzina` WHERE  `ID_user`=@ID_user AND ID_korzina=@ID_korzina";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            cmd.Parameters.Add(new MySqlParameter("@ID_korzina", Korzinalist[i].ID_korzina));
            await con.GetConnectBD();
            await cmd.ExecuteNonQueryAsync();
            await con.GetCloseBD();
            return true;
        }
        public async Task<bool> Delete_Korzina()
        {
            int i = 0;
            for (; i < Korzinalist.Count; i++)
            {

                await SQL_Delete_Korzina(UserID, i);


            }
            if (i == Korzinalist.Count)
            {
                Korzinalist.Clear();
                return true;
            }
            return false;
        }
        private  async Task<bool> LoadKorzinalist(int id)
        {

            string
                  sql = "SELECT k.ID_korzina,k.Total_price,k.Total_Price_with_discount, g.Name AS Goods_Name, g.ID_goods AS Goods_ID, " +
                  "g.ImageGood AS Goods_Image, u.ID AS User_ID, k.Count, g.V_nalichii AS Goods_V_nalichii " +
                  "FROM korzina k " +
                  "JOIN goods g ON k.ID_goods = g.ID_goods " +
                  "JOIN users u ON k.ID_user = u.ID " +
                  "WHERE ID_user=@ID_User";





            ConnectBD con = new ConnectBD();


            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_User", id));
            await con.GetConnectBD();
            MySqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {

                await con.GetCloseBD();

                return false;

            }

            Korzinalist = new ObservableCollection<Korzina>();

            while (await reader.ReadAsync())
            {

                Korzinalist.Add(new Korzina()
                {
                    ID_goods = Convert.ToInt32(reader["Goods_ID"]),
                    ID_korzina = Convert.ToInt32(reader["ID_korzina"]),
                    ID_user = Convert.ToInt32(reader["User_ID"]),
                    Count = Convert.ToInt32(reader["Count"]),
                    Name = reader["Goods_Name"].ToString(),
                    Image = reader["Goods_Image"].ToString(),
                    Price = Convert.ToSingle(reader["Total_price"]),
                    Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]),
                    V_nalichiioods = Convert.ToInt32(reader["Goods_V_nalichii"]),

                });

            }
            UpdateCount();
            OnPropertyChanged("Korzinalist");
            await con.GetCloseBD();
            // Preferences.Default.Set("Sostavzakazalist", Korzinalist.ToList<Korzina>);
            ///  UpdateCount();
            return true;


        }

        //Для загрузки списка в словарь лист в состав заказа
        public static void SaveList(string key, IList<Korzina> list)
        {
            var jsonString = JsonSerializer.Serialize(list);
            Preferences.Set(key, jsonString);
        }








        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
    }
}