using Marketplaes02.BD;
using MySqlConnector;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Marketplaes02.Model
{
    public class Korzina : INotifyPropertyChanged
    {
        public int _V_nalichiioods;
        public int V_nalichiioods
        {
            get => _V_nalichiioods;
            set
            {
                _V_nalichiioods = value;
                OnPropertyChanged("V_nalichiioods");
            }
        }
        int Kartochka_ID_goods, UserID;


        public Korzina()
        {
            Kartochka_ID_goods = Preferences.Default.Get("Kartochka_ID_goods", 0);
            UserID = Preferences.Default.Get("UserID", 0);
            //  Price = Price * Count;

        }

        private bool _btnbuy;
        public bool btnbuy
        {
            get => _btnbuy;
            set
            {
                _btnbuy = value;
                OnPropertyChanged("btnbuy");

            }
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



        private int _ID_korzina;
        private int _ID_goods;
        private int _ID_user;
        private int _Count;
        private string _Name;
        private StreamImageSource _Image;
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
        public StreamImageSource Image
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

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
