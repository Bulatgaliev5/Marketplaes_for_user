using Marketplaes02.Class;
using System.ComponentModel;

namespace Marketplaes02.Model
{
    public class GoodsKategoriya : Notify
    {
        private int _Discount;
        private float _Price_with_discount;
        private string _Description;
        private int _ID_goods;
        private string _Name;
        private float _Price;
        private ImageSource _Image;
        private string _ImageIsbrannoe;
        public string ImageIsbrannoe
        {
            get => _ImageIsbrannoe;
            set
            {
                _ImageIsbrannoe = value;
                OnPropertyChanged();

            }
        }
        public int Discount
        {
            get => _Discount;
            set
            {
                _Discount = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                OnPropertyChanged();

            }
        }
        public int ID_goods
        {
            get => _ID_goods;
            set
            {
                _ID_goods = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged();

            }
        }
        public float Price
        {
            get => _Price;
            set
            {
                _Price = value;
                OnPropertyChanged();

            }
        }
        public float Price_with_discount
        {
            get => _Price_with_discount;
            set
            {
                _Price_with_discount = value;
                OnPropertyChanged();

            }
        }
        public ImageSource Image
        {
            get => _Image;
            set
            {
                _Image = value;
                OnPropertyChanged();

            }
        }
    }
}
