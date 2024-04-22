using Marketplaes02.BD;
using Marketplaes02.Model;
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

            Load();

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





        public async void Load()
        {
            UserID = Preferences.Default.Get("UserID", 0);
            await LoadKorzinalist(UserID);
        }



        private async Task<bool> LoadKorzinalist(int id)
        {

            string
                  sql = "SELECT k.ID_korzina,k.Total_price,k.Total_Price_with_discount, g.Name AS Goods_Name, g.ID_goods AS Goods_ID, g.ImageGood AS Goods_Image, u.ID AS User_ID, k.Count " +
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


                });

            }

            OnPropertyChanged("Korzinalist");
            await con.GetCloseBD();
            //Preferences.Default.Set("Sostavzakazalist", Korzinalist);
            SaveList<Korzina>("Sostavzakazalist", Korzinalist);
            return true;


        }

        public static void SaveList<SostavZakaza>(string key, IList<SostavZakaza> list)
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
