using System.Collections.ObjectModel;
using BarParty.Models;

namespace BarParty.Views;

public partial class sizes : ContentPage
{
    private readonly LocalDbService _localDbService;
    private ListView _listView;
    public sizes(LocalDbService dbService)
    {
        InitializeComponent();
        _localDbService = dbService;

    }
    protected async override void OnAppearing()
    {
        //Write the code of your page here

        base.OnAppearing();
        //var listaContacto = await _servicioApi.ListarContactos();
        var listaContacto = await _localDbService.GetSizes();
        var contactos = new ObservableCollection<Sizes>(listaContacto);
        Lista.ItemsSource = contactos;

    }
    private async void listar()
    {
        var listaContacto = await _localDbService.GetSizes();
        var contactos = new ObservableCollection<Sizes>(listaContacto);
        Lista.ItemsSource = contactos;
    }
    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Sizes c = (Sizes)e.SelectedItem;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Editar", "Eliminar");
        switch (action)
        {
            case "Editar":
                await Navigation.PushAsync(new sizesNew(_localDbService, c));
                break;
            case "Eliminar":
                await _localDbService.DeleteSize(c);
                listar();
                break;
        }
    }
    private async void addCup(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new sizesNew(_localDbService, null));
    }
}