using Marketplaes02.ViewModel;

namespace Marketplaes02.View;

public partial class ViewKartochkaGood : ContentPage
{
	public ViewKartochkaGood()
	{
        BindingContext = new ViewModelKartochkaGood();
        //BindingContext = new ViewModelImagesGoods();
        InitializeComponent();
       
    }

    private void btzakazat(object sender, EventArgs e)
    {

    }
}