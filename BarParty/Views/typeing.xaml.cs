using System.Collections.ObjectModel;
using BarParty.Models;

namespace BarParty.Views;

public partial class typeing : ContentPage
{
    private readonly LocalDbService _localDbService;
    private ListView _listView;
    public typeing(LocalDbService dbService)
    {
        InitializeComponent();
        _localDbService = dbService;

    }
    protected async override void OnAppearing()
    {
        //Write the code of your page here

        base.OnAppearing();
        //var listaContacto = await _servicioApi.ListarContactos();
        listar();

    }
    private async void listar()
    {
        var listaContacto = await _localDbService.GetTypeIng();
        var contactos = new ObservableCollection<TypeIng>(listaContacto);
        Lista.ItemsSource = contactos;
    }
    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        TypeIng c = (TypeIng)e.SelectedItem;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Editar", "Eliminar");
        switch (action)
        {
            case "Editar":
                await Navigation.PushAsync(new typeingNew(_localDbService, c));
                break;
            case "Eliminar":
                await _localDbService.DeleteTypeIng(c);
                listar();
                break;
        }
    }
    private async void addCup(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new typeingNew(_localDbService, null));
    }
}