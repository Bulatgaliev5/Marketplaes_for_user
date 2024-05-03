using Marketplaes02.Model;
using Marketplaes02.ViewModel;
using Microsoft.Maui.Controls;




namespace Marketplaes02.View;

public partial class ViewMyOrder_items : ContentPage
{
    

    public ViewMyOrder_items()
	{
		InitializeComponent();
        BindingContext = new ViewModelMyOrder_items();


    }

    private void RefreshGoodsData(object sender, EventArgs e)
    {

    }

    private async void ClickCopy_Track_number(object sender, EventArgs e)
    {

        if (sender is ImageButton b && b.BindingContext is MyOrder_items g)
        {

            await Clipboard.SetTextAsync(g.Track_number);
            await DisplayAlert("Уведомление", "Трек номер заказа скопирован в буфер обмена ", "OK");
        }
        
            
           
       

      




    }
}