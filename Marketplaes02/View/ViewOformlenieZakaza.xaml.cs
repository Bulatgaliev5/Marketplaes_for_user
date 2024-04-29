
using Marketplaes02.BD;
using Marketplaes02.ViewModel;
using Dadata;
using Dadata.Model;
using Newtonsoft.Json.Linq;
using System.Linq;
using Geocoding;
using CommunityToolkit.Maui.Views;
using System.Net;


namespace Marketplaes02.View;

public partial class ViewOformlenieZakaza : ContentPage
{
    int Count;
    Yoomoney yoomoney = new Yoomoney();

    WebView webView;
    VewModelSostavZakaza vewModelSostavZakaza = new VewModelSostavZakaza();
    ViewModelKorzina modelKorzina = new ViewModelKorzina();
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

        if (!IsValidText(Adres_dostavki))
        {
           
                bool resultAddzakaz = await vewModelSostavZakaza.AddZakazi();
                bool resultDeletekorzina = await modelKorzina.Delete_Korzina();
                if (resultAddzakaz && resultDeletekorzina)
                {
                    modelKorzina.Load();
                    await DisplayAlert("Уведомление", "Оплата прошла", "Ок");
                }
                else{
                    await DisplayAlert("Уведомление", "Оплата  не прошла", "Ок");
            

                }
           
        }
        else
        {
            await DisplayAlert("Уведомление", "Заполните все поля", "Ок");
        }


    }

    public  bool Pay()
    {
        var link = yoomoney.GetPayLink(
            Convert.ToDecimal(vewModelSostavZakaza.Goods_Total_Price_with_discount),
            vewModelSostavZakaza.SostavZakazalist.Select(s => s.Name).ToList());

          webView = new WebView
        {
            Source = link
            
        };
        Content = webView;
       

        var result = yoomoney.GetStatusOperazii_and_check();
        if (result)
        {
            return true;
        }
        return false;

    }

    // обрабатываем завершение перехода
    void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        if (webView.CanGoBack)
        {
            webView.GoBack();
        }
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