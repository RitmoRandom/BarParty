namespace BarParty.Views;
using BarParty.Models;

public partial class cupsNew : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int _editID;
    private ListView _listView;
    public cupsNew(LocalDbService dbService, Cups cc)
	{
		InitializeComponent();
        _localDbService = dbService;
        if(cc != null)
        {
            _editID = cc.Id;
            Nombre.Text = cc.Name;
        }
    }
    private async void Save(object sender, EventArgs e)
    {
        if (_editID == 0)
        {
            //add
            await _localDbService.CreateCup(new Models.Cups
            {
                Name = Nombre.Text
            });
        }
        else
        {
            //edit
            await _localDbService.UpdateCup(new Models.Cups
            {
                Id = _editID,
                Name = Nombre.Text
            });
            _editID = 0;
        }
        Nombre.Text = string.Empty;
        await Navigation.PopAsync();
    }
}