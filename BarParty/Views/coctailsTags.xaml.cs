namespace BarParty.Views;

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BarParty.Models;
using Microsoft.Maui.Controls;
using Microsoft.VisualBasic.FileIO;

public partial class coctailsTags : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int flagEdit = 0;
    private List<TagXRec> final=new List<TagXRec>();
    private string filtnom;
    private int filttype;
    private List<TagType> opcionesUnidad;
    private int exId;
    public coctailsTags(LocalDbService dbService, List<TagXRec> a, int exid)
    {
        InitializeComponent();
        _localDbService = dbService;
        final = a;
        exId = exid;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Verescogidos();
        PkTipo();
        listar();
    }
    private async void Verescogidos()
    {
        string total = "";
        if (final.Count != 0)
        {
            foreach (TagXRec a in final)
            {
                Tags i = await _localDbService.GetTagById(a.TagID);
                if (i.Name != "Fav")
                {
                    if (total.Equals(""))
                    {
                        total = i.Name;
                    }
                    else
                    {
                        total = total + " - " + i.Name;
                    }
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
        if (final.Count != 0)
        {
            foreach (TagXRec a in final)
            {
                Tags i = await _localDbService.GetTagById(a.TagID);
                Tags p = list.Where(x => x.Id == i.Id).FirstOrDefault();
                if (p != null)
                {
                    list.Remove(p);
                }
            }
        }
        Tags fav = list.Where(x => x.Name == "Fav").FirstOrDefault();
        if (fav!=null)
        {
            list.Remove(fav);
        }
        var contactos = new ObservableCollection<Tags>(list);
        ListaTag.ItemsSource = null;
        ListaTag.ItemsSource = contactos;
    }

    private void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Tags ctem = (Tags)e.SelectedItem;
        if (flagEdit==0)
        {
            //add
            final.Add(new TagXRec { RecID = exId, TagID=ctem.Id });
            //limpiar filtro
            listar();
        }
        else
        {
            //quit
            TagXRec d=final.Where(x => x.TagID == ctem.Id).First();
            if (d != null)
            {
                final.Remove(d);
            }
            listselect();
        }
        Verescogidos();
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
        TagType felim=b.Where(x=>x.Name=="Fav").First();
        b.Remove(felim);
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
        nomTag.Text = "ELIMINANDO TAGS";
        typeTag.SelectedIndex = opcionesUnidad.FindIndex(x => x.Id == 5041);
        filtnom =null;
        filttype = 0;
        nomTag.IsReadOnly = true;
        typeTag.IsEnabled = false;
    }
    private async void listselect()
    {
        List<Tags> list = new List<Tags>();
        if (final.Count != 0)
        {
            foreach (TagXRec a in final)
            {
                Tags i = await _localDbService.GetTagById(a.TagID);
                list.Add(i);
            }
        }
        var contactos = new ObservableCollection<Tags>(list);
        ListaTag.ItemsSource = null;
        ListaTag.ItemsSource = contactos;
    }
    private async void add(object sender, EventArgs e)
    {
        if (flagEdit==1)
        {
            //done remove
            flagEdit=0;
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
            MessagingCenter.Send<coctailsTags, List<TagXRec>>(this, "NuevoT", final);
            await Navigation.PopAsync();
        }
    }
}