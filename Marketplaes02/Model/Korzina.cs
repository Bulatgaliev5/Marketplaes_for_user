using Marketplaes02.BD;
using MySqlConnector;
using System.ComponentModel;
using System.Windows.Input;

namespace Marketplaes02.Model
{
    public class Korzina : INotifyPropertyChanged
    {
        int Kartochka_ID_goods, UserID;
        public Korzina()
        {
            Kartochka_ID_goods = Preferences.Default.Get("Kartochka_ID_goods", 0);
            UserID = Preferences.Default.Get("UserID", 0);
            //  Price = Price * Count;
            UpdatePlusCountCommand = new Command<int>(UpdatePlusCount);
            UpdateMinusCountCommand = new Command<int>(UpdateMinusCount);

        }
        private async void UpdatePlusCount(int id_goods)
        {

            Count++;
            await UpdateCountKorzinaGood(id_goods, UserID);
            await LoadKorzinaGoodPrice(id_goods, UserID);


            // Price = Price * Count;
            // Price_with_discount = Price_with_discount * Count;
        }
        private async void UpdateMinusCount(int id_goods)
        {
            Count--;

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
                 reader = cmd.ExecuteReader();

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


                Price = Convert.ToSingle(reader["Total_price"]);
                Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]);
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
            cmd.Parameters.Add(new MySqlParameter("@Count", Count));
            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
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

            await conn.GetCloseBD();
            OnPropertyChanged("Price");
            OnPropertyChanged("Price_with_discount");
            return true;

        }
        public ICommand UpdatePlusCountCommand { get; set; }
        public ICommand UpdateMinusCountCommand { get; set; }
        private int _ID_korzina;
        private int _ID_goods;
        private int _ID_user;
        private int _Count;
        private string _Name;
        private string _Image;
        private float _Price;
        private float _Price_with_discount;

        public float Price
        {
            get => _Price;
            set
            {
                _Price = value;
                OnPropertyChanged("Price");

            }
        }
        public float Price_with_discount
        {
            get => _Price_with_discount;
            set
            {
                _Price_with_discount = value;
                OnPropertyChanged("Price_with_discount");

            }
        }
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Image
        {
            get => _Image;
            set
            {
                _Image = value;
                OnPropertyChanged("Image");
            }
        }
        public int ID_korzina
        {
            get => _ID_korzina;
            set
            {
                _ID_korzina = value;
                OnPropertyChanged("ID_korzina");
            }
        }

        public int ID_goods
        {
            get => _ID_goods;
            set
            {
                _ID_goods = value;
                OnPropertyChanged("ID_goods");
            }
        }
        public int Count
        {
            get => _Count;
            set
            {
                if (value <= 1)
                    value = 1;

                _Count = value;
                OnPropertyChanged("Count");
            }

        }
        public int ID_user
        {
            get => _ID_user;
            set
            {
                _ID_user = value;
                OnPropertyChanged("ID_user");
            }
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
