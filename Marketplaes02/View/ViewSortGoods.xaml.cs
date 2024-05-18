using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.Model;
using Marketplaes02.ViewModel;
using Mopups.Pages;
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

    private async void Close(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}