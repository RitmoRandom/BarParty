using System.Collections.ObjectModel;
using BarParty.Models;
using Microsoft.VisualBasic.FileIO;

namespace BarParty.Views;

public partial class ingredients : ContentPage
{
    private readonly LocalDbService _localDbService;
    private ListView _listView;
    private string filtnom;
    public ingredients(LocalDbService dbService)
    {
        InitializeComponent();
        _localDbService = dbService;
    }
    protected async override void OnAppearing()
    {
        //Write the code of your page here

        base.OnAppearing();
        listar();
    }
    private async void listar()
    {
        List<Ingredients> listRec = new List<Ingredients>();
        int fil = 0;
        if (filtnom != null)
        {
            fil = 1;
            listRec = await _localDbService.GetIngredientFiltName(filtnom);
        }
        if (listRec.Count == 0 && fil == 0)
        {
            listRec = await _localDbService.GetIngredients();
        }

        List<IngredientsView> listado=new List<IngredientsView>();
        foreach(Ingredients l in listRec)
        {
            TypeIng a= await _localDbService.GetTypeIngById(l.Type);
            Sizes b = await _localDbService.GetSizeById(l.Unit);
            listado.Add(new IngredientsView { Id=l.Id,Name=l.Name,NType=a.Name,NUnit=b.Name});
        }
        var contactos = new ObservableCollection<IngredientsView>(listado);
        ListaI.ItemsSource = contactos;
    }
    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        IngredientsView ctem= (IngredientsView)e.SelectedItem;
        Ingredients c = await _localDbService.GetIngredientById(ctem.Id);
        var action = await DisplayActionSheet("Action", "Cancel", null, "Editar", "Eliminar");
        switch (action)
        {
            case "Editar":
                await Navigation.PushAsync(new ingredientsNew(_localDbService, c));
                break;
            case "Eliminar":
                await _localDbService.DeleteIngredient(c);
                listar();
                break;
        }
    }
    private async void addCup(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ingredientsNew(_localDbService, null));
    }
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        filtnom = e.NewTextValue;
        listar();
    }
}