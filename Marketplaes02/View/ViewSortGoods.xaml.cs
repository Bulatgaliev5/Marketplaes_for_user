using Mopups.Pages;

namespace Marketplaes02.View;

public partial class ViewSortGoods : PopupPage
{
	public ViewSortGoods()
	{
		InitializeComponent();
	}

    private void blahButton_Clicked(object sender, EventArgs e)
    {

    }

    private void PopupPage_BackgroundClicked(object sender, EventArgs e)
    {
        Console.WriteLine(1);
    }
}