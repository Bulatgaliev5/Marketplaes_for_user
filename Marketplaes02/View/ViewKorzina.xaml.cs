using Marketplaes02.ViewModel;

namespace Marketplaes02.View;

public partial class ViewKorzina : ContentPage
{
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
}