using Marketplaes02.ViewModel;

namespace Marketplaes02.View;

public partial class ViewGoods : ContentPage
{
	public ViewGoods()
	{

        Update();
        InitializeComponent();
        
    }

    public void Update()
    {

        BindingContext = new ViewModel_Goods();
        // base.OnAppearing();


    }

    private void OpenKartochka(object sender, TappedEventArgs e)
    {

    }

    private void RefreshGoodsData(object sender, EventArgs e)
    {

    }
}