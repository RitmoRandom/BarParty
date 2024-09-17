using BarParty.Models;
using System.Collections.ObjectModel;

namespace BarParty.Views;

public partial class Partys : ContentPage
{
    private readonly LocalDbService _localDbService;
    public Partys()
    {
        InitializeComponent();
        LocalDbService dbService = new LocalDbService();
        _localDbService = dbService;
    }
    protected override void OnAppearing()
    {
        listar();
    }
    private async void listar()
    {
        var listRec=await _localDbService.GetPartys();
        var contactos = new ObservableCollection<Party>(listRec);
        ListaI.ItemsSource = contactos;
    }

    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Party c = (Party)e.SelectedItem;
        await Navigation.PushAsync(new PartysView(_localDbService, c));
    }
    private async void add(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PartysNew(_localDbService, null));
    }

}