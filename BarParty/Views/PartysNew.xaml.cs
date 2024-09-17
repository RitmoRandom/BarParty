using BarParty.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BarParty.Views;

public partial class PartysNew : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int _editID;
    private List<Recipe> cocteles = new List<Recipe>();
    public PartysNew(LocalDbService dbService, Party cc)
    {
        InitializeComponent();
        _localDbService = dbService;
        if (cc != null)
        {
            _editID = cc.Id;
            Nombre.Text = cc.Name;
            date.Date = cc.FechaEvento;
            people.Text = cc.people.ToString();
            tragos.Text = cc.drinks.ToString();
            getcocteles();
        }
        else
        {
            _editID = 0;
        }
        actualizar();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        MessagingCenter.Subscribe<PartysSelectRecipe, List<Recipe>>(this, "NuevoR", (sender, nuevoR) =>
        {
            cocteles = nuevoR;
        });
        actualizar();
    }
    private void actualizar()
    {
        var contactos = new ObservableCollection<Recipe>(cocteles);
        ListaIngre.ItemsSource = contactos;
    }
    private async void getcocteles()
    {
        List<CoctelxParty> a = await _localDbService.GetCoctelxPartyById(_editID);
        List<Recipe> b = new List<Recipe>();
        foreach (CoctelxParty c in a)
        {
            Recipe d=await _localDbService.GetRecipeById(c.RecID);
            b.Add(d);
        }
        cocteles = b;
        var contactos = new ObservableCollection<Recipe>(cocteles);
        ListaIngre.ItemsSource = contactos;
    }

    private async void Save(object sender, EventArgs e)
    {
        int error = 0;
        if (people.Text==null||people.Text==""|| tragos.Text == null || tragos.Text == "")
        {
            error = 1;
        }
        if (error == 0)
        {
            if (_editID == 0)
            {
                //add
                await _localDbService.CreateParty(new Party
                {
                    Name = Nombre.Text,
                    FechaEvento = date.Date,
                    people = int.Parse(people.Text),
                    drinks = int.Parse(tragos.Text)
                });

                List<Party> reca = await _localDbService.GetPartys();
                Party actre = reca.Where(x => x.Name == Nombre.Text && x.FechaEvento == date.Date).First();
                foreach (Recipe a in cocteles)
                {
                    await _localDbService.CreateCoctelxParty(new CoctelxParty { PartyID=actre.Id,RecID=a.Id});
                }
            }
            else
            {
                //edit
                await _localDbService.UpdateParty(new Party
                {
                    Id = _editID,
                    Name = Nombre.Text,
                    FechaEvento = date.Date,
                    people = int.Parse(people.Text),
                    drinks = int.Parse(tragos.Text)
                });
                List<CoctelxParty> inbd = await _localDbService.GetCoctelxPartyById(_editID);
                foreach (CoctelxParty item in inbd)
                {
                    await _localDbService.DeleteCoctelxParty(item);
                }
                foreach (Recipe a in cocteles)
                {
                    await _localDbService.CreateCoctelxParty(new CoctelxParty { PartyID = _editID, RecID = a.Id });
                }
            }
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Complete todos los campos", "OK");
        }
    }
    private async void addCocteles(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PartysSelectRecipe(_localDbService, cocteles, _editID));
    }
    private void quitar(object sender, EventArgs e)
    {
        ImageButton btnQuit = (ImageButton)sender;

        Recipe elemento = (Recipe)btnQuit.BindingContext;

        Recipe a=cocteles.Where(x=>x.Id==elemento.Id).First();
        cocteles.Remove(a);
        actualizar();
    }

}