
using Marketplaes02.BD;
using Marketplaes02.ViewModel;
using Dadata;
using Dadata.Model;
using Newtonsoft.Json.Linq;
using System.Linq;
using Geocoding;
using CommunityToolkit.Maui.Views;


namespace Marketplaes02.View;

public partial class ViewOformlenieZakaza : ContentPage
{
    int Count;
    Yoomoney yoomoney = new Yoomoney();


    VewModelSostavZakaza vewModelSostavZakaza = new VewModelSostavZakaza();
    public ViewOformlenieZakaza()
    {
        
        InitializeComponent();
        Update();



    }
    public async void Update()
    {

        BindingContext = new VewModelSostavZakaza();


    }
    private bool IsValidText(Label box)
    {
        return string.IsNullOrEmpty(box.Text) || box.Text.Length < 1;
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

    private async void BtdresDostavki(object sender, EventArgs e)
    {
        var popup = new ViewAddAdres_dostavki();

        this.ShowPopup(popup);
    }
    protected override void OnAppearing()
    {
        //Подписка на сообщения

        MessagingCenter.Subscribe<Page>(this, "ViewOformlenieZakaza", (sender) =>
        {
            Update();
        });
    }


}