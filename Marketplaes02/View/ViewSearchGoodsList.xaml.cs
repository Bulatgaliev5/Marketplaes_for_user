using Marketplaes02.Model;
using Marketplaes02.ViewModel;

namespace Marketplaes02.View;

public partial class ViewSearchGoodsList : ContentPage
{
    ViewModelSearchGoodsList viewModelSearchGoodsList = new ViewModelSearchGoodsList();
    public ViewSearchGoodsList()
    {
        InitializeComponent();
        Update();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
        {
            ElementSearchBar.Focus();
            return false; // Возвращает false, чтобы остановить таймер после одного вызова
        });
    }

    public void Update()
    {

        BindingContext = new ViewModelSearchGoodsList();
        // base.OnAppearing();


    }
    private void SearchBarSearch(object sender, TextChangedEventArgs e)
    {
        GoodsData.ItemsSource = viewModelSearchGoodsList.Goods
     .Where(a => a.Name.IndexOf(ElementSearchBar.Text, StringComparison.OrdinalIgnoreCase) >= 0
     || a.Description.IndexOf(ElementSearchBar.Text, StringComparison.OrdinalIgnoreCase) >= 0)
     .ToList();

    }



    private async void RefreshGoodsData(object sender, EventArgs e)
    {
        Update();
        await Task.Delay(1000);
        RefreshView1.IsRefreshing = false;
    }

    private async void OpenKartochkaImage(object sender, TappedEventArgs e)
    {
        if (sender is Image b && b.BindingContext is SearchGoodsList g)
        {

            Preferences.Default.Set("Kartochka_ID_goods", g.ID_goods);
        };
        await Navigation.PushAsync(new ViewKartochkaGood());
        await Navigation.PopModalAsync();
    }

    private async void OpenKartochka(object sender, TappedEventArgs e)
    {
        if (sender is StackLayout b && b.BindingContext is SearchGoodsList g)
        {

            Preferences.Default.Set("Kartochka_ID_goods", g.ID_goods);
        };
        await Navigation.PushAsync(new ViewKartochkaGood());

    }
}