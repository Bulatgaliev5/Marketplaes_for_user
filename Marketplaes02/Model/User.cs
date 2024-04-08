using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02.Model
{
    public class User : INotifyPropertyChanged
    {
        private int _id_user;
        private string _name;
        private string _login;
        private string _pass;


        public int ID_user
        {
            get => _id_user;
            set
            {
                _id_user = value;
                OnPropertyChanged("ID_user");
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");

            }
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged("Login");

            }
        }

        public string Pass
        {
            get => _pass;
            set
            {
                _pass = value;
                OnPropertyChanged("Pass");

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
