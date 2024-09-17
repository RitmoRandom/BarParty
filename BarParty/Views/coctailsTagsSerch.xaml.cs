namespace BarParty.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BarParty.Models;
public partial class coctailsTagsSerch : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int flagEdit = 0;
    private string filtnom;
    private int filttype;
    private List<TagType> opcionesUnidad;
    private List<Tags> tags=new List<Tags>();
    public coctailsTagsSerch(LocalDbService dbService, List<Tags> a)
    {
        InitializeComponent();
        _localDbService = dbService;
        tags = a;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Verescogidos();
        PkTipo();
        listar();
    }
    private void Verescogidos()
    {
        string total = "";
        if (tags.Count != 0)
        {
            foreach (Tags a in tags)
            {
                
                if (total.Equals(""))
                {
                    total = a.Name=="Fav"? "❤":a.Name;
                }
                else
                {
                    string s = a.Name == "Fav" ? "❤" : a.Name;
                    total = total + " - " + s;
                }
            }

        }
        tagsSelect.Text = total;
    }
    private async void listar()
    {
        List<Tags> list = new List<Tags>();
        int fil = 0;
        if (filtnom != null)
        {
            fil = 1;
            list = await _localDbService.GetTagByName(filtnom);
        }
        if (filttype != 0 && filttype != 5041)
        {
            fil = 2;
            //filtrar tipos
            if (list.Count != 0 || filtnom != null)
            {
                list = list.Where(x => x.tipo == filttype).ToList();
            }
            else
            {
                list = await _localDbService.GetTags();
                list = list.Where(x => x.tipo == filttype).ToList();
            }
        }
        if (list.Count == 0 && fil == 0)
        {
            list = await _localDbService.GetTags();
        }
        if (tags.Count != 0)
        {
            foreach (Tags a in tags)
            {
                Tags p = list.Where(x => x.Id == a.Id).FirstOrDefault();
                if (p != null)
                {
                    list.Remove(p);
                }
            }
        }
        Tags felim = list.Where(x => x.Name == "Fav" || x.Name == "❤").FirstOrDefault();
        if (felim != null)
        {
            Tags ag = new Tags { Id = felim.Id, Name = "❤", tipo = felim.tipo };
            list.Remove(felim);
            list.Add(ag);
        }
        var contactos = new ObservableCollection<Tags>(list);
        ListaTag.ItemsSource = null;
        ListaTag.ItemsSource = contactos;
    }

    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Tags ctem = (Tags)e.SelectedItem;
        if (flagEdit == 0)
        {
            //add
            Tags ab=await _localDbService.GetTagById(ctem.Id);
            tags.Add(ab);
            listar();
            limpiarFiltros();
        }
        else
        {
            //quit
            Tags d = tags.Where(x => x.Id == ctem.Id).First();
            if (d!=null)
            {
                tags.Remove(d);
            }
            listselect();
        }
        Verescogidos();
        
    }
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        filtnom = e.NewTextValue;
        listar();
    }
    private void limpiarFiltros()
    {
        //nombre
        nomTag.Text = null;
        filtnom = null;
        typeTag.SelectedIndex = opcionesUnidad.FindIndex(x => x.Id == 5041);
        filttype = 0;
    }
    private void Picker_Tipo(object sender, EventArgs e)
    {
        var selectedIndex = typeTag.SelectedIndex;
        filttype = opcionesUnidad[selectedIndex].Id;
        listar();
    }
    private async void PkTipo()
    {
        List<TagType> b = await _localDbService.GetTypeTags();
        TagType felim = b.Where(x => x.Name == "Fav" || x.Name == "❤").First();
        felim.Name = "❤";
        opcionesUnidad = b;
        foreach (var opcion in b)
        {
            typeTag.Items.Add(opcion.Name);
        }
        typeTag.Items.Add("---");
        opcionesUnidad.Add(new TagType { Name = "---", Id = 5041 });
    }
    private async void edita(object sender, EventArgs e)
    {
        
        listselect();
        flagEdit = 1;
        nomTag.Text = "ELIMINANDO TAGS SELECCIONADOS";
        typeTag.SelectedIndex = opcionesUnidad.FindIndex(x => x.Id == 5041);
        filtnom = null;
        filttype = 0;
        nomTag.IsReadOnly = true;
        typeTag.IsEnabled = false;
    }
    private async void listselect()
    {
        List<Tags> list = new List<Tags>();
        if (tags.Count != 0)
        {
            foreach (Tags a in tags)
            {
                list.Add(a);
            }
        }
        Tags felim = list.Where(x => x.Name == "Fav" || x.Name == "❤").FirstOrDefault();
        if (felim!=null)
        {
            Tags ag=new Tags { Id=felim.Id,Name= "❤", tipo=felim.tipo};
            list.Remove(felim);
            list.Add(ag);
        }
        var contactos = new ObservableCollection<Tags>(list);
        ListaTag.ItemsSource = null;
        ListaTag.ItemsSource = contactos;
    }
    private async void add(object sender, EventArgs e)
    {
        if (flagEdit == 1)
        {
            //done remove
            flagEdit = 0;
            filttype = 0;
            nomTag.Text = null;
            typeTag.SelectedIndex = opcionesUnidad.FindIndex(x => x.Id == 5041);
            typeTag.IsEnabled = true;
            nomTag.IsReadOnly = false;
            listar();
        }
        else
        {
            //save
            MessagingCenter.Send<coctailsTagsSerch, List<Tags>>(this, "NuevoTQB", tags);
            await Navigation.PopAsync();
        }
    }
}