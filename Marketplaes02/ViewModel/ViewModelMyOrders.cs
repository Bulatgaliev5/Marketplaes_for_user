using CommunityToolkit.Maui.Core.Primitives;
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
    public class ViewModelMyOrders: INotifyPropertyChanged
    {
       
        public ViewModelMyOrders()
        {
            Load();


        }
        public async void Load()
        {
            VisibleCollectionViewEmptyView = false;
            VisibleNullList = true;
            UserID = Preferences.Default.Get("UserID", 0);
            await LoadMyOrders(UserID);
            await LoadMyOrder_items(UserID);
            if (MyOrderslist != null && MyOrder_itemslist != null)
            {
                All_MyOrderList = (from order in MyOrderslist
                                   join orderItem in MyOrder_itemslist on order.ID_order equals orderItem.ID_order into orderGroup
                                   select new All_MyOrder { Order = order, Items = orderGroup.ToList() })
                   .OrderByDescending(o => o.Order.ID_order)
                   .ToList();
                VisibleCollectionViewEmptyView = true;
            }
            else
            {
                
            }
            VisibleNullList = false;


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
        private IList<All_MyOrder> _All_MyOrderList;
        public IList<All_MyOrder> All_MyOrderList
        {
            get => _All_MyOrderList;
            set
            {
                _All_MyOrderList = value;
                OnPropertyChanged("All_MyOrderList");
            }
        }
        int UserID;
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

        private IList<MyOrders> _MyOrderslist;
        public IList<MyOrders> MyOrderslist
        {
            get => _MyOrderslist;
            set
            {
                _MyOrderslist = value;
                OnPropertyChanged("MyOrderslist");
            }
        }
        private async Task<bool> LoadMyOrders(int id)
        {

            string
                  sql = "SELECT o.ID_order, o.Order_date, o.Total_Count,o.Total_Price_with_discount, u.ID AS User_ID, o.Status, o.Track_number " +
                  "FROM orders o " +
                  "JOIN users u ON o.ID_user = u.ID " +
                  "WHERE ID_user = @ID_User";





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

            MyOrderslist = new ObservableCollection<MyOrders>();

            while (await reader.ReadAsync())
            {

                MyOrderslist.Add(new MyOrders()
                {
                    ID_order = Convert.ToInt32(reader["ID_order"]),
                     Total_Count = Convert.ToInt32(reader["Total_Count"]),
                    ID_user = Convert.ToInt32(reader["User_ID"]),
                    Order_date = Convert.ToDateTime(reader["Order_date"]),
                    Total_Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]),
                    Status = Convert.ToString(reader["Status"]),
                    Track_number = Convert.ToString(reader["Track_number"]),
                });
               
            }
            
            OnPropertyChanged("MyOrderslist");
            await con.GetCloseBD();

            return true;


        }

        private async Task<bool> LoadMyOrder_items(int id)
        {

            string
                  sql = "SELECT u_i.ID_order_item AS ID_order_item, o.Status AS Status_order, o.ID_order, o.Order_date, u_i.Total_Count,u_i.Total_Price_with_discount, " +
                  "g.Name AS Goods_Name, g.ID_goods AS Goods_ID, g.ImageGood AS Goods_Image, u.ID AS User_ID, o.Adres_Dostavki, " +
                  "u.Name AS User_Name, u.Number_phone AS User_Number_phone  " +
                  "FROM orders o " +
                  "JOIN order_items u_i ON u_i.ID_order = o.ID_order " +
                  "JOIN goods g ON u_i.ID_goods = g.ID_goods " +
                  "JOIN users u ON o.ID_user = u.ID " +
                  "WHERE ID_user = @ID_User " +
                  "ORDER BY u_i.ID_order_item DESC";

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

            MyOrder_itemslist = new ObservableCollection<MyOrder_items>();

            while (await reader.ReadAsync())
            {

                MyOrder_itemslist.Add(new MyOrder_items()
                {
                    ID_Order_items = Convert.ToInt32(reader["ID_order_item"]),
                    Image = Convert.ToString(reader["Goods_Image"]),
                    Name = Convert.ToString(reader["Goods_Name"]),
                    Total_Count = Convert.ToInt32(reader["Total_Count"]),
                    ID_user = Convert.ToInt32(reader["User_ID"]),
                    Order_date = Convert.ToDateTime(reader["Order_date"]),
                    Total_Price_with_discount = Convert.ToSingle(reader["Total_Price_with_discount"]),
                    Status = Convert.ToString(reader["Status_order"]),
                    ID_order = Convert.ToInt32(reader["ID_order"]),

                });
               
            }

            OnPropertyChanged("MyOrder_itemslist");
         
            await con.GetCloseBD();

            return true;


        }
        private void Get_Count_goods()
        {
           
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
