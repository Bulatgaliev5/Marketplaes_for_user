using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.BD;
using Marketplaes02.Class;
using Marketplaes02.Commands;
using Marketplaes02.Model;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using Marketplaes02.View;




namespace Marketplaes02.ViewModel
{
    public class ViewModelKorzina : INotifyPropertyChanged
    {

        public ViewModelKorzina()
        {
            VisibleCollectionViewEmptyView = false;
            VisibleNullList = true;
            Load();
            WeakReferenceMessenger.Default.Register<UpdateSostavZakaza>(this, (r, m) =>
            {

                Load();

            });

        }
        public async void LoadUpdate(object item)
        {
            IList<Korzina> korzian =(IList<Korzina>)item;
             await Application.Current.MainPage.Navigation.PushAsync(new ViewOformlenieZakaza(korzian));

        }
        private ICommand _UpdateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                if (_UpdateCommand == null)
                {
                    _UpdateCommand = new ActionCommand(LoadUpdate);
                }
                return _UpdateCommand;
            }
        }

        private ICommand _UpdatePlusCountCommand;
        public ICommand UpdatePlusCountCommand
        {
            get
            {
                if (_UpdatePlusCountCommand == null)
                {
                    _UpdatePlusCountCommand = new ActionCommand(UpdatePlusCount);
                }
                return _UpdatePlusCountCommand;
            }
        }

        private ICommand _UpdateMinusCountCommand;
        public ICommand UpdateMinusCountCommand
        {
            get
            {
                if (_UpdateMinusCountCommand == null)
                {
                    _UpdateMinusCountCommand = new ActionCommand(UpdateMinusCount);
                }
                return _UpdateMinusCountCommand;
            }

        }

        public async void UpdatePlusCount(object item)
        {
            Korzina
                 korzina = (Korzina)item;
            await LoadKorzinaGoodPrice(korzina.ID_goods, UserID, korzina);
            if (korzina.V_nalichiioods > korzina.Count)
            {
                korzina.Count++;
                korzina.Price = korzina.Price * korzina.Count;
                korzina.Price_with_discount = korzina.Price_with_discount * korzina.Count;
                await UpdateCountKorzinaGood(korzina.ID_goods, UserID, korzina.Count);

            }
      
        }


        public async void UpdateMinusCount(object item)
        {
            Korzina
                   korzina = (Korzina)item;

                korzina.Count--;

                await UpdateCountKorzinaGood(korzina.ID_goods, UserID, korzina.Count);
            await LoadKorzinaGoodPrice(korzina.ID_goods, UserID, korzina);
    
        }
        public async Task<bool> LoadKorzinaGoodPrice(int id_good, int ID_user, Korzina korzina)
        {
            string
             sql = "SELECT k.Total_price, k.ID_goods, k.Total_Price_with_discount, u.ID AS User_ID, g.V_nalichii AS Goods_V_nalichii " +
             "FROM korzina k " +
             "JOIN users u ON k.ID_user = u.ID " +
             "JOIN goods g ON k.ID_goods = g.ID_goods " +
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

                korzina.Price = Convert.ToSingle(reader["Total_price"]);
                korzina.Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]);
                korzina.V_nalichiioods = Convert.ToInt32(reader["Goods_V_nalichii"]);
            }

            await conn.GetCloseBD();
            OnPropertyChanged("Price");
            OnPropertyChanged("Price_with_discount");
            return true;

        }

        public async Task<bool> UpdateCountKorzinaGood(int id_good, int ID_user, int count)
        {
            string
             sql = "UPDATE korzina SET Count=@Count WHERE  ID_goods=@id and ID_user=@ID_user";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id", id_good));
            cmd.Parameters.Add(new MySqlParameter("@Count", count));
            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            await conn.GetConnectBD();
            cmd.ExecuteNonQuery();
            await conn.GetCloseBD();
            return true;

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
        private string _LabelPay;
        public string LabelPay
        {
            get => _LabelPay;
            set
            {
                _LabelPay = value;
                OnPropertyChanged("LabelPay");

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


        /// <summary>
        /// Удалить тоапр из корзины
        /// </summary>
        /// <param name="ID_user"></param>
        /// <param name="i"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод для загрузки корзины пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                    Image = await new Class.FileBase().LoadImageAsync(reader["Goods_Image"].ToString()),
                    Price = Convert.ToSingle(reader["Total_price"]),
                    Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]),
                    V_nalichiioods = Convert.ToInt32(reader["Goods_V_nalichii"]),

                });


            }
            for (int i = 0; i < Korzinalist.Count; i++)
            {
                LabelPay += Convert.ToString(Korzinalist[i].ID_korzina)+",";
            }
           
            OnPropertyChanged("Korzinalist");
            await con.GetCloseBD();

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