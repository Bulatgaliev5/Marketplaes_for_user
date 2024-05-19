
using CommunityToolkit.Maui.Views;
using Marketplaes02.Model;
using Marketplaes02.ViewModel;


namespace Marketplaes02.View;

public partial class ViewKorzina : ContentPage
{

    ViewModelKorzina viewModelKorzina = new ViewModelKorzina();
    public ViewKorzina()
    {
        Update();
        InitializeComponent();
      
        
    }

    public async void Update()
    {
      
        BindingContext = new ViewModelKorzina();
   

    }
   
    private async void OnDataUpdated(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ViewKorzina), false);
    }


    private async void RefreshGoodsData(object sender, EventArgs e)
    {


         Update();

        RefreshView1.IsRefreshing = false;

    }



    private async void btnpay(object sender, EventArgs e)
    {

        Update();
        var currentPage = Navigation.NavigationStack.LastOrDefault();
        if (!(currentPage is ViewOformlenieZakaza))
        {
            await Navigation.PushAsync(new ViewOformlenieZakaza());

        }
    
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