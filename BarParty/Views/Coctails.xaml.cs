using BarParty.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BarParty.Views;

public partial class Coctails : ContentPage
{
    private readonly LocalDbService _localDbService;
    private List<Ingredients> LQT=new List<Ingredients>();
    private List<Tags> TQB = new List<Tags>();
    private string filtnom;
    public Coctails()
    {
        InitializeComponent();
        LocalDbService dbService = new LocalDbService();
        _localDbService = dbService;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _localDbService.fav();
        MessagingCenter.Subscribe<coctailsIngredSerch, List<Ingredients>>(this, "NuevoLQT", (sender, nuevoLQT) =>
        {
            LQT = nuevoLQT;
        });
        MessagingCenter.Subscribe<coctailsTagsSerch, List<Tags>>(this, "NuevoTQB", (sender, nuevoTQB) =>
        {
            TQB = nuevoTQB;
        });
        string total="";
        string totalTags = "";
        foreach (Ingredients a in LQT)
        {
            if (total.Equals(""))
            {
                total = a.Name;
            }
            else
            {
                total = total + " - " + a.Name;
            }
        }
        foreach (Tags a in TQB)
        {
            if (totalTags.Equals(""))
            {
                
                totalTags = a.Name == "Fav" ? "❤" : a.Name;
            }
            else
            {
                string s = a.Name == "Fav" ? "❤" : a.Name;
                totalTags = totalTags + " - " + s;
            }
        }
        string ng = total == "" ? "" : ("Ingre: "+total);
        string tg = totalTags == "" ? "" : ("Tags: "+ totalTags);
        string ss = ng+(ng!=""?"\n":"")+tg;
        listaFil.Text = ss;
        listar();
    }
    private async void listar()
    {
        List<Recipe> nrec = new List<Recipe>();
        int fil = 0;
        if (filtnom != null)
        {
            fil = 1;
            nrec = await _localDbService.GetRecipeFiltName(filtnom);
        }
        if (nrec.Count == 0 && fil == 0)
        {
            nrec = await _localDbService.GetRecipes();
        }
        List<Recipe> listingfil = new List<Recipe>();
        List<Recipe> finalRec = new List<Recipe>();
        if (LQT.Count != 0)
        {
            List<int> ta = new List<int>();
            foreach (Ingredients x in LQT)
            {
                ta.Add(x.Id);
            }
            //filtrar ingr parcial : llenar listingfil

            foreach (Recipe b in nrec)
            {
                List<IngXRec> c = await _localDbService.GetIngXRecById(b.Id);
                List<int> d = new List<int>();
                foreach (IngXRec xRec in c)
                {
                    d.Add(xRec.IngID);
                }

                if (ta.All(item => d.Contains(item)))
                {
                    listingfil.Add(b);
                }
            }

        }
        else
        {
            listingfil = nrec;
        }
        if (TQB.Count != 0)
        {
            List<int> ta=new List<int>();
            foreach (Tags x in TQB)
            {
                ta.Add(x.Id);
            }
            //filtrar tags total : llenar finalRec
            foreach (Recipe b in listingfil)
            {
                List<TagXRec> c = await _localDbService.GetTagXRecById(b.Id);
                List<int> d = new List<int>();
                foreach (TagXRec xRec in c)
                {
                    d.Add(xRec.TagID);
                }
                
                if (ta.All(item => d.Contains(item)))
                {
                    finalRec.Add(b);
                }
            }
        }
        else
        {
            finalRec = listingfil;
        }
        var contactos = new ObservableCollection<Recipe>(finalRec);
        ListaI.ItemsSource = contactos;
    }

    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Recipe c = (Recipe)e.SelectedItem;
        await Navigation.PushAsync(new coctailsView(_localDbService, c));
    }
    private async void add(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new coctailsNew(_localDbService, null));
    }
    private async void ingre(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new coctailsIngredSerch(_localDbService, LQT));
    }
    private async void tager(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new coctailsTagsSerch(_localDbService, TQB));
    }
    private async void limp(object sender, EventArgs e)
    {
        LQT = new List<Ingredients>();
        TQB = new List<Tags>();
        listaFil.Text = "...";
        listar();
    }
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        filtnom = e.NewTextValue;
        listar();
    }


}