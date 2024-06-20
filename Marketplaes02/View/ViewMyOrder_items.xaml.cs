using Marketplaes02.Model;
using Marketplaes02.ViewModel;





namespace Marketplaes02.View;

public partial class ViewMyOrder_items : ContentPage
{
    

    public ViewMyOrder_items()
	{
		InitializeComponent();
        BindingContext = new ViewModelMyOrder_items();
        CheckBtnCopy();



    }
    private async void CheckBtnCopy()
    {
        if (BindingContext is MyOrders g)
        {
            if (g.Track_number != "")
            {
                BtnCopy.IsVisible = true;
                return;
            }
            BtnCopy.IsVisible = false;
            return;
        }
    }


    private async void ClickCopy_Track_number(object sender, EventArgs e)
    {

        if (BindingContext is MyOrders g)
        {
            if (g.Track_number!= "")
            {
                await Clipboard.SetTextAsync(g.Track_number);
                await DisplayAlert("Уведомление", "Трек номер заказа скопирован в буфер обмена ", "OK");
            }
           
        }
        
            
           
       

      




    }
}