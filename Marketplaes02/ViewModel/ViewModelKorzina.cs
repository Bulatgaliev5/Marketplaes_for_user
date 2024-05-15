using Marketplaes02.BD;
using Marketplaes02.Model;
using Microsoft.Maui;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;


namespace Marketplaes02.ViewModel
{
    public class ViewModelKorzina : Korzina, INotifyPropertyChanged
    {

        public ViewModelKorzina()
        {
            VisibleCollectionViewEmptyView = false;
            VisibleNullList = true; 
            Load();
           

        }
       
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
            cmd.ExecuteNonQuery();
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
        private async Task<bool> LoadKorzinalist(int id)
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
            MySqlDataReader reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                
                await con.GetCloseBD();

                return false;

            }

            Korzinalist = new ObservableCollection<Korzina>();

            while (reader.Read())
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
