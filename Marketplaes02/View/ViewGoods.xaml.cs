using Marketplaes02.Model;
using Marketplaes02.ViewModel;

namespace Marketplaes02.View;

public partial class ViewGoods : ContentPage
{
    ViewModel_Goods viewModel_Goods = new ViewModel_Goods();
    public ViewGoods()
    {


        InitializeComponent();
        Update();

    }


    public void Update()
    {

        BindingContext = new ViewModel_Goods();
        // base.OnAppearing();


    }


    private async void OpenKartochka(object sender, TappedEventArgs e)
    {
        if (sender is StackLayout b && b.BindingContext is Goods g)
        {

            Preferences.Default.Set("Kartochka_ID_goods", g.ID_goods);
        };
        await Navigation.PushAsync(new ViewKartochkaGood());
    }

    private async void RefreshGoodsData(object sender, EventArgs e)
    {
        Update();
        await Task.Delay(1000);
        RefreshView1.IsRefreshing = false;
    }

    private async void OpenKartochkaImage(object sender, TappedEventArgs e)
    {
        if (sender is Image b && b.BindingContext is Goods g)
        {

            Preferences.Default.Set("Kartochka_ID_goods", g.ID_goods);
        };
        await Navigation.PushAsync(new ViewKartochkaGood());
    }


}