using Marketplaes02.BD;
using Marketplaes02.Model;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Marketplaes02.ViewModel
{
    public class ViewModelKategoriya : INotifyPropertyChanged
    {
        private IList<Kategoriya> _KategoriyaList;
        public IList<Kategoriya> KategoriyaList
        {
            get => _KategoriyaList;
            set
            {
                _KategoriyaList = value;
                OnPropertyChanged("KategoriyaList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));


        }

        public ViewModelKategoriya()
        {
            Load();
        }


        public async void Load()
        {
            await Loadkategoriya();
        }
        /// <summary>
        /// Метод для загрузки категории
        /// </summary>
        /// <returns></returns>
        private async Task<bool> Loadkategoriya()
        {
            // Строка запроса
            string
                sql = "SELECT * FROM kategoriya";



            ConnectBD con = new ConnectBD();

            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConnBD());

            // Синхронное подключение к БД
            await con.GetConnectBD();

            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Список товаров опусташается
                KategoriyaList.Clear();
                // Синхронное отключение от БД
                await con.GetCloseBD();
                // Возращение false
                return false;
            }
            KategoriyaList = new ObservableCollection<Kategoriya>();
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {

                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)
                KategoriyaList.Add(new Kategoriya()
                {
                    ID_katehorii = Convert.ToInt32(reader["id_kategoriya"]),
                    Name = reader["Name"].ToString(),

                    Image = reader["Image"].ToString(),
                });

            }
            OnPropertyChanged("KategoriyaList");

            // Синхронное отключение от БД
            await con.GetCloseBD();
            // Возращение true
            return true;
        }
    }
}
