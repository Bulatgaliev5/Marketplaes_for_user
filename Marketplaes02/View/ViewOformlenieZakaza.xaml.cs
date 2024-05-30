
using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.BD;
using Marketplaes02.Class;
using Marketplaes02.Model;
using Marketplaes02.ViewModel;
using Mopups.Services;


namespace Marketplaes02.View;

public partial class ViewOformlenieZakaza : ContentPage
{
    bool result;
    bool hasKey;
    Yoomoney yoomoney = new Yoomoney();
    Thread myThread1;
    Thread myThread2;
    WebView webView;
    VewModelSostavZakaza vewModelSostavZakaza;
    ViewModelKorzina modelKorzina = new ViewModelKorzina();
    string label;
    string link;
    private IList<Korzina> _SostavZakazalist;
    public IList<Korzina> SostavZakazalist
    {
        get => _SostavZakazalist;
        set
        {
            _SostavZakazalist = value;
            OnPropertyChanged("SostavZakazalist");
        }
    }
    public ViewOformlenieZakaza(IList<Korzina> Korzinalist)
    {

        InitializeComponent();
        SostavZakazalist = Korzinalist;
        Update();


    }

    public async void Update()
    {

        
        BindingContext = new VewModelSostavZakaza(SostavZakazalist);
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Проверка наличия ключа
         hasKey = Preferences.Default.ContainsKey("LabelPay");



        // Очистить все ключи
        Preferences.Default.Clear();

        myThread1 = new Thread(CheckPaymentStatus);
        myThread1.Start();

    }

    private bool IsValidText(Label box)
    {
        return string.IsNullOrEmpty(box.Text) || box.Text.Length < 1;
    }
    private async void BtnZakazat(object sender, EventArgs e)
    {
        ButtonZakazat.IsEnabled = false;
        try
        {
            if (!IsValidText(Adres_dostavki))
            {
                await DisplayAlert("Уведомление", "После оплаты вернитесь назад и нажмите на кнопку 'Проверить платеж'", "Ок");
                myThread2 = new Thread(Pay);
                myThread2.Start();

            }
            else
            {
                await DisplayAlert("Уведомление", "Заполните все поля", "Ок");
            }
        }
        finally
        {
            // Разблокировка кнопки
            ButtonZakazat.IsEnabled = true;
        }



    }

    public async void Pay()
    {
        label = modelKorzina.LabelPay;
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
       
        
         result = yoomoney.GetStatusOperazii_and_check(label);
        if (result)
        {
            Preferences.Default.Set("LabelPay", label);
            await this.Dispatcher.DispatchAsync(async () =>
            {
                await DisplayAlert("Сообщение", "Ваш платеж прошел! Спасибо за покупку", "Ок");
                WeakReferenceMessenger.Default.Send(new UpdateSostavZakaza());
                vewModelSostavZakaza = new VewModelSostavZakaza(SostavZakazalist);
                await vewModelSostavZakaza.AddZakazi(label);
                await modelKorzina.Delete_Korzina();
                await Navigation.PopAsync();
                // Если ключ существует, очистить его
                if (hasKey)
                {
                    Preferences.Default.Remove("LabelPay");
                }

            });

        }
        else
        {
            await this.Dispatcher.DispatchAsync(async() =>
            {
                await DisplayAlert("Сообщение", "Сначало оплатите заказ!", "Ок");
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


    }
}