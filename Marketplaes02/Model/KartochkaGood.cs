using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Marketplaes02.Model
{
    public class KartochkaGood : INotifyPropertyChanged
    {

        /// <summary>
        /// Поле ID_goods
        /// </summary>
        private int _ID_goods;
        /// <summary>
        /// Поле Название изделия
        /// </summary>
        private string _Name;
        /// <summary>
        /// Поле Цена
        /// </summary>
        private float _Price;

        /// <summary>
        /// Поле Цена
        /// </summary>
        private float _Price_with_discount;

        /// <summary>
        /// Поле Изображение изделия
        /// </summary>
        private ImageSource _Image;

        /// <summary>
        /// Поле Описание товара
        /// </summary>
        private string _Description;

        private int _Count;

        private int _V_nalichii;

        /// <summary>
        /// Свойство Описание товара
        /// </summary>
        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                OnPropertyChanged("Description");

            }
        }

        /// <summary>
        /// Свойсва ID_goods
        /// </summary>
        /// 

        public int V_nalichii
        {
            get => _V_nalichii;
            set
            {
                _V_nalichii = value;
                OnPropertyChanged("V_nalichii");
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
        /// <summary>
        /// Свойсва Название изделия
        /// </summary>
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
        /// Свойства цена
        /// </summary>
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

        public ImageSource Image
        {
            get => _Image;
            set
            {
                _Image = value;
                OnPropertyChanged("Image");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
