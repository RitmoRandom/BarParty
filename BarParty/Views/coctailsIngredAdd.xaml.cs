
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BarParty.Models;


namespace BarParty.Views;

public partial class coctailsIngredAdd : ContentPage
{
    private readonly LocalDbService _localDbService;
    private List<Ingredients> added=new List<Ingredients>();
    private List<AddIng> lista = new List<AddIng>();
    private string filtnom;
    private int exId;
    private int filttype;
    private List<IngXRec> final=new List<IngXRec>();
    private List<TypeIng> opcionesUnidad;
    public coctailsIngredAdd(LocalDbService dbService, List<IngXRec> a, int exid)
    {
        InitializeComponent();
        _localDbService = dbService;
        final = a;
        exId = exid;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (final.Count!=0)
        {
            foreach (IngXRec a in final)
            {
                Ingredients i = await _localDbService.GetIngredientById(a.IngID);
                added.Add(i);
            }
            listarReceta();
        }
        listar();
        PkTipo();
    }
    private async void listar()
    {
        List<Ingredients> listRec=new List<Ingredients>();
        int fil = 0;
        if (filtnom!=null)
        {
            fil = 1;
            listRec = await _localDbService.GetIngredientFiltName(filtnom);
        }
        if (filttype!=0 && filttype != 5041)
        {
            fil = 2;
            //filtrar tipos
            if (listRec.Count!=0||filtnom!=null)
            {
                listRec=listRec.Where(x=>x.Type==filttype).ToList();
            }
            else
            {
                listRec = await _localDbService.GetIngredients();
                listRec = listRec.Where(x => x.Type == filttype).ToList();
            }
        }
        if (listRec.Count == 0 && fil==0)
        {
            listRec = await _localDbService.GetIngredients();
        }
        if (added.Count!=0)
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
        List<IngredientsView> prepre= new List<IngredientsView>();
        foreach(Ingredients a in listRec)
        {
            TypeIng ti=await _localDbService.GetTypeIngById(a.Type);
            Sizes ui = await _localDbService.GetSizeById(a.Unit);
            prepre.Add(new IngredientsView { Id =a.Id, Name=a.Name,NType=ti.Name,NUnit=ui.Name});
        }
        var contactos = new ObservableCollection<IngredientsView>(prepre);
        ListaI.ItemsSource = contactos;
    }
    private async void listarReceta()
    {
        lista.Clear();
        foreach (IngXRec a in final)
        {
            Ingredients i = await _localDbService.GetIngredientById(a.IngID);
            Sizes s = await _localDbService.GetSizeById(i.Unit);
            string ca=a.Cant==0?null:a.Cant.ToString("0.00");
            string no = a.Note == "zwz" ? null : a.Note;
            if (s.Name=="ICE")
            {
                //hielo
                lista.Add(new AddIng { RecID = a.RecID, IngID = a.IngID, Cant = "XXXXX", Unit = s.Name, Note = "Autocalc.", NameIng = i.Name });
            }
            else
            {
                lista.Add(new AddIng { RecID = a.RecID, IngID = a.IngID, Cant = ca, Unit = s.Name, Note = no, NameIng = i.Name });
            }
            
        }
        var contactos = new ObservableCollection<AddIng>(lista);
        ListaAd.ItemsSource = contactos;
    }


    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        IngredientsView ctem = (IngredientsView)e.SelectedItem;

        Ingredients pm = await _localDbService.GetIngredientById(ctem.Id);
        added.Add(pm);
        listar();
        //limpiar filtros
        final.Add(new IngXRec { RecID = exId, IngID = ctem.Id, Cant = 0, Note = "zwz" });
        listarReceta();
        limpiarFiltros();
    }
    private void quitar(object sender, EventArgs e)
    {
        ImageButton btnQuit = (ImageButton)sender;

        AddIng elemento = (AddIng)btnQuit.BindingContext;

        IngXRec tmpa= final.Where(x=>x.IngID==elemento.IngID).First();
        Ingredients tempb = added.Where(x => x.Id == elemento.IngID).First();

        final.Remove(tmpa);
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
        tags.SelectedIndex = opcionesUnidad.FindIndex(x=>x.Id==5041);
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
        int error = 0;
        List<IngXRec> nuevofinal = new List<IngXRec>();
        foreach (AddIng x in lista)
        {
            if (x.Cant==null)
            {
                error = 1;
            }
        }
        if (error == 0)
        {
            foreach (AddIng x in lista)
            {
                double cntt;
                if (double.TryParse(x.Cant, out double numero))
                {
                    cntt=numero;
                    
                }
                else
                {
                    cntt = 0;
                }
                nuevofinal.Add(new IngXRec { Cant = cntt, IngID = x.IngID, Note = x.Note, RecID = x.RecID});
            }
            MessagingCenter.Send<coctailsIngredAdd, List<IngXRec>>(this, "NuevoI", nuevofinal);
            
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Rellene todos los campos cantidad.", "OK");
        }
        
    }


}