using Marketplaes02.BD;
using Marketplaes02.ViewModel;
using MySqlConnector;

namespace Marketplaes02.View;

public partial class ViewUser_autorizasiya : ContentPage
{
    ViewModelUser viewModelUser = new ViewModelUser();
    public string
           UserName;
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
            Preferences.Default.Set("UserName", UserName); // Строковое значение
            //  Role_name = Convert.ToString(readed["RoleName"]);
            // Role_id = Convert.ToInt32(readed["RoleID"]);
            // ID_User = Convert.ToInt32(readed["UserID"]);
            break;

        }


        await con.GetCloseBD();

        return true;
    }

    public async void Voiti(object sender, EventArgs e)
    {

        bool stateinternet = CheckInternet();

        if (stateinternet)
        {
            bool state = await CheckPass();
            if (state)
            {
                Navigation.RemovePage(this);
                //  Navigation.RemovePage(new Okno_avtoriazii());

                await DisplayAlert("Уведомление", "Вы вошли как: ", "Ок");
                await Shell.Current.GoToAsync("//glavnaya");

                // MainPage main = new MainPage(2);
            }
            return;
        }
    }
}