using Marketplaes02.Model;
using Marketplaes02.ViewModel;
using Microsoft.Maui.Controls;

namespace Marketplaes02.View;

public partial class ViewGoods : ContentPage
{
    ViewModel_Goods viewModel_Goods = new ViewModel_Goods();
    public ViewGoods()
    {

        Update();
        InitializeComponent();
        

    }


    public async void Update()
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

    private async void Add_isbrannoe(object sender, TappedEventArgs e)
    {
        var ID_user = Preferences.Default.Get("UserID", 0);
        if (sender is Image b && b.BindingContext is Goods g)
        {
     
            if (sender is Image image)
            {
                if (image.Source is FileImageSource fileImageSource)
                {
                    if (fileImageSource.File=="isbrannoe_true.png")
                     {
                           await viewModel_Goods.SQLImageIsbrannoeDelete(ID_user, g.ID_goods);
                           fileImageSource.File = "isbrannoe.png";
                     }
                    else
                    {
                        await viewModel_Goods.AddSQLImageIsbrannoe(ID_user, g.ID_goods);
                        fileImageSource.File = "isbrannoe_true.png";
                    }
                }
            }
        };




    }
}