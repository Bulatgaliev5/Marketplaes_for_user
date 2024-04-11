using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02.Model
{
    public class OformlenieZakaza : INotifyPropertyChanged
    {
        private int _ID_OformlenieZakaza;

        public int ID_OformlenieZakaza
        {
            get => _ID_OformlenieZakaza;
            set
            {
                _ID_OformlenieZakaza = value;
                OnPropertyChanged("ID_OformlenieZakaza");
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
