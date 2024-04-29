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
    }
}