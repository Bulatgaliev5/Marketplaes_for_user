using Marketplaes02.Model;
using Marketplaes02.ViewModel;


namespace Marketplaes02.View;


public partial class ViewKategoriya : ContentPage
{
    public ViewKategoriya()
    {
        InitializeComponent();
        BindingContext = new ViewModelKategoriya();
    }

    private async void OpenKategoriya(object sender, TappedEventArgs e)
    {
        if (sender is StackLayout b && b.BindingContext is Kategoriya g)
        {

            Preferences.Default.Set("id_kategoriya", g.ID_katehorii);
        };
        await Navigation.PushAsync(new ViewGoodsKategoriya());
    }

    private async void OpenKartochkaImage(object sender, TappedEventArgs e)
    {
        if (sender is Image b && b.BindingContext is Kategoriya g)
        {

            Preferences.Default.Set("id_kategoriya", g.ID_katehorii);
        };
        await Navigation.PushAsync(new ViewGoodsKategoriya());
    }

    private void SearchBarSearch(object sender, TextChangedEventArgs e)
    {

    }

    private async void FocusedSearchBar(object sender, FocusEventArgs e)
    {
        await Navigation.PushModalAsync(new ViewSearchGoodsList());
        

    }
}