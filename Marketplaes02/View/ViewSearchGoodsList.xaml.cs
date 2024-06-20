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

        Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            ElementSearchBar.Focus();
            return false; // Возвращает false, чтобы остановить таймер после одного вызова
        });
    }

    public void Update()
    {

        BindingContext = new ViewModelSearchGoodsList();



    }




    private async void RefreshGoodsData(object sender, EventArgs e)
    {
        Update();
        RefreshView1.IsRefreshing = false;
    }

    private async void OpenKartochkaImage(object sender, TappedEventArgs e)
    {
        if (sender is Image b && b.BindingContext is SearchGoodsList g)
        {

            Preferences.Default.Set("Kartochka_ID_goods", g.ID_goods);
            var currentPage = Navigation.NavigationStack.LastOrDefault();
            if (!(currentPage is ViewKartochkaGood))
            {
                await Navigation.PushAsync(new ViewKartochkaGood());

            }
        };

     
    }

    private async void OpenKartochka(object sender, TappedEventArgs e)
    {
        if (sender is StackLayout b && b.BindingContext is SearchGoodsList g)
        {

            Preferences.Default.Set("Kartochka_ID_goods", g.ID_goods);
            var currentPage = Navigation.NavigationStack.LastOrDefault();
            if (!(currentPage is ViewKartochkaGood))
            {
                await Navigation.PushAsync(new ViewKartochkaGood());

            }
        };


    }

    private async void Add_isbrannoe(object sender, TappedEventArgs e)
    {
        var ID_user = Preferences.Default.Get("UserID", 0);
        if (sender is Image b && b.BindingContext is SearchGoodsList g)
        {

            if (sender is Image image)
            {
                if (image.Source is FileImageSource fileImageSource)
                {
                    if (fileImageSource.File == "isbrannoe_true.png")
                    {
                        await viewModelSearchGoodsList.SQLImageIsbrannoeDelete(ID_user, g.ID_goods);
                        fileImageSource.File = "isbrannoe.png";
                    }
                    else
                    {
                        await viewModelSearchGoodsList.AddSQLImageIsbrannoe(ID_user, g.ID_goods);
                        fileImageSource.File = "isbrannoe_true.png";
                    }
                }
            }
        };
    }


}