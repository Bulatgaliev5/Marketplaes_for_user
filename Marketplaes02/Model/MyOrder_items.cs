﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02.Model
{
    public class MyOrder_items : INotifyPropertyChanged
    {
      
        private int _Total_Count;
       
        private float _Total_Price_with_discount;

        public float Total_Price_with_discount
        {
            get => _Total_Price_with_discount;
            set
            {
                _Total_Price_with_discount = value;
                OnPropertyChanged("Total_Price_with_discount");

            }
        }

        private float _Total_Order_Price_with_discount;

        public float Total_Order_Price_with_discount
        {
            get => _Total_Order_Price_with_discount;
            set
            {
                _Total_Order_Price_with_discount = value;
                OnPropertyChanged("Total_Order_Price_with_discount");

            }
        }

        public int Total_Count
        {
            get => _Total_Count;
            set
            {
                _Total_Count = value;
                OnPropertyChanged("Total_Count");

            }
        }
        private string _track_number;

        public string Track_number
        {
            get => _track_number;
            set
            {
                _track_number = value;
                OnPropertyChanged("Track_number");
            }
        }


        private string _status;
        public string Status
        {
            get => _status;
            set
            {
               
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        public MyOrder_items()
        {

        }
        private int _ID_Order_items;

        /// <summary>
        /// Поле Название изделия
        /// </summary>
        private string _Name;
        /// <summary>
        /// Поле Изображение изделия
        /// </summary>
        private ImageSource _Image;

        private int _ID_order;
        public int ID_order
        {
            get => _ID_order;
            set
            {
                _ID_order = value;
                OnPropertyChanged("ID_order");
            }
        }

        private DateTime _Order_date;
        private int _ID_user;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
        public DateTime Order_date
        {
            get => _Order_date;
            set
            {
                _Order_date = value;
                OnPropertyChanged("Order_date");
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
        public int ID_Order_items
        {
            get => _ID_Order_items;
            set
            {
                _ID_Order_items = value;
                OnPropertyChanged("ID_Order_items");
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
        /// <summary>
        /// Свойства Изображение изделия
        /// </summary>
        public ImageSource Image
        {
            get => _Image;
            set
            {
                _Image = value;
                OnPropertyChanged("Image");

            }
        }
    }
}
