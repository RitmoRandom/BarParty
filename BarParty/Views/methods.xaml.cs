using System.Collections.ObjectModel;
using BarParty.Models;

namespace BarParty.Views;

public partial class methods : ContentPage
{
    private readonly LocalDbService _localDbService;
    private ListView _listView;
    public methods(LocalDbService dbService)
    {
        InitializeComponent();
        _localDbService = dbService;

    }
    protected async override void OnAppearing()
    {
        //Write the code of your page here

        base.OnAppearing();
        //var listaContacto = await _servicioApi.ListarContactos();
        var listaContacto = await _localDbService.GetMethods();
        var contactos = new ObservableCollection<Methods>(listaContacto);
        Lista.ItemsSource = contactos;

    }
    private async void listar()
    {
        var listaContacto = await _localDbService.GetMethods();
        var contactos = new ObservableCollection<Methods>(listaContacto);
        Lista.ItemsSource = contactos;
    }
    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Methods c = (Methods)e.SelectedItem;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Editar", "Eliminar");
        switch (action)
        {
            case "Editar":
                await Navigation.PushAsync(new methodsNew(_localDbService, c));
                break;
            case "Eliminar":
                await _localDbService.DeleteMethod(c);
                listar();
                break;
        }
    }
    private async void addCup(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new methodsNew(_localDbService, null));
    }
}