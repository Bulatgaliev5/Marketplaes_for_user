using Marketplaes02.BD;
using Marketplaes02.ViewModel;
using Mopups.PreBaked.Services;
using MySqlConnector;
using System.Linq;

namespace Marketplaes02.View;

public partial class ViewUser_autorizasiya : ContentPage
{
    ViewModelUser viewModelUser = new ViewModelUser();
    public string
           UserName, UserNumber_phone, UserAdres_Dostavki;
    int UserID;
    public ViewUser_autorizasiya()
    {
        InitializeComponent();


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
        await Navigation.PopAsync();
        var currentPage = Navigation.NavigationStack.LastOrDefault();
        if (!(currentPage is ViewUser_autorizasiya))
        {
            await Navigation.PushAsync(new ViewUser_Registration());

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
            //  Role_name = Convert.ToString(readed["RoleName"]);
            // Role_id = Convert.ToInt32(readed["RoleID"]);
            // ID_User = Convert.ToInt32(readed["UserID"]);
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

        bool stateinternet = CheckInternet();
        //List<string> message = ["Выполняется вход"];
        //await PreBakedMopupService.GetInstance().WrapTaskInLoader(Task.Delay(3000), Color.FromRgb(0, 127, 255), Color.FromRgb(255, 255, 250), message, Color.FromRgb(0, 0, 0));

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
}