namespace Marketplaes02.View;

public partial class ViewProfil : ContentPage
{
    public ViewProfil()
    {
        InitializeComponent();
        string UserName = Preferences.Default.Get("UserName", "Unknown");
        NameUser.Text = "Здравствуй, " + UserName;
    }
    

    private async Task AnimateButton(StackLayout frame)
    {
        await frame.ScaleTo(0.9, 100);
        await frame.FadeTo(0.7, 100);
        await frame.ScaleTo(1, 100);
        await frame.FadeTo(1, 100);
    }






    private async void ClickStackLayout(object sender, TappedEventArgs e)
    {
        if (sender is StackLayout frame)
        {
            await AnimateButton(frame);
        }
        var currentPage = Navigation.NavigationStack.LastOrDefault();
        if (!(currentPage is ViewMyOrders))
        {
            await Navigation.PushAsync(new ViewMyOrders());

        }

    }

    private async void ClickStackLayout_isbrannoe(object sender, TappedEventArgs e)
    {
        if (sender is StackLayout frame)
        {
            await AnimateButton(frame);
        }
        var currentPage = Navigation.NavigationStack.LastOrDefault();
        if (!(currentPage is ViewIsbrannoe))
        {
            await Navigation.PushAsync(new ViewIsbrannoe());

        }

    }

    private async void ClickStackLayout_Exit(object sender, TappedEventArgs e)
    {
        if (sender is StackLayout frame)
        {
            await AnimateButton(frame);
        }
        bool result = await DisplayAlert("Подтвердить действие", "Вы действительно хотите выйти?", "Да", "Нет");
        if (result)
        {
            Application.Current.MainPage = new AppShell();
        }
    }
}