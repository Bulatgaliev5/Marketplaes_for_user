using Marketplaes02.Model;
using Marketplaes02.ViewModel;

namespace Marketplaes02.View;

public partial class ViewKartochkaGood : ContentPage
{
    ViewModelKartochkaGood viewmodel = new ViewModelKartochkaGood();
    
    public ViewKartochkaGood()
	{
        BindingContext = new ViewModelKartochkaGood();
        //BindingContext = new ViewModelImagesGoods();
        InitializeComponent();
        LoadCount();
    }

    public async void LoadCount()
    {
        if (BindingContext is KartochkaGood g)
        {
            bool result = await viewmodel.CheckAddKorzinaGood(g.ID_goods);
           
        };
    }

    private void btzakazat(object sender, EventArgs e)
    {
        //viewmodelkorzina viewmodel = new viewmodelkorzina();
        //bindingcontext = viewmodel;
        //viewmodel.load();
    }

    public async void btkotzina(object sender, EventArgs e)
    {

       

        FChetchik.IsVisible = true;

        // Ждем 5 секунд
        await Task.Delay(5000);

        // Скрываем frame
        FChetchik.IsVisible = false;
    }
}