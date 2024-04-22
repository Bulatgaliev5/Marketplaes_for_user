using System.ComponentModel;

namespace Marketplaes02.Model
{
    public class ImagesGoods : INotifyPropertyChanged
    {
        /// <summary>
        /// Поле ImageID
        /// </summary>
        private int _ImageID;
        /// <summary>
        /// Поле Изображение
        /// </summary>
        private string _ImageGoods;
        /// <summary>
        /// Поле ID_goods
        /// </summary>
        private int _ID_goods;


        /// <summary>
        /// Свойсва ImageID
        /// </summary>
        /// 

        public int ImageID
        {
            get => _ImageID;
            set
            {
                _ImageID = value;
                OnPropertyChanged("ImageID");
            }
        }

        /// <summary>
        /// Свойсва ID_goods
        /// </summary>
        /// 

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
        /// Свойства Изображение 
        /// </summary>
        public string ImageGoods
        {
            get => _ImageGoods;
            set
            {
                _ImageGoods = value;
                OnPropertyChanged("ImageGoods");

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
