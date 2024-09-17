using System.Collections.ObjectModel;
using BarParty.Models;

namespace BarParty.Views;

public partial class TypeTag : ContentPage
{
    private readonly LocalDbService _localDbService;
    private ListView _listView;
    public TypeTag(LocalDbService dbService)
    {
        InitializeComponent();
        _localDbService = dbService;

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        listar();
    }
    private async void listar()
    {
        List<TagType> listaContacto = await _localDbService.GetTypeTags();
        TagType a = listaContacto.Where(x => x.Name == "Fav").FirstOrDefault();
        if (a!=null)
        {
            listaContacto.Remove(a);
        }
        var contactos = new ObservableCollection<TagType>(listaContacto);
        Lista.ItemsSource = contactos;
    }
    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        TagType c = (TagType)e.SelectedItem;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Editar", "Eliminar");
        switch (action)
        {
            case "Editar":
                await Navigation.PushAsync(new TypeTagNew(_localDbService, c));
                break;
            case "Eliminar":
                await _localDbService.DeleteTypeTag(c);
                listar();
                break;
        }
    }
    private async void addCup(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TypeTagNew(_localDbService, null));
    }
}