using System.ComponentModel;


namespace Marketplaes02.Model
{
    public class Isbrannoe : INotifyPropertyChanged
    {
        private int _Discount;
        private float _Price_with_discount;
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
        /// Поле Изображение изделия
        /// </summary>
        private ImageSource _Image;


        private string _ImageIsbrannoe;
        public string ImageIsbrannoe
        {
            get => _ImageIsbrannoe;
            set
            {
                _ImageIsbrannoe = value;
                OnPropertyChanged("ImageIsbrannoe");

            }
        }
        /// <summary>
        /// Свойсва ID_goods
        /// </summary>
        /// 
        public int Discount
        {
            get => _Discount;
            set
            {
                _Discount = value;
                OnPropertyChanged("Discount");
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
        /// Свойсва Название
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
        /// <summary>
        /// Свойства Изображение товара
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }

    }
}
