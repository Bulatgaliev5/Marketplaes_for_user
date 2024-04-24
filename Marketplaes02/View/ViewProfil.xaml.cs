namespace Marketplaes02.View;

public partial class ViewProfil : ContentPage
{
    public ViewProfil()
    {
        InitializeComponent();
        string UserName = Preferences.Default.Get("UserName", "Unknown");
        NameUser.Text = "Здравствуй, " + UserName;
    }
    
    private void FrameExitIsProfile(object sender, TappedEventArgs e)
    {

    }

    private void FrameO_APP(object sender, TappedEventArgs e)
    {

    }

    private void Selet_Aktev_zakaz(object sender, TappedEventArgs e)
    {

    }

    private void OnButtonTapped(object sender, TappedEventArgs e)
    {

    }
}