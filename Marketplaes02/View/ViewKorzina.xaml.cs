using CommunityToolkit.Maui.Views;
using Marketplaes02.Model;
using Marketplaes02.ViewModel;


namespace Marketplaes02.View;

public partial class ViewKorzina : ContentPage
{
    ViewModelKorzina viewModelKorzina = new ViewModelKorzina();
    public ViewKorzina()
    {
        InitializeComponent();
        Update();

    }
    public void Update()
    {

        BindingContext = new ViewModelKorzina();



    }


    private async void RefreshGoodsData(object sender, EventArgs e)
    {
        Update();
        await Task.Delay(1000);
        RefreshView1.IsRefreshing = false;
    }

    private void CountGoods(object sender, ValueChangedEventArgs e)
    {




    }

    private async void btnpay(object sender, EventArgs e)
    {
        await Task.Delay(1000);
        viewModelKorzina.Load();
       await Navigation.PushAsync(new ViewOformlenieZakaza());
    }

    private void OnFavoriteSwipeItemInvoked(object sender, EventArgs e)
    {

    }

    private void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {

    }

    private void btnclick(object sender, EventArgs e)
    {
        var popup = new PopupView();

        this.ShowPopup(popup);
    }

    private void SelectionChangedKorzinaGoods(object sender, SelectionChangedEventArgs e)
    {
      
    }

    private async void BtnDeleteGoodKorzina(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Подтвердить действие", "Вы хотите удалить товар из корзины?", "Да", "Нет");
        int UserID = Preferences.Default.Get("UserID", 0);

        if (result)
        {
            if (sender is ImageButton b && b.BindingContext is Korzina g)
            {
              
                await viewModelKorzina.DeleteGoodSQL(g.ID_goods, UserID);
                viewModelKorzina.CleanListData(g.ID_goods);
                await DisplayAlert("Уведомление", "Товар успешно удален ", "OK");
            }
            Update();
        }
    }
}