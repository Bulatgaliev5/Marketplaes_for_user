using Marketplaes02.Model;
using Marketplaes02.ViewModel;
using System.Timers;

namespace Marketplaes02.View;

public partial class ViewKartochkaGood : ContentPage
{
    ViewModelKartochkaGood viewmodel = new ViewModelKartochkaGood();
    private System.Timers.Timer _timer;
    public ViewKartochkaGood()
	{
        BindingContext = new ViewModelKartochkaGood();
        //BindingContext = new ViewModelImagesGoods();
        InitializeComponent();
        _timer = new System.Timers.Timer();
        _timer.Interval = 5000; // 5 seconds of inactivity
        _timer.Elapsed += OnTimerElapsed;
    }

    public void OnUserInteraction()
    {
        // Reset the timer whenever the user interacts
        _timer.Stop();
        _timer.Start();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            FChetchik.IsVisible = false;
        });
    }

    private void btzakazat(object sender, EventArgs e)
    {
        //viewmodelkorzina viewmodel = new viewmodelkorzina();
        //bindingcontext = viewmodel;
        //viewmodel.load();
    }

    public void btkotzina(object sender, EventArgs e)
    {


        FChetchik.IsVisible = true;
        OnUserInteraction(); // Start or reset the timer
    }

    private void btnminus(object sender, EventArgs e)
    {
        OnUserInteraction();
    }

    private void btnplus(object sender, EventArgs e)
    {
        OnUserInteraction();

    }
}