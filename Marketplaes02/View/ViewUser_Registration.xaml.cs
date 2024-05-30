using Marketplaes02.BD;
using Mopups.PreBaked.Services;
using MySqlConnector;

namespace Marketplaes02.View;

public partial class ViewUser_Registration : ContentPage
{
    public ViewUser_Registration()
    {
        InitializeComponent();
    }
    private async Task<bool> CheckPassCopy()
    {
        if (entryPas.Text == entryPasCopy.Text)
        {

            return true;

        }
        await DisplayAlert("Уведомление", "Пароли не совпадают!", "Ок");
        return false;

    }
    private async Task<bool> CheckLogin()
    {
        ConnectBD con = new ConnectBD();

        string sql = "SELECT * FROM users WHERE Login = @login";

        MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
        cmd.Parameters.Add(new MySqlParameter("@login", entryLog.Text));

        await con.GetConnectBD();

        MySqlDataReader readed = await cmd.ExecuteReaderAsync();
        await Task.Run(() =>
        {
            List<string> message = ["Проверка..."];
            PreBakedMopupService.GetInstance().WrapTaskInLoader(Task.Delay(1000), Color.FromRgb(0, 127, 255), Color.FromRgb(255, 255, 250), message, Color.FromRgb(0, 0, 0));
        });
        if (readed.HasRows)
        {
            await con.GetCloseBD();
            await DisplayAlert("Уведомление", "Такой логин уже существует. Придумайте другой логин", "Ок");
            return false;
        }

        await con.GetCloseBD();

        return true;

    }

    private async void BtnRegister(object sender, EventArgs e)
    {


        BtnRegistr.IsEnabled = false;
        try
        {
            bool res = await CheckPassCopy();
            if (res)
            {
                bool result = await CheckLogin();
                if (result)
                {

                    bool res2 = await RegisterProfile();
                    if (res2)
                    {
                        await Navigation.PopAsync();
                    }
                   
                }
            }
        }
        finally
        {
            // Разблокировка кнопки
            BtnRegistr.IsEnabled = true;
        }


    }
    private async Task<bool> RegisterProfile()
    {
        ConnectBD con = new ConnectBD();
        if (string.IsNullOrEmpty(entryName.Text) | string.IsNullOrEmpty(entryLog.Text) 
            | string.IsNullOrEmpty(entryPas.Text) | string.IsNullOrEmpty(entryNumber.Text))
        {
            await con.GetCloseBD();
            await DisplayAlert("Уведомление", "Заполните все поля", "Ок");
            return false;
        }


        string sql = "INSERT INTO `users` (`Login`, `Pass`, `Name`, `Number_phone`) VALUES (@login, @pass, @name, @Number_phone)";
        MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
        cmd.Parameters.Add(new MySqlParameter("@name", entryName.Text));
        cmd.Parameters.Add(new MySqlParameter("@login", entryLog.Text));
        cmd.Parameters.Add(new MySqlParameter("@pass", entryPas.Text));
        cmd.Parameters.Add(new MySqlParameter("@Number_phone", entryNumber.Text));
        await con.GetConnectBD();
        await  cmd.ExecuteNonQueryAsync();
        await con.GetCloseBD();
        await DisplayAlert("Уведомление", "Вы зарегестрировались! Пожалуйста авторизуйтесь", "Ок");

        return true;

    }
}