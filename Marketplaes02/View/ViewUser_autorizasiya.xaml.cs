using Marketplaes02.BD;
using Marketplaes02.ViewModel;
using Mopups.PreBaked.Services;
using MySqlConnector;


namespace Marketplaes02.View;

public partial class ViewUser_autorizasiya : ContentPage
{
    public string
           UserName, UserNumber_phone, UserAdres_Dostavki;
    int UserID;
    public ViewUser_autorizasiya()
    {
        InitializeComponent();


    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await RequestStoragePermission();
    }

    public async Task RequestStoragePermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.StorageWrite>();
        }

        if (status == PermissionStatus.Granted)
        {
            // Разрешение предоставлено
        }
        else
        {
            // Разрешение не предоставлено
        }
    }

    public bool CheckInternet()
    {
        var current = Connectivity.NetworkAccess;

        if (current != NetworkAccess.Internet)
        {
            DisplayAlert("Уведомление", "Ошибка сети. Пожалуйста, подключитесь к интернету", "Ок");
            return false;
        }
        return true;
    }

    private async void Registration(object sender, EventArgs e)
    {
        BtnRegistr.IsEnabled = false;
        try 
        {
            await Navigation.PopAsync();
            await Navigation.PushAsync(new ViewUser_Registration());
        }
        finally
        {
            // Разблокировка кнопки
            BtnRegistr.IsEnabled = true;
        }

    }

    public async Task<bool> CheckPass()
    {

        ConnectBD con = new ConnectBD();

        string sql = "SELECT * FROM users WHERE Login = @login AND Pass = @pass";

        MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
        cmd.Parameters.Add(new MySqlParameter("@login", entryLogin.Text));
        cmd.Parameters.Add(new MySqlParameter("@pass", entryPass.Text));

        await con.GetConnectBD();

        MySqlDataReader readed = await cmd.ExecuteReaderAsync();
     
        if (!readed.HasRows)
        {
            await con.GetCloseBD();
            await DisplayAlert("Ошибка", "Не правильный логин или пароль", "Ок");
            return false;
        }
        while (await readed.ReadAsync())
        {
            UserName = Convert.ToString(readed["Name"]);
            UserID = Convert.ToInt32(readed["ID"]);
            UserNumber_phone = Convert.ToString(readed["Number_phone"]);
            UserAdres_Dostavki = Convert.ToString(readed["Adres_Dostavki"]);
            Preferences.Default.Set("UserName", UserName);
            Preferences.Default.Set("UserID", UserID);
            Preferences.Default.Set("UserNumber_phone", UserNumber_phone);
            Preferences.Default.Set("UserAdres_Dostavki", UserAdres_Dostavki);

            break;

        }
        await Task.Run(() =>
        {
            List<string> message = ["Выполняется вход..."];
            PreBakedMopupService.GetInstance().WrapTaskInLoader(Task.Delay(1000), Color.FromRgb(0, 127, 255), Color.FromRgb(255, 255, 250), message, Color.FromRgb(0, 0, 0));
        });


        await con.GetCloseBD();

        return true;
    }

    public async void Voiti(object sender, EventArgs e)
    {
        BtnVoiti.IsEnabled = false;
        try
        {
            bool stateinternet = CheckInternet();

            if (stateinternet)
            {

                bool state = await CheckPass();

                if (state)
                {
                    Navigation.RemovePage(this);

                    await Shell.Current.GoToAsync("//glavnaya");

                }
                return;
            }
        }
        finally
        {
            // Разблокировка кнопки
            BtnVoiti.IsEnabled = true;
        }

    }
}



