
using yoomoney_api.account;

using yoomoney_api.quickpay;


namespace Marketplaes02.BD
{
    public class Yoomoney
    {
        public string authorizevar;
        public string link;
        /// <summary>
        /// Метод для получения ссылки для оплаты
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public string GetPayLink(decimal sum, string label)
        {

            var quickpay = new Quickpay(receiver: "410015744747795", quickpayForm: "shop", sum: sum,
                label: label, paymentType: "AC");
            return quickpay.LinkPayment;
        }
        /// <summary>
        /// Метод проверки статуса заказа
        /// </summary>
        /// <param name="LabelParam"></param>
        /// <returns></returns>
        public bool GetStatusOperazii_and_check(string LabelParam)
        {
            if (LabelParam!=null)
            {

                    string YOUR_TOKENvar = "410015744747795.04C768471489B8989C7285F4B0DA2AA628D22A8558ACD7DB3437BF6CAE210AFD15A3A18A938FA71CA8CE9937B5352080C7A7F7C9AA4226FBBC1E72718F21F90449F0784E8D5010D87E74CAB083A9D6CE36B733129215FFEF0B05288446F4DF719CDED37031E130A30079BCB5BEDDA7AE900E1D5C46C52308698F5DB1E6B6B478";
                    var client = new Client(token: YOUR_TOKENvar);
                    ///Для проверки платежа
                    var operationrHistory = client.GetOperationHistory(token: YOUR_TOKENvar);

                    foreach (var Items in operationrHistory.Operations)
                    {
                        if (Items.Label == LabelParam && Items.Status == "success")
                        {
                            return true;
                        }

                    }
            }
            return false;

        }
    }   
}
