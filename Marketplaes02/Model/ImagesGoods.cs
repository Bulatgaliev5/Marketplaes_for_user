using System.ComponentModel;

namespace Marketplaes02.Model
{
    public class ImagesGoods : INotifyPropertyChanged
    {
        private int _ImageID;
        private ImageSource _ImageGoods;
        private int _ID_goods;
        public int ImageID
        {
            get => _ImageID;
            set
            {
                _ImageID = value;
                OnPropertyChanged("ImageID");
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
        public ImageSource ImageGoods
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
