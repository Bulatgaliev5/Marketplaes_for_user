using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.Model;
using Marketplaes02.ViewModel;
using Mopups.Pages;
using Mopups.PreBaked.Services;
using Mopups.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Marketplaes02.View;

public partial class ViewSortGoods : PopupPage
{
    
    public ViewSortGoods(IList<bool> boolRadioButton)
    {
        InitializeComponent();
        BindingContext = new ViewModelSortGoods(boolRadioButton);
    }

    private void blahButton_Clicked(object sender, EventArgs e)
    {

    }
    public async Task Search()
    {
        List<string> message = ["Поиск.."];
        await PreBakedMopupService.GetInstance().WrapTaskInLoader(Task.Delay(3000), Color.FromRgb(0, 127, 255), Color.FromRgb(255, 255, 250), message, Color.FromRgb(0, 0, 0));
    }
    private async void Close(object sender, EventArgs e)
    {
        await Search();
        await MopupService.Instance.PopAsync();
    }
}