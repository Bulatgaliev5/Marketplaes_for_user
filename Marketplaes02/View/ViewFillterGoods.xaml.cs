using Mopups.Pages;
using Mopups.Services;
using Marketplaes02.ViewModel;

namespace Marketplaes02.View;

public partial class ViewFillterGoods : PopupPage
{
	public ViewFillterGoods()
    {
        InitializeComponent();
        BindingContext = new ViewModelFillterGoods();
    }

    private async void Close(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}