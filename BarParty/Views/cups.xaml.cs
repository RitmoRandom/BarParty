namespace BarParty.Views;
using BarParty.Models;
using System.Collections.ObjectModel;

public partial class cups : ContentPage
{
    private readonly LocalDbService _localDbService;
    private ListView _listView;
    public cups(LocalDbService dbService)
	{
		InitializeComponent();
        _localDbService = dbService;
        
    }
    protected async override void OnAppearing()
    {
        //Write the code of your page here

        base.OnAppearing();
        //var listaContacto = await _servicioApi.ListarContactos();
        var listaContacto = await _localDbService.GetCups();
        var contactos = new ObservableCollection<Cups>(listaContacto);
        Lista.ItemsSource = contactos;

    }
    private async void listar()
    {
        var listaContacto = await _localDbService.GetCups();
        var contactos = new ObservableCollection<Cups>(listaContacto);
        Lista.ItemsSource = contactos;
    }
    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Cups c = (Cups)e.SelectedItem;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Editar", "Eliminar");
        switch (action)
        {
            case "Editar":
                await Navigation.PushAsync(new cupsNew(_localDbService, c));
                break;
            case "Eliminar":
                await _localDbService.DeleteCup(c);
                listar();
                break;
        }
    }
    private async void addCup(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new cupsNew(_localDbService, null));
    }
}