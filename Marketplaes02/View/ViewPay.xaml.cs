using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.Class;

namespace Marketplaes02.View;

public partial class ViewPay : ContentPage
{
    public ViewPay(WebView webView)
    {
        InitializeComponent();

        Content = webView;
    }
    protected override bool OnBackButtonPressed()
    {
        DisplayAlert("Уведомление", "Нажмите на кнопку 'Проверить платёж'", "Ок");
        WeakReferenceMessenger.Default.Send(new UpdateSostavZakaza());
        return base.OnBackButtonPressed();

    }
}
