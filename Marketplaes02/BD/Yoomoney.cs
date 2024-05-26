
using yoomoney_api.account;
using yoomoney_api.authorize;
using yoomoney_api.quickpay;
using yoomoney_api.notification;
using yoomoney_api.operation_details;
using Microsoft.Maui;



namespace Marketplaes02.BD
{
    public class Yoomoney
    {
        public string authorizevar;
        string token;
        public string link;
        Authorize authorize;
        private async void GetToken()
        {
            //Marketplaes02

            var clientIdvar = "56436F1B322FEB2269D708DBF543ACDCA962C113823AE7808604B4B3B9E68DC4";
            var redirectUrivar = "https://t.me/Bulatgaliev53";

            Authorize authorize = new(clientId: clientIdvar, redirectUri: redirectUrivar, scope: new[]
            {
                "account-info",
                "operation-history",
                "operation-details",
                "incoming-transfers",
                "payment-p2p",
            });
           

            var YOUR_СODEvar = "https://t.me/Bulatgaliev53?code=4B70019CF7D4DBDF050F0CCE6835FC3A67725D483596F08AF549D867FB5BF0350736A5BC7C52729D7A23F9F85788EB9D1AB115A2C5205098412E1A80D4087D51A3680393A9797BB674CFFCA0F7AA07CA2A1B45ECFCB48CDB78967239CE2952912BE9DD544DBB6E50BB1D5DBDFF959EAE28E4481B5D013A7247C9DC70CDD683C2";
             token = await authorize.GetAccessToken(code: YOUR_СODEvar, clientId: clientIdvar, redirectUri: redirectUrivar);

            //  string YOUR_TOKENvar = "410015744747795.24F209EDDEE737D82CF3B7A4C5D185E76F39A4427A5321B085D97AC9ECB7BC63CDCBF5A6858F59D21D4E6C4B4267673F05ED9162938B8305A2C36998A08059322AFB193B9DC4261292D7B0D38E6A64009957E43CBD8E8C56CBDDFD3CD1FF39CE3D697F9B9B3D792D57D4B9F3EBAAA9C65A9B0E34E8A11550EB8EA5A150583F3B";
        

        }


        public string GetPayLink(decimal sum, string label)
        {
            // string YOUR_TOKENvar = "410015744747795.24F209EDDEE737D82CF3B7A4C5D185E76F39A4427A5321B085D97AC9ECB7BC63CDCBF5A6858F59D21D4E6C4B4267673F05ED9162938B8305A2C36998A08059322AFB193B9DC4261292D7B0D38E6A64009957E43CBD8E8C56CBDDFD3CD1FF39CE3D697F9B9B3D792D57D4B9F3EBAAA9C65A9B0E34E8A11550EB8EA5A150583F3B";

            var quickpay = new Quickpay(receiver: "410015744747795", quickpayForm: "shop", sum: sum,
                label: label, paymentType: "AC");
            return quickpay.LinkPayment;
        }

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
