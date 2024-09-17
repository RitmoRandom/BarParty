using System.Collections.ObjectModel;
using BarParty.Models;

namespace BarParty.Views;

public partial class tags : ContentPage
{
    private readonly LocalDbService _localDbService;
    private ListView _listView;
    public tags(LocalDbService dbService)
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
        List<Tags> listaContacto = await _localDbService.GetTags();
        Tags a = listaContacto.Where(x => x.Name == "Fav").FirstOrDefault();
        if (a != null)
        {
            listaContacto.Remove(a);
        }
        var contactos = new ObservableCollection<Tags>(listaContacto);
        Lista.ItemsSource = contactos;
    }
    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Tags c = (Tags)e.SelectedItem;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Editar", "Eliminar");
        switch (action)
        {
            case "Editar":
                await Navigation.PushAsync(new tagsNew(_localDbService, c));
                break;
            case "Eliminar":
                await _localDbService.DeleteTag(c);
                listar();
                break;
        }
    }
    private async void addCup(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new tagsNew(_localDbService, null));
    }
}