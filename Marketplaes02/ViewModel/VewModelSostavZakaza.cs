using Marketplaes02.BD;
using Marketplaes02.Model;
using MySqlConnector;
using System.ComponentModel;
using System.Text.Json;
using System.Xml.Linq;


namespace Marketplaes02.ViewModel
{
    public class VewModelSostavZakaza : OformlenieZakaza, INotifyPropertyChanged
    {

        int ID_goods;
        private IList<SostavZakaza> _SostavZakazalist;
        public IList<SostavZakaza> SostavZakazalist
        {
            get => _SostavZakazalist;
            set
            {
                _SostavZakazalist = value;
                OnPropertyChanged("SostavZakazalist");
            }
        }
        public VewModelSostavZakaza()
        {

            Load();

        }


        public async void Load()
        {

         
            SostavZakazalist = GetList<SostavZakaza>("Sostavzakazalist");
            User_Name = Preferences.Default.Get("UserName", "NoName");
            User_Number_phone = Preferences.Default.Get("UserNumber_phone", "NoName");
            User_Adres_Dostavki = Preferences.Default.Get("UserAdres_Dostavki", "NoName");
            ButtonAdres_Dostavki = CheckUserAdres_Dostavki();
            User_Data = GetUser_Data();
            Goods_Total_Price_with_discount = GetGoods_Total_Price_with_discount();
            Goods_Total_Count = GetGoods_Total_Count();
            //  ID_goods = Preferences.Default.Get("ID_goods", 0);
            //  await LoadSostavZakazalist(ID_goods);
        }

        public async Task<bool> SQL_AddZakazi(int i)
        {
            ConnectBD con = new ConnectBD();
            
            DateTime data_zakaza = DateTime.Now;
            int UserID = Preferences.Default.Get("UserID", 0);

            // int id_klienta = (Int32)Application.Current.Properties["UserID"];
            var Status = "Заказ обрабатывается";
            string sql = "INSERT INTO zakaz (`ID_user`, `ID_goods`, `Datetime_zakaza`, `Status`, `Total_Count`, `Total_Price_with_discount`) VALUES (@ID_user, @ID_goods, @Datetime_zakaza, @Status, @Total_Count, @Total_Price_with_discount)";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_user", UserID));
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", SostavZakazalist[i].ID_goods));
            cmd.Parameters.Add(new MySqlParameter("@Datetime_zakaza", data_zakaza));
            cmd.Parameters.Add(new MySqlParameter("@Status", Status));
            cmd.Parameters.Add(new MySqlParameter("@Total_Count", Goods_Total_Count));
            cmd.Parameters.Add(new MySqlParameter("@Total_Price_with_discount", Goods_Total_Price_with_discount));
            await con.GetConnectBD();
            cmd.ExecuteNonQuery();
            await con.GetCloseBD();
            return true;
        }

        public async Task<bool> AddZakazi()
        {
            int i = 0;
            for (; i < SostavZakazalist.Count; i++)
            {
              
                await SQL_AddZakazi(i);
          

            }
            if (i == SostavZakazalist.Count)
            {
               return true;
            }
            return false;
        }



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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
    }
}
