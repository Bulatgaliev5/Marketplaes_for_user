using Yoomoney_API_username_Bulatgaliev5.account;
using Yoomoney_API_username_Bulatgaliev5.authorize;
using Yoomoney_API_username_Bulatgaliev5.quickpay;
using Yoomoney_API_username_Bulatgaliev5.notification;
using Yoomoney_API_username_Bulatgaliev5.operation_details;
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


          
            var YOUR_СODEvar = "https://t.me/Bulatgaliev53?code=4B70019CF7D4DBDF050F0CCE6835FC3A67725D483596F08AF549D867FB5BF0350736A5BC7C52729D7A23F9F85788EB9D1AB115A2C5205098412E1A80D4087D51A3680393A9797BB674CFFCA0F7AA07CA2A1B45ECFCB48CDB78967239CE2952912BE9DD544DBB6E50BB1D5DBDFF959EAE28E4481B5D013A7247C9DC70CDD683C2";
             token = await authorize.GetAccessToken(code: YOUR_СODEvar, clientId: clientIdvar, redirectUri: redirectUrivar);

            //  string YOUR_TOKENvar = "410015744747795.24F209EDDEE737D82CF3B7A4C5D185E76F39A4427A5321B085D97AC9ECB7BC63CDCBF5A6858F59D21D4E6C4B4267673F05ED9162938B8305A2C36998A08059322AFB193B9DC4261292D7B0D38E6A64009957E43CBD8E8C56CBDDFD3CD1FF39CE3D697F9B9B3D792D57D4B9F3EBAAA9C65A9B0E34E8A11550EB8EA5A150583F3B";
        

        }


        public string GetPayLink(decimal sum, List<string> comment)
        {
            //  string YOUR_TOKENvar = "410015744747795.24F209EDDEE737D82CF3B7A4C5D185E76F39A4427A5321B085D97AC9ECB7BC63CDCBF5A6858F59D21D4E6C4B4267673F05ED9162938B8305A2C36998A08059322AFB193B9DC4261292D7B0D38E6A64009957E43CBD8E8C56CBDDFD3CD1FF39CE3D697F9B9B3D792D57D4B9F3EBAAA9C65A9B0E34E8A11550EB8EA5A150583F3B";
              var label = Guid.NewGuid().ToString();
            var quickpay = new Quickpay(receiver: "410015744747795", quickpayForm: "shop", sum: sum, comment: comment.ToString(),
                label: label, paymentType: "AC");
            return quickpay.LinkPayment;
        }

        public async Task< bool> GetStatusOperazii_and_check()
        {
            string YOUR_TOKENvar = "410015744747795.3A16C76AEC3568152E277959620EC7F993709927B27B64D605D964BA34E3787B8B1C00BD7E5BF9612CA8AFDAAAA106DBE72AE3119C1AF99A995A79DE8FEFA27E9688697456B7C7EA38D6CB274CF855B894EF4EDF7B48A1CA08E1D7915E1F9E178FA66E63016C193156FE570E49159661E360BE4CAD388B4D5BE1917E1308E91B";
            var client = new Client(token: YOUR_TOKENvar);
            var operationrHistory =  client.GetOperationHistory(token: YOUR_TOKENvar);
           
            return true;
        }
    }   
}
