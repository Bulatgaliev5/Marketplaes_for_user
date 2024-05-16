using Marketplaes02.BD;
using MySqlConnector;

namespace Marketplaes02.View;

public partial class ViewUser_Registration : ContentPage
{
	public ViewUser_Registration()
	{
		InitializeComponent();
	}

	private void BtnRegister(object sender, EventArgs e)
	{

	}
    private async Task<bool> RegisterProfile()
    {
        ConnectBD con = new ConnectBD();
        if (string.IsNullOrEmpty(entryName.Text) | string.IsNullOrEmpty(entryLog.Text) | string.IsNullOrEmpty(entryPas.Text))
        {
            await con.GetCloseBD();
            await DisplayAlert("Ошибка", "Заполните все поля", "Ок");
            return false;
        }


        string sql = "INSERT INTO `users` (`Name`, `Login`, `Pass`, `ID_roli`) VALUES (@name, @login, @pass, 1)";
        MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
        cmd.Parameters.Add(new MySqlParameter("@name", entryName.Text));
        cmd.Parameters.Add(new MySqlParameter("@login", entryLog.Text));
        cmd.Parameters.Add(new MySqlParameter("@pass", entryPas.Text));
        await con.GetConnectBD();
        cmd.ExecuteNonQuery();
        await con.GetCloseBD();
        return true;

    }
}