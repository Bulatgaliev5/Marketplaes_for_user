using System.ComponentModel;

namespace Marketplaes02.Model
{
    public class Kategoriya : INotifyPropertyChanged
    {
        /// <summary>
        /// Поле ID_katehorii
        /// </summary>
        private int _ID_katehorii;
        /// <summary>
        /// Поле Название товара
        /// </summary>
        private string _Name;
        /// <summary>
        /// Поле Изображение
        /// </summary>
        private string _Image;


        /// <summary>
        /// Свойсва ID_katehorii
        /// </summary>
        /// 

        public int ID_katehorii
        {
            get => _ID_katehorii;
            set
            {
                _ID_katehorii = value;
                OnPropertyChanged("ID_katehorii");
            }
        }
        /// <summary>
        /// Свойсва Название товара
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
        /// Изображение
        /// </summary>

        public string Image
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
