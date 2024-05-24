using Mopups.Pages;
using Mopups.Services;

namespace Marketplaes02.View;

public partial class ViewWebYoomoney : PopupPage
{
	public ViewWebYoomoney()
	{
		InitializeComponent();
		
        //webView.Navigating += (s, e) =
        //{
        //    var url = e.Url;
        //    var str = "https://yoomoney.ru/transfer/process/success";
        //    if (url.Contains(str)) // Замените на ваше конкретное условие
        //    {
        //        e.Cancel = true; // Отменить навигацию
        //        OnBackButtonPressed(); // Вызовите ваш обработчик
        //    }
        //};
    }

    private async void Close(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}