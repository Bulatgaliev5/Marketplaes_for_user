using Mopups.Pages;
using Mopups.Services;
using Marketplaes02.ViewModel;
using Mopups.PreBaked.Services;

namespace Marketplaes02.View;

public partial class ViewFillterGoods : PopupPage
{
	public ViewFillterGoods(float OtPrice, float DoPrice)
    {
        InitializeComponent();
        BindingContext = new ViewModelFillterGoods( OtPrice,  DoPrice);
    }
    
    private async void Close(object sender, EventArgs e)
    {

        await MopupService.Instance.PopAsync();
    }
}