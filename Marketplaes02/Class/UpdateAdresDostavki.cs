

namespace Marketplaes02.Class
{
    /// <summary>
    /// Класс для обновления адреса доставки
    /// </summary>
    public class UpdateAdresDostavki
    {
        public string SelectAdresDostavki { get; }

        public UpdateAdresDostavki(string selectAdresDostavki)
        {
            SelectAdresDostavki = selectAdresDostavki;
        }
    }
}
