using Marketplaes02.BD;
using Marketplaes02.Model;
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
    public class VewModelSostavZakaza :  INotifyPropertyChanged
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
            ID_goods = Preferences.Default.Get("ID_goods", 0);
            await LoadSostavZakazalist(ID_goods);
        }
        private async Task<bool> LoadSostavZakazalist(int id)
        {

            string
                  sql = "SELECT * FROM goods WHERE  ID_goods=@ID_goods";




            ConnectBD con = new ConnectBD();


            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", id));
            await con.GetConnectBD();
            MySqlDataReader reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                await con.GetCloseBD();

                return false;

            }

            SostavZakazalist = new ObservableCollection<SostavZakaza>();

            while (reader.Read())
            {

                SostavZakazalist.Add(new SostavZakaza()
                {
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                  //  Count =  ,
                    Name = reader["Name"].ToString(),
                    // Image = reader["Image"].ToString(),
                    Price = Convert.ToSingle(reader["Goods_Price"]),
                    Price_with_discount = Convert.ToSingle(reader["Goods_Price_with_discount"]),


                }) ;

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
