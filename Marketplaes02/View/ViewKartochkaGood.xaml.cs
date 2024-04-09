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
        //viewmodelkorzina viewmodel = new viewmodelkorzina();
        //bindingcontext = viewmodel;
        //viewmodel.load();
    }

    private async void btkotzina(object sender, EventArgs e)
    {
        FChetchik.IsVisible = true;

        // Ждем 5 секунд
        await Task.Delay(3000);

        // Скрываем frame
        FChetchik.IsVisible = false;
    }
}