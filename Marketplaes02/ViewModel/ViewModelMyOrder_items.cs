using Marketplaes02.BD;
using Marketplaes02.Model;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
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
    public class ViewModelMyOrder_items : MyOrders, INotifyPropertyChanged
    {
        
        public ViewModelMyOrder_items()
        {

            Load();
            
        }

        public async void Load()
        {
          ID_order = Preferences.Default.Get("ID_order", 0);
            UserID = Preferences.Default.Get("UserID", 0);
            Track_number = Preferences.Default.Get("Track_number", "Null");
            Status = Preferences.Default.Get("Status", "Null");

            await LoadMyOrders(UserID, ID_order);   
          
        }
        int UserID;
        //float Total_Order_Price_with_discount;
        private IList<MyOrder_items> _MyOrder_itemslist;
        public IList<MyOrder_items> MyOrder_itemslist
        {
            get => _MyOrder_itemslist;
            set
            {
                _MyOrder_itemslist = value;
                OnPropertyChanged("MyOrder_itemslist");
            }
        }
        private async Task<bool> LoadMyOrders(int id, int ID_order)
        {

            string
                  sql = "SELECT u_i.ID_order_item AS ID_order_item, o.Track_number, o.Status, o.ID_order, o.Order_date, u_i.Total_Count,u_i.Total_Price_with_discount, " +
                  "g.Name AS Goods_Name, g.ID_goods AS Goods_ID, g.ImageGood AS Goods_Image, u.ID AS User_ID, o.Adres_Dostavki, " +
                  "u.Name AS User_Name, u.Number_phone AS User_Number_phone, o.Total_Price_with_discount AS Total_Order_Price_with_discount  " +
                  "FROM orders o " +
                  "JOIN order_items u_i ON u_i.ID_order = o.ID_order " +
                  "JOIN goods g ON u_i.ID_goods = g.ID_goods " +
                  "JOIN users u ON o.ID_user = u.ID " +
                  "WHERE ID_user = @ID_User AND o.ID_order =@ID_order " +
                  "ORDER BY u_i.ID_order_item DESC";

            ConnectBD con = new ConnectBD();


            MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@ID_User", id));
            cmd.Parameters.Add(new MySqlParameter("@ID_order", ID_order));
            await con.GetConnectBD();
            MySqlDataReader reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {

                await con.GetCloseBD();

                return false;

            }

            MyOrder_itemslist = new ObservableCollection<MyOrder_items>();

            while (reader.Read())
            {

                MyOrder_itemslist.Add(new MyOrder_items()
                {
                    ID_Order_items = Convert.ToInt32(reader["ID_order_item"]),
                    Image = Convert.ToString(reader["Goods_Image"]),
                    Name = Convert.ToString(reader["Goods_Name"]),
                    Total_Count = Convert.ToInt32(reader["Total_Count"]),
                    ID_user = Convert.ToInt32(reader["User_ID"]),
                    Total_Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]),
                   // Status = Convert.ToString(reader["Status"]),
                   // Track_number = Convert.ToString(reader["Track_number"]),

                });
                 Adres_Dostavki = Convert.ToString(reader["Adres_Dostavki"]);
                 User_Name = Convert.ToString(reader["User_Name"]);
                User_Number_phone = Convert.ToString(reader["User_Number_phone"]);
                Total_Order_Price_with_discount = Convert.ToSingle(reader["Total_Order_Price_with_discount"]);
                Order_date = Convert.ToDateTime(reader["Order_date"]);


            }
            OnPropertyChanged("Order_date");
            OnPropertyChanged("Adres_Dostavki");
            OnPropertyChanged("MyOrder_itemslist");
            OnPropertyChanged("User_Name");
            OnPropertyChanged("User_Number_phone");
            OnPropertyChanged("Track_number");
            OnPropertyChanged("Total_Order_Price_with_discount");
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
