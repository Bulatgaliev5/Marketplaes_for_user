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
       // await Task.Delay(1000);
        RefreshView1.IsRefreshing = false;
       
    }

   
    private async void SelectOrder(object sender, TappedEventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is All_MyOrder all_MyOrderListt)
        {

            await AnimateButton(grid);

            Preferences.Default.Set("ID_order", all_MyOrderListt.Order.ID_order);
            Preferences.Default.Set("Total_Order_Price_with_discount", all_MyOrderListt.Order.Total_Price_with_discount);
            Preferences.Default.Set("Track_number", all_MyOrderListt.Order.Track_number);
            Preferences.Default.Set("Status", all_MyOrderListt.Order.Status);

        }

        await Navigation.PushAsync(new ViewMyOrder_items());
    }

    private async Task AnimateButton(Grid grid)
    {
        await grid.ScaleTo(0.9, 100);
        await grid.FadeTo(0.7, 100);
        await grid.ScaleTo(1, 100);
        await grid.FadeTo(1, 100);
    }
}