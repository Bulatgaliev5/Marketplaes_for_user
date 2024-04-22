
using Marketplaes02.BD;
using Marketplaes02.Model;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Marketplaes02.ViewModel
{
    public class ViewModelKartochkaGood : KartochkaGood, INotifyPropertyChanged
    {
        int Kartochka_ID_goods, UserID;
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
            Kartochka_ID_goods = Preferences.Default.Get("Kartochka_ID_goods", 0);
            UserID = Preferences.Default.Get("UserID", 0);
            Load();
            CheckAddKorzinaGood(Kartochka_ID_goods, UserID);
            UpdatePlusCountCommand = new Command<int>(UpdatePlusCount);
            UpdateMinusCountCommand = new Command<int>(UpdateMinusCount);
        }
        private int _Count;
        public ICommand UpdatePlusCountCommand { get; set; }
        public ICommand UpdateMinusCountCommand { get; set; }
        public async void UpdatePlusCount(int id_goods)
        {
            bool result = await CheckAddKorzinaGood(id_goods, UserID);
            if (result)
            {
                Count++;
                await AddKorzinaGood(id_goods, UserID);
            }
            else
            {
                Count++;
                await UpdateCountKorzinaGood(id_goods, UserID);
            }


        }
        public async void UpdateMinusCount(int id_goods)
        {
            Count--;
            await UpdateCountKorzinaGood(id_goods, UserID);
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


        /// <summary>
        /// Метод загрузки изделтй Load
        /// </summary>
        public async void Load()
        {
            await GoodsSelectSQL(Kartochka_ID_goods);
            await ImagesGoodsSelectSQL(Kartochka_ID_goods);

        }


        public async Task<bool> AddKorzinaGood(int id_good, int UserID)
        {
            ConnectBD con = new ConnectBD();

            string sql = "INSERT INTO korzina (ID_goods, ID_user, Count) VALUES (@ID_goods, @ID_user, @Count)";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", id_good));
            cmd.Parameters.Add(new MySqlParameter("@ID_user", UserID));
            cmd.Parameters.Add(new MySqlParameter("@Count", Count));
            await con.GetConnectBD();
            cmd.ExecuteNonQuery();
            await con.GetCloseBD();
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

            return true;

        }
        public async Task<bool> CheckAddKorzinaGood(int id_good, int UserID)
        {
            string
             sql = "SELECT * FROM korzina WHERE  ID_goods=@id and ID_user=@UserID ";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id", id_good));
            cmd.Parameters.Add(new MySqlParameter("@UserID", UserID));
            await conn.GetConnectBD();
            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = cmd.ExecuteReader();

            // Проверка, что строк нет

            int count = 0;
            while (await reader.ReadAsync())
            {
                count++;
                Count = Convert.ToInt32(reader["Count"]);

            }
            OnPropertyChanged("Count");
            if (count != 0)
            {

                // Синхронное отключение от БД
                await conn.GetCloseBD();
                // Возращение false
                return false;
            }


            await conn.GetCloseBD();

            return true;

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
            while ((await reader.ReadAsync()))
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
