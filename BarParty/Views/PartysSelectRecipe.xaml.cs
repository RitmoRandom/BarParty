using BarParty.Models;
using System.Collections.ObjectModel;

namespace BarParty.Views;

public partial class PartysSelectRecipe : ContentPage
{
    private readonly LocalDbService _localDbService;
    private string filtnom;
    private int exId;
    private List<Recipe> final = new List<Recipe>();
    public PartysSelectRecipe(LocalDbService dbService, List<Recipe> a, int exid)
    {
        InitializeComponent();
        _localDbService = dbService;
        final = a;
        exId = exid;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ver();
        listar();
    }
    private void ver()
    {
            string total = "";
            foreach (Recipe a in final)
            {
                if (total.Equals(""))
                {
                    total = a.Name;
                }
                else
                {
                    total = total + " - " + a.Name;
                }
            }
            listt.Text = total;
    }
    private async void listar()
    {
        List<Recipe> listRec = new List<Recipe>();
        int fil = 0;
        if (filtnom != null)
        {
            fil = 1;
            listRec = await _localDbService.GetRecipeFiltName(filtnom);
        }
        if (listRec.Count == 0 && fil == 0)
        {
            listRec = await _localDbService.GetRecipes();
        }
        if (final.Count != 0)
        {
            foreach (Recipe a in final)
            {
                Recipe p = listRec.Where(x => x.Id == a.Id).FirstOrDefault();
                if (p != null)
                {
                    listRec.Remove(p);
                }

            }
        }
        var contactos = new ObservableCollection<Recipe>(listRec);
        ListaI.ItemsSource = contactos;
    }

    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Recipe ctem = (Recipe)e.SelectedItem;
        Recipe pm = await _localDbService.GetRecipeById(ctem.Id);
        final.Add(pm);
        ver();
        limpiarFiltros();
        listar();
    }
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        filtnom = e.NewTextValue;
        listar();
    }
    private void limpiarFiltros()
    {
        //nombre
        filt.Text = null;
        filtnom = null;
    }
    private async void add(object sender, EventArgs e)
    {
            MessagingCenter.Send<PartysSelectRecipe, List<Recipe>>(this, "NuevoR", final);
            await Navigation.PopAsync();
    }
}