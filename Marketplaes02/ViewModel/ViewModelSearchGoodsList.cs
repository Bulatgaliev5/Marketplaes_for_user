using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.BD;
using Marketplaes02.Class;
using Marketplaes02.Model;
using Marketplaes02.View;
using Mopups.PreBaked.Services;
using Mopups.Services;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Marketplaes02.ViewModel
{
    public class ViewModelSearchGoodsList : INotifyPropertyChanged
    {
        int ID_user;
        /// <summary>
        /// Список Good во время поиска 
        /// </summary>
        private IList<SearchGoodsList> _Goods;
        public IList<SearchGoodsList> Goods
        {
            get => _Goods;
            set
            {
                _Goods = value;
                OnPropertyChanged("Goods");
            }
        }
        private IList<SearchGoodsList> _AllGoods;
        public IList<SearchGoodsList> AllGoods
        {
            get => _AllGoods;
            set
            {
                _AllGoods = value;
                OnPropertyChanged("AllGoods");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));


        }
        public ICommand ClickOpenSortCommand { get; set; }
        public ICommand ClickOpenFilterCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        private IList<bool> _boolRadioButton;
        public IList<bool> BoolRadioButton
        {
            get => _boolRadioButton;
            set
            {
                _boolRadioButton = value;
                OnPropertyChanged("BoolRadioButton");
            }
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged("SearchText");


                }
            }
        }
        public async Task Search()
        {
            List<string> message = ["Поиск.."];
            await PreBakedMopupService.GetInstance().WrapTaskInLoader(Task.Delay(1000), Color.FromRgb(0, 127, 255), Color.FromRgb(255, 255, 250), message, Color.FromRgb(0, 0, 0));
        }
        private async void SearchBarSearch(string searchText)
        {
            await Search();
            await Task.Run(() =>
            {

                Goods = AllGoods;
                Goods = Goods.Where(a => a.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                  || a.Description.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                OnPropertyChanged("Goods");
            });


        }
        public ViewModelSearchGoodsList()
        {
            Load();
            BoolRadioButton = new List<bool> { true, false, false };

            // Регистрируемся для получения сообщений о сортировке по цене
            WeakReferenceMessenger.Default.Register<UpdateSort>(this, (r, m) =>
            {
                SortGoodsPrice(m.SelectParam);


            });
            ClickOpenSortCommand = new Command(ListboolRadioButton);

            WeakReferenceMessenger.Default.Register<UpdateFillter>(this, (r, m) =>
            {
                FillterGoodsPrice(m.OtPrice, m.DoPrice);

            });
            ClickOpenFilterCommand = new Command(PriceLoad);

            SearchCommand = new Command<string>(SearchBarSearch);


        }
        public async void PriceLoad()
        {
            try
            {
                var minPrice = Goods.Min(goods => goods.Price_with_discount);
                var maxPrice = Goods.Max(goods => goods.Price_with_discount);
                await MopupService.Instance.PushAsync(new ViewFillterGoods(minPrice, maxPrice));
            }
            catch (InvalidOperationException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Уведомление", "У вас в результате поиска 0 товаров " + ex.Message, "Ок");
            }

        }
        private void FillterGoodsPrice(float OtPrice, float DoPrice)
        {
            if (OtPrice != 0 && DoPrice != 3402823)
            {
                Goods = AllGoods;
                Goods = new ObservableCollection<SearchGoodsList>(Goods.Where(goods => goods.Price_with_discount >= OtPrice
                && goods.Price_with_discount <= DoPrice).ToList());
                OnPropertyChanged("Goods");
            }
            else
            {
                Goods = AllGoods;
                OnPropertyChanged("Goods");
            }


        }

        public async void ListboolRadioButton()
        {
            await MopupService.Instance.PushAsync(new ViewSortGoods(BoolRadioButton));
        }
        private async void SortGoodsPrice(string sortOption)
        {

            switch (sortOption)
            {
                case "Дешевле":
                    Goods = new ObservableCollection<SearchGoodsList>(Goods.OrderBy(goods => goods.Price_with_discount));
                    BoolRadioButton[1] = true;
                    BoolRadioButton[0] = false;
                    BoolRadioButton[2] = false;
                    break;
                case "Дороже":
                    Goods = new ObservableCollection<SearchGoodsList>(Goods.OrderByDescending(goods => goods.Price_with_discount));
                    BoolRadioButton[2] = true;
                    BoolRadioButton[0] = false;
                    BoolRadioButton[1] = false;
                    break;
                default:
                    Goods = AllGoods;
                    BoolRadioButton[0] = true;
                    BoolRadioButton[2] = false;
                    BoolRadioButton[1] = false;
                    break;
            }
            OnPropertyChanged("Goods");
        }


        /// <summary>
        /// Метод загрузки изделтй Load
        /// </summary>
        public async void Load()
        {
            ID_user = Preferences.Default.Get("UserID", 0);
            await LoadGoods();
            await ImageIsbrannoeLoad();
            AllGoods = Goods;
        }
        public async Task<bool> ImageIsbrannoeLoad()
        {


            int i = 0;
            for (; i < Goods.Count; i++)
            {

                bool res = await SQLImageIsbrannoeLoad(ID_user, Goods[i].ID_goods);
                if (res)
                {
                    Goods[i].ImageIsbrannoe = "isbrannoe_true.png";
                }
                else
                {

                    Goods[i].ImageIsbrannoe = "isbrannoe.png";
                }

            }

            return true;
        }
        private async Task<bool> SQLImageIsbrannoeLoad(int ID_user, int ID_goods)
        {
            // Строка запроса
            string
                sql = "SELECT * FROM isbrannoe_goods where ID_user=@ID_user AND ID_goods=@ID_goods";


            ConnectBD con = new ConnectBD();
            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());

            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", ID_goods));
            await con.GetConnectBD();

            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = cmd.ExecuteReader();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {

                await con.GetCloseBD();

                return false;
            }

            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {


            }
            OnPropertyChanged("Goods");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return true;
        }
        public async Task<bool> AddSQLImageIsbrannoe(int ID_user, int ID_goods)
        {
            // Строка запроса
            //INSERT INTO `dp_bulat_base`.`isbrannoe_goods` (`ID_goods`, `ID_user`) VALUES (57, 27);
            string
                sql = "INSERT INTO isbrannoe_goods (ID_goods, ID_user) VALUES (@ID_goods, @ID_user)";


            ConnectBD con = new ConnectBD();
            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());

            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", ID_goods));
            await con.GetConnectBD();
            cmd.ExecuteNonQuery();
            OnPropertyChanged("Goods");
            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return true;
        }
        public async Task<bool> SQLImageIsbrannoeDelete(int ID_user, int ID_goods)
        {
            // Строка запроса
            string
                sql = "DELETE FROM isbrannoe_goods where ID_user=@ID_user AND ID_goods=@ID_goods";


            ConnectBD con = new ConnectBD();
            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());

            cmd.Parameters.Add(new MySqlParameter("@ID_user", ID_user));
            cmd.Parameters.Add(new MySqlParameter("@ID_goods", ID_goods));
            await con.GetConnectBD();

            if (await cmd.ExecuteNonQueryAsync() == 1)
            {
                await con.GetCloseBD();
                return true;

            }


            OnPropertyChanged("Goods");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return false;
        }
        /// <summary>
        /// Метод Получения изделий из БД
        /// </summary>
        /// <returns></returns>
        private async Task<bool> LoadGoods()
        {
            // Строка запроса
            string
                sql = "SELECT * FROM goods ORDER BY RAND()";

            // Объявление переменной на основе класс подключения:
            // >    Connector conn
            // Инициализация переменной:
            // >    = new Connector()


            ConnectBD con = new ConnectBD();

            // Объявление объекта команды:
            // >    MySqlCommand cmd
            // Инициализация объекта команды:
            // >    new MySqlCommand(sql, conn.GetConn());
            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());

            // Синхронное подключение к БД
            await con.GetConnectBD();

            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = cmd.ExecuteReader();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Список товаров опусташается
                Goods.Clear();
                // Синхронное отключение от БД
                await con.GetCloseBD();
                // Возращение false
                return false;
            }
            Goods = new ObservableCollection<SearchGoodsList>();
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {

                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)
                Goods.Add(new SearchGoodsList()
                {
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                    Name = reader["Name"].ToString(),
                    Price = Convert.ToSingle(reader["Price"]),
                    Image = reader["ImageGood"].ToString(),
                    Price_with_discount = Convert.ToSingle(reader["Price_with_discount"]),
                    Discount = Convert.ToInt32(reader["Discount"]),
                    Description = reader["Description"].ToString(),
                });

                // await Task.Delay(1000);
            }
            OnPropertyChanged("Goods");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return true;
        }



        public async Task<bool> GoodsSelectSQL(int id)
        {
            string
              sql = "SELECT * FROM goods WHERE  ID_goods=@id";
            ConnectBD
             conn = new ConnectBD();
            MySqlCommand
              cmd = new MySqlCommand(sql, conn.GetConnBD());
            cmd.Parameters.Add(new MySqlParameter("@id", id));
            await conn.GetConnectBD();
            if (await cmd.ExecuteNonQueryAsync() == 1)
            {
                await conn.GetCloseBD();
                return true;

            }
            await conn.GetCloseBD();
            // App.Current.Properties["ID_goods"] = id;
            return false;
        }

    }
}
