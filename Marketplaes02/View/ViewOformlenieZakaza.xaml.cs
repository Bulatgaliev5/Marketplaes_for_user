using Marketplaes02.BD;
using Marketplaes02.ViewModel;

namespace Marketplaes02.View;

public partial class ViewOformlenieZakaza : ContentPage
{
    int Count;
    Yoomoney yoomoney = new Yoomoney();
    public ViewOformlenieZakaza()
    {
        InitializeComponent();
        BindingContext = new VewModelSostavZakaza();

    }

    private void BtnZakazat(object sender, EventArgs e)
    {

        yoomoney.LoadData();



    }

    private void Btnyomt(object sender, EventArgs e)
    {
        yoomoney.LoadDataAccaunt();
        //  lab21.Text = Convert.ToString(yoomoney.accountInfo);
    }
}