using Marketplaes02.BD;
using Marketplaes02.Model;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02.ViewModel
{
    public class ViewModelKorzina : INotifyPropertyChanged
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
                  sql = "SELECT k.ID_korzina, g.Name AS Goods_Name, g.Price AS Goods_Price,g.Price_with_discount AS Goods_Price_with_discount, i.Image AS Goods_Image, u.ID AS User_ID, k.Count " +
                  "FROM korzina k " +
                  "JOIN goods g ON k.ID_goods = g.ID_goods " +
                  "JOIN users u ON k.ID_user = u.ID " +
                  "JOIN imagesgoods i ON g.ImageID = i.ImageID " +
                  "WHERE ID_user = @ID_User";




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
                    
                    ID_korzina = Convert.ToInt32(reader["ID_korzina"]),
                    ID_user = Convert.ToInt32(reader["User_ID"]),
                    Count = Convert.ToInt32(reader["Count"]),
                    NameGood = reader["Goods_Name"].ToString(),
                    ImageGood = reader["Goods_Image"].ToString(),
                    Price = Convert.ToSingle(reader["Goods_Price"]),
                    Price_with_discount = Convert.ToSingle(reader["Goods_Price_with_discount"]),


                });

            }

            OnPropertyChanged("Korzinalist");
            await con.GetCloseBD();
            return true;
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
