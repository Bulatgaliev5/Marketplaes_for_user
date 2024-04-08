using Marketplaes02.ViewModel;


namespace Marketplaes02.View;


public partial class ViewKategoriya : ContentPage
{
	public ViewKategoriya()
	{
		InitializeComponent();
        BindingContext = new ViewModelKategoriya();
    }

    private void OpenKategoriya(object sender, TappedEventArgs e)
    {

    }
}