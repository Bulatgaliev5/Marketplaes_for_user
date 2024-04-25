using CommunityToolkit.Maui.Views;
using Dadata;
using Dadata.Model;
using Marketplaes02.BD;
using MySqlConnector;
using Newtonsoft.Json.Linq;
namespace Marketplaes02.View;

public partial class ViewAddAdres_dostavki : Popup
{
    SuggestResponse<Address> result;
    bool ValidBool=false;
    public ViewAddAdres_dostavki()
	{
		InitializeComponent();
	}



    private async void addressEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        // https://github.com/hflabs/dadata-csharp

        var token = "d876ce5d68d7b3b55246dc43af0a75d2aeb7e8cd";
        var api = new SuggestClientAsync(token);
         result = await api.SuggestAddress(addressEntry.Text);

        IList<string> addresses = new List<string>();
        for (int i = 0; i < result.suggestions.Count; i++)
        {

            addresses.Add(result.suggestions[i].value);
        }

        addressDataList.ItemsSource = addresses.ToList();
    }

    private async void SelectAdres(object sender, SelectedItemChangedEventArgs e)
    {
        addressEntry.Text = e.SelectedItem.ToString();
        var selectedAddress = result.suggestions.FirstOrDefault(s => s.value == e.SelectedItem.ToString());
        if (selectedAddress != null && !string.IsNullOrEmpty(selectedAddress.data.flat))
        {
            ValidBool = true;
        }
        else
        {
            ValidBool = false;
        }
    }

    public async Task<bool> AddAdres_dostavki()
    {
       int UserID = Preferences.Default.Get("UserID", 0);

        ConnectBD con = new ConnectBD();
      
        if (!ValidBool)
        {
            await con.GetCloseBD();
            await Application.Current.MainPage.DisplayAlert("Сообщение", "Введите полный адрес доставки", "Ок");
            return false;

        }

        string sql = "UPDATE users SET Adres_Dostavki = @adres_dostavki WHERE ID = @ID_Users";
        MySqlCommand cmd = new MySqlCommand(sql, con.GetConnBD());
        cmd.Parameters.Add(new MySqlParameter("@adres_dostavki", addressEntry.Text));
        cmd.Parameters.Add(new MySqlParameter("@ID_Users", UserID));
        await con.GetConnectBD();
        cmd.ExecuteNonQuery();
        Preferences.Default.Set("UserAdres_Dostavki", addressEntry.Text);
        await con.GetCloseBD();
        return true;
    }

    private async void Save(object sender, EventArgs e)
    {
       bool result = await AddAdres_dostavki();
        if (result)
        {
            MessagingCenter.Send<Popup>(this, "ViewOformlenieZakaza");
            await Application.Current.MainPage.DisplayAlert("Уведомление", "Данные успешно сохранены", "Ок");

            await CloseAsync();
        }

    }

}