namespace BarParty.Views;
using BarParty.Models;

public partial class sizesNew : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int _editID;
    private ListView _listView;
    public sizesNew(LocalDbService dbService, Sizes cc)
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
            await _localDbService.CreateSize(new Models.Sizes
            {
                Name = Nombre.Text
            });
        }
        else
        {
            //edit
            await _localDbService.UpdateSize(new Models.Sizes
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