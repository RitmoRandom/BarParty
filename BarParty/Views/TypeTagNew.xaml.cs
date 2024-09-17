namespace BarParty.Views;
using BarParty.Models;

public partial class TypeTagNew : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int _editID;
    public TypeTagNew(LocalDbService dbService, TagType cc)
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
        int error = 0;
            if (Nombre.Text == "Fav")
            {
                error = 1;
            Nombre.Text = string.Empty;
            await DisplayAlert("Error", "Nombre reservado", "OK");
            }
            else
            {
            if (_editID == 0)
            {
                //add
                await _localDbService.CreateTypeTag(new Models.TagType
                {
                    Name = Nombre.Text
                });
            }
            else
            {
                //edit
                await _localDbService.UpdateTypeTag(new Models.TagType
                {
                    Id = _editID,
                    Name = Nombre.Text
                });
                _editID = 0;
            }
        }
        if (error == 0)
        {
            await Navigation.PopAsync();
        }
    }
}