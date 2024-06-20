using System.ComponentModel;

namespace Marketplaes02.Model
{
    public class SostavZakaza : INotifyPropertyChanged
    {

        private int _Count;
        private float _Price_with_discount;

        private int _ID_goods;

        private string _Name;

        private float _Price;

        private StreamImageSource _Image;
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
        public int ID_goods
        {
            get => _ID_goods;
            set
            {
                _ID_goods = value;
                OnPropertyChanged("ID_goods");
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

        public StreamImageSource Image
        {
            get => _Image;
            set
            {
                _Image = value;
                OnPropertyChanged("Image");

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
