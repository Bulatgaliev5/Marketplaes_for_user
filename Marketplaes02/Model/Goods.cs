using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Marketplaes02.Model
{
    public class Goods : INotifyPropertyChanged
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
        /// Поле Изображение изделия
        /// </summary>
        private string _Image;

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
        /// <summary>
        /// Свойства Изображение изделия
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
