using Marketplaes02.Model;
using Marketplaes02.ViewModel;
using Mopups.PreBaked.PopupPages.SingleResponse;
using Mopups.Services;


namespace Marketplaes02.View;

public partial class ViewGoodsKategoriya : ContentPage
{
    ViewModelGoodsKategoriya viewModelGoodsKategoriya = new ViewModelGoodsKategoriya();
    public ViewGoodsKategoriya()
    {
        Update();

        InitializeComponent();
    }
    public void Update()
    {

        BindingContext = new ViewModelGoodsKategoriya();
        // base.OnAppearing();


    }
    private async void RefreshGoodsData(object sender, EventArgs e)
    {
        Update();
        await Task.Delay(1000);
        RefreshView1.IsRefreshing = false;
    }

    private void SearchBarSearch(object sender, TextChangedEventArgs e)
    {
        GoodsData.ItemsSource = viewModelGoodsKategoriya.GoodsKategoriyalist
     .Where(a => a.Name.IndexOf(ElementSearchBar.Text, StringComparison.OrdinalIgnoreCase) >= 0
     || a.Description.IndexOf(ElementSearchBar.Text, StringComparison.OrdinalIgnoreCase) >= 0)
     .ToList();

    }

    private async void OpenKartochka(object sender, TappedEventArgs e)
    {
        if (sender is StackLayout b && b.BindingContext is GoodsKategoriya g)
        {

            Preferences.Default.Set("Kartochka_ID_goods", g.ID_goods);
        };
        await Navigation.PushAsync(new ViewKartochkaGood());
    }

    private async void OpenKartochkaImage(object sender, TappedEventArgs e)
    {
        if (sender is Image b && b.BindingContext is GoodsKategoriya g)
        {

            Preferences.Default.Set("Kartochka_ID_goods", g.ID_goods);
        };
        await Navigation.PushAsync(new ViewKartochkaGood());
    }

    private async void Add_isbrannoe(object sender, TappedEventArgs e)
    {
        var ID_user = Preferences.Default.Get("UserID", 0);
        if (sender is Image b && b.BindingContext is GoodsKategoriya g)
        {

            if (sender is Image image)
            {
                if (image.Source is FileImageSource fileImageSource)
                {
                    if (fileImageSource.File == "isbrannoe_true.png")
                    {
                        await viewModelGoodsKategoriya.SQLImageIsbrannoeDelete(ID_user, g.ID_goods);
                        fileImageSource.File = "isbrannoe.png";
                    }
                    else
                    {
                        await viewModelGoodsKategoriya.AddSQLImageIsbrannoe(ID_user, g.ID_goods);
                        fileImageSource.File = "isbrannoe_true.png";
                    }
                }
            }
        };
    }

    private async void ClickOpenFilter(object sender, TappedEventArgs e)
    {
         await SingleResponseViewModel.AutoGenerateBasicPopup(Color.FromRgb(255, 105, 180), Color.FromRgb(255, 105, 255), "I Accept", Color.FromRgb(135, 105, 180), "Good Job, enjoy this single response example", "thumbsup.png");
    }

    private async void ClickOpenSort(object sender, TappedEventArgs e)
    {
        await MopupService.Instance.PushAsync(new ViewSortGoods());
    }
}