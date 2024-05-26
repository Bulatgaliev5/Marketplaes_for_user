using Mopups.Pages;
using Mopups.Services;

namespace Marketplaes02.View;

public partial class ViewWebYoomoney : PopupPage
{
	public ViewWebYoomoney(WebView webView)
    {
        InitializeComponent();

        Content = webView;
        webView.Navigating += async (s, e) =>
        {

            e.Cancel = true; // Отменить навигацию
            OnBackButtonPressed(); // Вызовите ваш обработчик
        };
    }
    protected override bool OnBackButtonPressed()
    {
        DisplayAlert("Уведомление", "Нажмите на кнопку 'Проверить платёж'", "Ок");
        Navigation.PopAsync();
        return base.OnBackButtonPressed();
    }
    private async void Close(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}