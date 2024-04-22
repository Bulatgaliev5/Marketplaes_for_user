using System.ComponentModel;

namespace Marketplaes02.Model
{
    public class OformlenieZakaza : INotifyPropertyChanged
    {
        private string _User_Name;
        private string _User_Number_phone;
        private string _User_Data;
        private float _Goods_Total_Price_with_discount;
        private int _Goods_Total_Count;

        public int Goods_Total_Count
        {
            get => _Goods_Total_Count;
            set
            {
                _Goods_Total_Count = value;
                OnPropertyChanged("Goods_Total_Count");
            }
        }
        public float Goods_Total_Price_with_discount
        {
            get => _Goods_Total_Price_with_discount;
            set
            {
                _Goods_Total_Price_with_discount = value;
                OnPropertyChanged("Goods_Total_Price_with_discount");
            }
        }
        public string User_Name
        {
            get => _User_Name;
            set
            {
                _User_Name = value;
                OnPropertyChanged("User_Name");
            }
        }


        public string User_Number_phone
        {
            get => _User_Number_phone;
            set
            {
                _User_Number_phone = value;
                OnPropertyChanged("User_Number_phone");
            }
        }

        public string User_Data
        {
            get => _User_Data;
            set
            {
                _User_Data = value;
                OnPropertyChanged("User_Data");
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
