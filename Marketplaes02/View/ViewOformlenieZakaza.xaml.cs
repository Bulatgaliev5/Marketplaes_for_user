
using Marketplaes02.BD;
using Marketplaes02.ViewModel;
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
           await DisplayAlert("Уведомление", "После оплаты нажмите на кнопку 'Проверить платеж'", "Ок");
            myThread2 = new Thread(Pay);
            myThread2.Start();
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
        webView = new WebView
        {
            Source = link

        };
        await this.Dispatcher.DispatchAsync(async () =>
       {
           //Content = webView;
           await Navigation.PushAsync(new ViewPay(webView));
       });


    }

    private async void CheckPaymentStatus()
    {
       
        
        var result = yoomoney.GetStatusOperazii_and_check(label);
        if (result)
        {
            await this.Dispatcher.DispatchAsync(async ()  =>
            {
                await DisplayAlert("Сообщение", "Ваш платеж прошел! Спасибо за покупку", "Ок");
                await vewModelSostavZakaza.AddZakazi();
                await modelKorzina.Delete_Korzina();
                await Navigation.PopAsync();


            });

        }
        else
        {
            await this.Dispatcher.DispatchAsync(async() =>
            {
                await DisplayAlert("Сообщение", "Ваш платеж не прошел", "Ок");
            });
           
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

    private void BtnProverit(object sender, EventArgs e)
    {
        myThread1 = new Thread(CheckPaymentStatus);
        myThread1.Start();
    }
}