using System.ComponentModel;

namespace Marketplaes02.Model
{
    public class Kategoriya : INotifyPropertyChanged
    {

        private int _ID_katehorii;
        private string _Name;
        private string _Image;
        public int ID_katehorii
        {
            get => _ID_katehorii;
            set
            {
                _ID_katehorii = value;
                OnPropertyChanged("ID_katehorii");
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
