using Marketplaes02.Model;
using System.ComponentModel;
using System.Text.Json;


namespace Marketplaes02.ViewModel
{
    public class VewModelSostavZakaza : OformlenieZakaza, INotifyPropertyChanged
    {
        int ID_goods;
        private IList<SostavZakaza> _SostavZakazalist;
        public IList<SostavZakaza> SostavZakazalist
        {
            get => _SostavZakazalist;
            set
            {
                _SostavZakazalist = value;
                OnPropertyChanged("SostavZakazalist");
            }
        }
        public VewModelSostavZakaza()
        {

            Load();

        }
        public async void Load()
        {


            SostavZakazalist = GetList<SostavZakaza>("Sostavzakazalist");
            User_Name = Preferences.Default.Get("UserName", "Unknown");
            User_Number_phone = Preferences.Default.Get("UserNumber_phone", "Unknown");
            User_Data = GetUser_Data();
            Goods_Total_Price_with_discount = GetGoods_Total_Price_with_discount();
            Goods_Total_Count = GetGoods_Total_Count();
            //  ID_goods = Preferences.Default.Get("ID_goods", 0);
            //  await LoadSostavZakazalist(ID_goods);
        }

        public float GetGoods_Total_Price_with_discount()
        {
            return SostavZakazalist.Sum(item => item.Price_with_discount);
        }

        public int GetGoods_Total_Count()
        {
            return SostavZakazalist.Sum(item => item.Count);
        }


        public string GetUser_Data()
        {
            return User_Name + ", " + User_Number_phone;
        }



        public static List<SostavZakaza> GetList<SostavZakaza>(string key)
        {
            var jsonString = Preferences.Get(key, string.Empty);
            return string.IsNullOrEmpty(jsonString) ? null : JsonSerializer.Deserialize<List<SostavZakaza>>(jsonString);
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
