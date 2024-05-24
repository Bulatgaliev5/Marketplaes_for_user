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
    public class ViewModelGoodsKategoriya : INotifyPropertyChanged
    {
        int id_kategoriya, ID_user;
        private string _NameKategoriya;
        public string NameKategoriya
        {
            get => _NameKategoriya;
            set
            {
                _NameKategoriya = value;
                OnPropertyChanged("NameKategoriya");

            }
        }
        /// <summary>
        /// Список Good
        /// </summary>
        private IList<GoodsKategoriya> _GoodsKategoriyalist;
        public IList<GoodsKategoriya> GoodsKategoriyalist
        {
            get => _GoodsKategoriyalist;
            set
            {
                _GoodsKategoriyalist = value;
                OnPropertyChanged("GoodsKategoriyalist");
            }
        }

        private IList<GoodsKategoriya> _AllGoodsKategoriyalist;
        public IList<GoodsKategoriya> AllGoodsKategoriyalist
        {
            get => _AllGoodsKategoriyalist;
            set
            {
                _AllGoodsKategoriyalist = value;
                OnPropertyChanged("AllGoodsKategoriyalist");
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
            await  Search();
            await Task.Run(() =>
            {

                GoodsKategoriyalist = AllGoodsKategoriyalist;
                GoodsKategoriyalist = GoodsKategoriyalist.Where(a => a.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                  || a.Description.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                OnPropertyChanged("GoodsKategoriyalist");
            });

           
        }
        public ViewModelGoodsKategoriya()
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
                var minPrice = GoodsKategoriyalist.Min(goods => goods.Price_with_discount);
                var maxPrice = GoodsKategoriyalist.Max(goods => goods.Price_with_discount);
                await MopupService.Instance.PushAsync(new ViewFillterGoods(minPrice, maxPrice));
            }
            catch (InvalidOperationException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Уведомление","У вас в результате поиска 0 товаров " + ex.Message, "Ок");
            }

        }
        private async void FillterGoodsPrice(float OtPrice, float DoPrice)
        {
            if (OtPrice!=0 && DoPrice!= 3402823)
            {
                GoodsKategoriyalist = AllGoodsKategoriyalist;
                GoodsKategoriyalist = new ObservableCollection<GoodsKategoriya>(GoodsKategoriyalist.Where(goods => goods.Price_with_discount >= OtPrice 
                && goods.Price_with_discount <= DoPrice).ToList());
                OnPropertyChanged("GoodsKategoriyalist");
            }
            else
            {
                GoodsKategoriyalist = AllGoodsKategoriyalist;
                OnPropertyChanged("GoodsKategoriyalist");
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
                    GoodsKategoriyalist = new ObservableCollection<GoodsKategoriya>(GoodsKategoriyalist.OrderBy(goods => goods.Price_with_discount));
                    BoolRadioButton[1] = true;
                    BoolRadioButton[0] = false;
                    BoolRadioButton[2] = false;
                    break;
                case "Дороже":
                    GoodsKategoriyalist = new ObservableCollection<GoodsKategoriya>(GoodsKategoriyalist.OrderByDescending(goods => goods.Price_with_discount));
                    BoolRadioButton[2] = true;
                    BoolRadioButton[0] = false;
                    BoolRadioButton[1] = false;
                    break;
                default:
                    GoodsKategoriyalist = AllGoodsKategoriyalist;
                    BoolRadioButton[0] = true;
                    BoolRadioButton[2] = false;
                    BoolRadioButton[1] = false;
                    break;
            }
            OnPropertyChanged("GoodsKategoriyalist");
        }


        /// <summary>
        /// Метод загрузки изделтй Load
        /// </summary>
        public async void Load()
        {
            ID_user = Preferences.Default.Get("UserID", 0);
            id_kategoriya = Preferences.Default.Get("id_kategoriya", 0);
            await LoadGoods(id_kategoriya);
            await ImageIsbrannoeLoad();
            AllGoodsKategoriyalist = GoodsKategoriyalist;
        }
        /// <summary>
        /// Метод Получения изделий из БД
        /// </summary>
        /// <returns></returns>
        private async Task<bool> LoadGoods(int id_kategoriya)
        {
            // Строка запроса
            string
                sql = "SELECT *, k.Name AS NameKategoriya FROM goods g " +
                "JOIN kategoriya k ON k.id_kategoriya = g.id_kategoriya " +
                "WHERE k.id_kategoriya=@id_kategoriya";

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
            cmd.Parameters.Add(new MySqlParameter("@id_kategoriya", id_kategoriya));

            // Синхронное подключение к БД
            await con.GetConnectBD();

            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Список товаров опусташается
                GoodsKategoriyalist.Clear();
                // Синхронное отключение от БД
                await con.GetCloseBD();
                // Возращение false
                return false;
            }
            GoodsKategoriyalist = new ObservableCollection<GoodsKategoriya>();
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {

                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)
                GoodsKategoriyalist.Add(new GoodsKategoriya()
                {
                    ID_goods = Convert.ToInt32(reader["ID_goods"]),
                    Name = reader["Name"].ToString(),
                    Price = Convert.ToSingle(reader["Price"]),
                    Image = reader["ImageGood"].ToString(),
                    Price_with_discount = Convert.ToSingle(reader["Price_with_discount"]),
                    Discount = Convert.ToInt32(reader["Discount"]),
                    Description = reader["Description"].ToString(),
                });
                NameKategoriya = reader["NameKategoriya"].ToString();
                // await Task.Delay(1000);
            }
            
            OnPropertyChanged("NameKategoriya");
            OnPropertyChanged("GoodsKategoriyalist");
           
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


        public async Task<bool> ImageIsbrannoeLoad()
        {


            int i = 0;
            for (; i < GoodsKategoriyalist.Count; i++)
            {

                bool res = await SQLImageIsbrannoeLoad(ID_user, GoodsKategoriyalist[i].ID_goods);
                if (res)
                {
                    GoodsKategoriyalist[i].ImageIsbrannoe = "isbrannoe_true.png";
                }
                else
                {

                    GoodsKategoriyalist[i].ImageIsbrannoe = "isbrannoe.png";
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
                 reader = await cmd.ExecuteReaderAsync();

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
            OnPropertyChanged("GoodsKategoriyalist");

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
            await cmd.ExecuteNonQueryAsync();
            OnPropertyChanged("GoodsKategoriyalist");
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


            OnPropertyChanged("GoodsKategoriyalist");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return false;
        }
    }
}
