using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02.Model
{
    public class Korzina : INotifyPropertyChanged
    {
        private int _ID_korzina;
        private int _ID_goods;
        private int _ID_user;
        private int _Count;
        private string _NameGood;
        private string _ImageGood;
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
        public string NameGood
        {
            get => _NameGood;
            set
            {
                _NameGood = value;
                OnPropertyChanged("NameGood");
            }
        }
        public string ImageGood
        {
            get => _ImageGood;
            set
            {
                _ImageGood = value;
                OnPropertyChanged("ImageGood");
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
