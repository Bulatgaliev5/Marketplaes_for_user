
using Marketplaes02.BD;
using Marketplaes02.ViewModel;
using Dadata;
using Dadata.Model;
using Newtonsoft.Json.Linq;
using System.Linq;
using Geocoding;
using CommunityToolkit.Maui.Views;
using System.Net;
using Mopups.Services;


namespace Marketplaes02.View;

public partial class ViewOformlenieZakaza : ContentPage
{
    int Count;
    Yoomoney yoomoney = new Yoomoney();
    Thread myThread1;
    Thread myThread2;
    WebView webView;
    VewModelSostavZakaza vewModelSostavZakaza = new VewModelSostavZakaza();
    ViewModelKorzina modelKorzina = new ViewModelKorzina();
    string label;
    string link;
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
            myThread2 = new Thread(Pay);
            myThread2.Start();


           //await vewModelSostavZakaza.AddZakazi();
           //await modelKorzina.Delete_Korzina();

           // myThread2.Join();
           //myThread1 = new Thread(CheckPaymentStatus);
           //myThread1.Start();

        }
        else
        {
            await DisplayAlert("Уведомление", "Заполните все поля", "Ок");
        }


    }

    public async void Pay()
    {
        label = Guid.NewGuid().ToString();
        link = yoomoney.GetPayLink(
              Convert.ToDecimal(10), label);
        // myThread2.Join();
        webView = new WebView
        {
            Source = link

        };
        webView.Navigated +=  WebView_Navigated;

        //myThread2.Join();

        await this.Dispatcher.DispatchAsync(() =>
       {
           
           Content = webView;
       });

       

       


    }
    protected override bool OnBackButtonPressed()
    {
         DisplayAlert("Уведомление", "Ошибка", "Ок");
         return base.OnBackButtonPressed();
    }

    private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        if (e.Result == WebNavigationResult.Success)
        {
            // Обработка успешного завершения навигации
            var s = webView;
           // await DisplayAlert("Уведомление", "После оплаты вернитесь назад и нажмите на кнопку 'Проверить платеж' ", "Ок");
            webView.Navigating += async (s, e) =>
            {
                var url = e.Url;
                var str = "https://yoomoney.ru/transfer/process/success";
                if (url.Contains(str)) // Замените на ваше конкретное условие
                {
                    await vewModelSostavZakaza.AddZakazi();
                    await modelKorzina.Delete_Korzina();
                    await this.Navigation.PopAsync();
                    await MopupService.Instance.PushAsync(new ViewWebYoomoney());

                }
            };
        }
        else if (e.Result == WebNavigationResult.Failure)
        {
            // Обработка неудачной навигации
            await DisplayAlert("Уведомление", "Ошибка", "Ок");
        }



    }

    //protected override bool OnBackButtonPressed()
    //{
    //    if (webView.CanGoBack)
    //    {
    //        webView.GoBack();
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    private void CheckPaymentStatus()
    {
       
        
        var result = yoomoney.GetStatusOperazii_and_check(label);
        myThread1.Join();
        if (result)
        {
            DisplayAlert("Сообщение", "Ваш платеж виден", "Ок");
        }
        else
        {
            DisplayAlert("Сообщение", "Ошибка не виден", "Ок");
        }

    }



    private async void BtdresDostavki(object sender, EventArgs e)
    {

        await MopupService.Instance.PushAsync(new ViewAddAdres_dostavki());

    }


    private async void RefreshGoodsData(object sender, EventArgs e)
    {
        Update();
        await Task.Delay(1000);
        RefreshView1.IsRefreshing = false;
    }
}