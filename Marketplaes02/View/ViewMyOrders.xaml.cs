using Marketplaes02.Model;
using Marketplaes02.ViewModel;

namespace Marketplaes02.View;

public partial class ViewMyOrders : ContentPage
{
	public ViewMyOrders()
	{
        
        InitializeComponent();
        Load();

    }
    public  void Load()
    {
        BindingContext = new ViewModelMyOrders();
    }

    private void BtFilter(object sender, EventArgs e)
    {

    }

    private async void RefreshGoodsData(object sender, EventArgs e)
    {
        Load();
        await Task.Delay(1000);
        RefreshView1.IsRefreshing = false;
       
    }

    private async void SelectOrder(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault() as OrderWithItems;

        Preferences.Default.Set("ID_order", selectedItem.Order.ID_order);
        Preferences.Default.Set("Total_Order_Price_with_discount", selectedItem.Order.Total_Price_with_discount);

        await Navigation.PushAsync(new ViewMyOrder_items());
    }
}