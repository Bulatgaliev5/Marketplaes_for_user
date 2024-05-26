
using Marketplaes02.BD;
using Marketplaes02.Commands;
using Marketplaes02.Model;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Marketplaes02.ViewModel
{
    public class ViewModelKartochkaGood : KartochkaGood
    {
        int Kartochka_ID_goods, UserID;
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
        public ViewModelKartochkaGood()
        {
            Kartochka_ID_goods = Preferences.Default.Get("Kartochka_ID_goods", 0);
            UserID = Preferences.Default.Get("UserID", 0);
            Load();
            CheckAddKorzinaGood(Kartochka_ID_goods, UserID);
        }
        private int _Count;
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
            int ID_goods = (int)item;
            await CheckVnalichiiGood(ID_goods);
            bool result = await CheckAddKorzinaGood(ID_goods, UserID);

            if (V_nalichii > Count)
            {
                Count++;
                if (result)
                {
                    await AddKorzinaGood(ID_goods, UserID);
                }
                else
                {
                    await UpdateCountKorzinaGood(ID_goods, UserID, Count);
                }
            }



        }
        public async void UpdateMinusCount(object item)
        {
            int ID_goods = (int)item;
            bool result = await CheckAddKorzinaGood(ID_goods, UserID);


                Count--;
                if (!result)
                {
                    await AddKorzinaGood(ID_goods, UserID);
                }
                else
                {
                    await UpdateCountKorzinaGood(ID_goods, UserID, Count);
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
        public async void Load()
        {
            await GoodsSelectSQL(Kartochka_ID_goods);
            await ImagesGoodsSelectSQL(Kartochka_ID_goods);

        }
        public async Task<bool> CheckVnalichiiGood(int id_good)
        {
            string
             sql = "SELECT * FROM goods WHERE  ID_goods=@id ";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id", id_good));
            await conn.GetConnectBD();
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();
            if (!reader.HasRows)
            {
                // Синхронное отключение от БД
                await conn.GetCloseBD();
                // Возращение false
                return false;
            }
            while (await reader.ReadAsync())
            {
                V_nalichii = Convert.ToInt32(reader["V_nalichii"]);
            }

            await conn.GetCloseBD();

            return true;

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
            await cmd.ExecuteNonQueryAsync();
            await con.GetCloseBD();
            return true;

        }

        public async Task<bool> UpdateCountKorzinaGood(int id_good, int ID_user, int Count)
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
            MySqlDataReader
                 reader  = await cmd.ExecuteReaderAsync();
            if (!reader.HasRows)
            {
                await conn.GetCloseBD();
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
                 reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                
                if (reader.HasRows)
                {
                    Count = Convert.ToInt32(reader["Count"]);
                    await conn.GetCloseBD();
                }

                return false;
            }

            await conn.GetCloseBD();

            return true;

        }
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
                 reader = await cmd.ExecuteReaderAsync();

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
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();
            if (!reader.HasRows)
            {
                await conn.GetCloseBD();
                return false;
            }

            while (await reader.ReadAsync())
            {
                ID_goods = Convert.ToInt32(reader["ID_goods"]);
                Name = reader["Name"].ToString();
                Price = Convert.ToSingle(reader["Price"]);
                Description = reader["Description"].ToString();
                Price_with_discount = Convert.ToSingle(reader["Price_with_discount"]);

            }
            await conn.GetCloseBD();

            return true;
        }
    }
}
