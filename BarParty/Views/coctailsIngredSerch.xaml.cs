namespace BarParty.Views;
using BarParty.Models;
using System.Collections.ObjectModel;

public partial class coctailsIngredSerch : ContentPage
{
    private readonly LocalDbService _localDbService;
    private List<Ingredients> added = new List<Ingredients>();
    private List<IngXRec> ixr;
    private string filtnom;
    private int filttype;
    private List<TypeIng> opcionesUnidad;
    public coctailsIngredSerch(LocalDbService dbService, List<Ingredients> a)
    {
        InitializeComponent();
        _localDbService = dbService;
        added = a;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        listar();
        if (added.Count!=0)
        {
            listarReceta();
        }
        PkTipo();
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
        if (filttype != 0 && filttype != 5041)
        {
            fil = 2;
            //filtrar tipos
            if (listRec.Count != 0 || filtnom != null)
            {
                listRec = listRec.Where(x => x.Type == filttype).ToList();
            }
            else
            {
                listRec = await _localDbService.GetIngredients();
                listRec = listRec.Where(x => x.Type == filttype).ToList();
            }
        }
        if (listRec.Count == 0 && fil == 0)
        {
            listRec = await _localDbService.GetIngredients();
        }
        if (added.Count != 0)
        {
            foreach (Ingredients a in added)
            {
                Ingredients p = listRec.Where(x => x.Id == a.Id).FirstOrDefault();
                if (p != null)
                {
                    listRec.Remove(p);
                }
            }
        }
        var contactos = new ObservableCollection<Ingredients>(listRec);
        ListaI.ItemsSource = contactos;
    }
    private void listarReceta()
    {
        var contactos = new ObservableCollection<Ingredients>(added);
        ListaAd.ItemsSource = contactos;
    }


    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Ingredients ctem = (Ingredients)e.SelectedItem;
        Ingredients pm = await _localDbService.GetIngredientById(ctem.Id);
        added.Add(pm);
        listar();
        //limpiar filtros
        listarReceta();
        limpiarFiltros();
    }
    private void quitar(object sender, EventArgs e)
    {
        ImageButton btnQuit = (ImageButton)sender;

        Ingredients elemento = (Ingredients)btnQuit.BindingContext;
        Ingredients tempb = added.Where(x => x.Id == elemento.Id).First();
        added.Remove(tempb);
        listarReceta();
        listar();
        limpiarFiltros();
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
        tags.SelectedIndex = opcionesUnidad.FindIndex(x => x.Id == 5041);
        filttype = 0;
    }
    private void Picker_Tipo(object sender, EventArgs e)
    {
        var selectedIndex = tags.SelectedIndex;
        filttype = opcionesUnidad[selectedIndex].Id;
        listar();
    }
    private async void PkTipo()
    {
        List<TypeIng> b = await _localDbService.GetTypeIng();
        opcionesUnidad = b;
        foreach (var opcion in b)
        {
            tags.Items.Add(opcion.Name);
        }
        tags.Items.Add("---");
        opcionesUnidad.Add(new TypeIng { Name = "---", Id = 5041 });
    }
    private async void add(object sender, EventArgs e)
    {
        //limpiarFiltros();
        MessagingCenter.Send<coctailsIngredSerch, List<Ingredients>>(this, "NuevoLQT", added);
        await Navigation.PopAsync();
        
    }
}