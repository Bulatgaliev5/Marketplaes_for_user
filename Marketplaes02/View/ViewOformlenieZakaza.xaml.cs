using Marketplaes02.BD;
using Marketplaes02.ViewModel;

using Newtonsoft.Json.Linq;
using System.Linq;

namespace Marketplaes02.View;

public partial class ViewOformlenieZakaza : ContentPage
{
    int Count;
    Yoomoney yoomoney = new Yoomoney();


    VewModelSostavZakaza vewModelSostavZakaza = new VewModelSostavZakaza();
    public ViewOformlenieZakaza()
    {
        
        InitializeComponent();
        BindingContext = new VewModelSostavZakaza();
        

    }

    private async void BtnZakazat(object sender, EventArgs e)
    {


        bool result = Pay();
        if (result)
        {

        }
        else
        {

        }


    }

    public bool Pay()
    {
        var link = yoomoney.GetPayLink(
            Convert.ToDecimal(vewModelSostavZakaza.Goods_Total_Price_with_discount),
            vewModelSostavZakaza.SostavZakazalist.Select(s => s.Name).ToList());

        WebView webView = new WebView
        {
            Source = link
        };
        Content = webView;
        return true;
    }

    private void BtdresDostavki(object sender, EventArgs e)
    {
        
    }
}