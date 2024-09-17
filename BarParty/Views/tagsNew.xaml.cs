namespace BarParty.Views;
using BarParty.Models;

public partial class tagsNew : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int _editID;
    private List<TagType> opcionesTipo;
    private int typSelect;
    public tagsNew(LocalDbService dbService, Tags cc)
    {
        InitializeComponent();
        _localDbService = dbService;
        if (cc != null)
        {
            _editID = cc.Id;
            Nombre.Text = cc.Name;
            typSelect = cc.tipo;
        }
        cargarList(cc != null ? cc : null);
    }
    private async void cargarList(Tags i)
    {
        List<TagType> b = await _localDbService.GetTypeTags();
        TagType favelim=b.Where(x=>x.Name=="Fav").First();
        b.Remove(favelim);
        opcionesTipo = b;
        foreach (var opcion in b)
        {
            Tipo.Items.Add(opcion.Name);
        }
        if (i != null)
        {
            Tipo.SelectedIndex = opcionesTipo.FindIndex(x => x.Id == i.tipo);
        }

    }
    private void Picker_Tipo(object sender, EventArgs e)
    {
        var selectedIndex = Tipo.SelectedIndex;
        typSelect = opcionesTipo[selectedIndex].Id;
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
                await _localDbService.CreateTag(new Models.Tags
                {
                    Name = Nombre.Text,
                    tipo = typSelect
                });
            }
            else
            {
                //edit
                await _localDbService.UpdateTag(new Models.Tags
                {
                    Id = _editID,
                    Name = Nombre.Text,
                    tipo = typSelect
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