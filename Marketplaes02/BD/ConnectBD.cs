using MySqlConnector;

namespace Marketplaes02.BD
{
    public class ConnectBD
    {
        // Строка подключения
        private static readonly string txt =
        "Server=37.18.74.116;" +
        "Port=3309;" +
        "Database=dp_bulat_base;" +
        "UserID=g_bulat;" +
        "Password=bulat.123;" +
        "CharacterSet=utf8mb4;" +
        "ConvertZeroDatetime=True;" +
        "AllowZeroDatetime=True; Allow User Variables = True";

        private readonly MySqlConnection con = new MySqlConnection(txt);

        /// <summary>
        /// Метод синхронного подключения к БД с объекта подключения 
        /// </summary>
        /// <returns></returns>
        public async Task GetConnectBD()
        {
            await con.OpenAsync();
        }
        /// <summary>
        /// Метод синхронного отключения от БД с объекта подключения
        /// </summary>
        /// <returns></returns>
        public async Task GetCloseBD() => await con.CloseAsync();

        /// <summary>
        /// Метод возвращения объекта подключения
        /// </summary>
        /// <returns></returns>
        public MySqlConnection GetConnBD() => con;


    }
}
