

namespace Marketplaes02.Class
{
    /// <summary>
    /// Класс для фильтрации данных
    /// </summary>
    public class UpdateFillter
    {
        public float OtPrice { get; }
        public float DoPrice { get; }
        public UpdateFillter(float otPrice, float doPrice)
        {
            OtPrice = otPrice;
            DoPrice = doPrice;
        }
    }

}
