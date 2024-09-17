using BarParty.Models;

namespace BarParty.Views;

public partial class typeingNew : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int _editID;
    private ListView _listView;
    public typeingNew(LocalDbService dbService, TypeIng cc)
    {
        InitializeComponent();
        _localDbService = dbService;
        if (cc != null)
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
            await _localDbService.CreateTypeIng(new Models.TypeIng
            {
                Name = Nombre.Text
            });
        }
        else
        {
            //edit
            await _localDbService.UpdateTypeIng(new Models.TypeIng
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