
using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.BD;
using Marketplaes02.Class;
using Marketplaes02.Model;

using MySqlConnector;

using System.Data;
using System.Text.Json;



namespace Marketplaes02.ViewModel
{
    public class VewModelSostavZakaza : OformlenieZakaza
    {
        string Order_date;
        int ID_order;
        int ID_user;
        private IList<Korzina> _SostavZakazalist;
        public IList<Korzina> SostavZakazalist
        {
            get => _SostavZakazalist;
            set
            {
                _SostavZakazalist = value;
                OnPropertyChanged("SostavZakazalist");
            }
        }
        private bool _VisiblBtnZakazat;

        public bool VisiblBtnZakazat
        {
            get => _VisiblBtnZakazat;
            set
            {
                _VisiblBtnZakazat = value;
                OnPropertyChanged("VisiblBtnZakazat");
            }
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Korzinalist"></param>
        public VewModelSostavZakaza(IList<Korzina> Korzinalist)
        {
            SostavZakazalist = Korzinalist;
            VisiblBtnZakazat = true;
            Load();
            WeakReferenceMessenger.Default.Register<UpdateAdresDostavki>(this, (r, m) =>
            {
                User_Adres_Dostavki = m.SelectAdresDostavki;


            });
            WeakReferenceMessenger.Default.Register<UpdateSostavZakaza>(this, (r, m) =>
            {

                Load();

            });
            WeakReferenceMessenger.Default.Register<UpdateResultPay>(this, (r, m) =>
            {
                if (m.Result)
                {
                    VisiblBtnZakazat = false;
               
                }

            });

        }

        /// <summary>
        /// Метод для загрузки 
        /// </summary>
        public async void Load()
        {

            User_Name = Preferences.Default.Get("UserName", "NoName");
            User_Number_phone = Preferences.Default.Get("UserNumber_phone", "NoName");
            User_Adres_Dostavki = Preferences.Default.Get("UserAdres_Dostavki", "NoName");
            ID_user = Preferences.Default.Get("UserID", 0);
            ButtonAdres_Dostavki = CheckUserAdres_Dostavki();
            User_Data = GetUser_Data();
            Goods_Total_Price_with_discount = GetGoods_Total_Price_with_discount();
            Goods_Total_Count = GetGoods_Total_Count();

        }
        /// <summary>
        /// БД доавть адрес доставки
        /// </summary>
        /// <param name="labelPay"></param>
        /// <returns></returns>
        public async Task<bool> SQL_AddOrders(string labelPay)
        {
            ConnectBD con = new ConnectBD();

            Order_date =  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            
            string sql = "INSERT INTO `orders` (`ID_user`, `Order_date`, `Total_Count`, `Total_Price_with_discount`, Adres_Dostavki, labelPay) VALUES (@ID_user, @Order_date, @Total_Count, @Total_Price_with_discount, @Adres_Dostavki, @labelPay)";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            cmd.Parameters.Add(new MySqlParameter("@Adres_Dostavki", User_Adres_Dostavki));
            cmd.Parameters.Add(new MySqlParameter("@Order_date", Order_date));
            cmd.Parameters.Add(new MySqlParameter("@Total_Count", Goods_Total_Count));
            cmd.Parameters.Add(new MySqlParameter("@Total_Price_with_discount", Goods_Total_Price_with_discount));
            cmd.Parameters.Add(new MySqlParameter("@labelPay", labelPay));
            await con.GetConnectBD();
            await cmd.ExecuteNonQueryAsync();
            await con.GetCloseBD();
            return true;
        }
        /// <summary>
        /// Получить заказа
        /// </summary>
        /// <param name="Order_date"></param>
        /// <param name="ID_user"></param>
        /// <returns></returns>
        private async Task<int> Get_ID_order(string Order_date, int ID_user)
        {

            string
                sql = "SELECT * FROM orders WHERE Order_date=@Order_date AND ID_user=@ID_user";
            ConnectBD con = new ConnectBD();

            MySqlCommand
             cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@Order_date", Order_date));
            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));

            await con.GetConnectBD();

            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {
                await con.GetCloseBD();
            }
            while (await reader.ReadAsync())
            {
                ID_order = Convert.ToInt32(reader["ID_order"]);

            }
            await con.GetCloseBD();

            return ID_order;
        }

        /// <summary>
        /// Добавить  заказы в состав заакза
        /// </summary>
        /// <param name="ID_order"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public async Task<bool> SQL_AddOrder_items(int ID_order, int i)
        {
            ConnectBD con = new ConnectBD();
          
            string sql = "INSERT INTO `order_items` (`ID_order`, `ID_goods`, `Total_Price_with_discount`, `Total_Count`) VALUES (@ID_order, @ID_goods, @Total_Price_with_discount, @Total_Count)";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_order", ID_order));
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", SostavZakazalist[i].ID_goods));
            cmd.Parameters.Add(new MySqlParameter("@Total_Count", SostavZakazalist[i].Count));
            cmd.Parameters.Add(new MySqlParameter("@Total_Price_with_discount", SostavZakazalist[i].Price_with_discount));
            await con.GetConnectBD();
            await cmd.ExecuteNonQueryAsync();
            await con.GetCloseBD();
            return true;
        }
        /// <summary>
        /// Обновить товары в наличии
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public async Task<bool> SQL_Upbate_V_nalichii(int i)
        {
            ConnectBD con = new ConnectBD();

            string sql = "UpdateGoodsV_nalichii";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("@goodsID", SostavZakazalist[i].ID_goods));
            cmd.Parameters.Add(new MySqlParameter("@quantity", SostavZakazalist[i].Count));
          
            await con.GetConnectBD();
            await cmd.ExecuteNonQueryAsync();
            await con.GetCloseBD();
            return true;
        }
        /// <summary>
        /// Добавить заказы
        /// </summary>
        /// <param name="labelPay"></param>
        /// <returns></returns>
        public async Task<bool> AddZakazi(string labelPay)
        {
            await SQL_AddOrders(labelPay);
            await Get_ID_order(Order_date, ID_user);
            int i = 0;
            for (; i < SostavZakazalist.Count; i++)
            {
              
                await SQL_AddOrder_items(ID_order, i);

                await SQL_Upbate_V_nalichii(i);

            }
            if (i == SostavZakazalist.Count)
            {
               return true;
            }
            return false;
        }


        /// <summary>
        /// Проверить указан адрес доставки
        /// </summary>
        /// <returns></returns>
        public string CheckUserAdres_Dostavki()
        {
            if (User_Adres_Dostavki!= "")
            {
                return "Изменить адрес доставки";
            }
            return "Добавить адрес доставки";
        }

        public float GetGoods_Total_Price_with_discount()
        {
            return SostavZakazalist.Sum(item => item.Price_with_discount);
        }

        public int GetGoods_Total_Count()
        {
            return SostavZakazalist.Sum(item => item.Count);
        }


        public string GetUser_Data()
        {
            return User_Name + ", " + User_Number_phone;
        }


        //Получает список от корзины и записывает в новый список для формирования заказа
        public static List<SostavZakaza> GetList<SostavZakaza>(string key)
        {
            var jsonString = Preferences.Get(key, string.Empty);
            return JsonSerializer.Deserialize<List<SostavZakaza>>(jsonString);
        }


        
    }
}
