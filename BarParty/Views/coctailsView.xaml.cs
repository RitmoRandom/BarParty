using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BarParty.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace BarParty.Views;

public partial class coctailsView : ContentPage
{
    
    private readonly LocalDbService _localDbService;
    private int _editID;
    private int punt = 0;
    public coctailsView(LocalDbService dbService, Recipe cc)
    {
        InitializeComponent();
        _localDbService = dbService;
        if (cc != null)
        {
            _editID = cc.Id;
        }

    }
    protected async override void OnAppearing()
    {

        base.OnAppearing();
        if (_editID != 0)
        {
            Recipe cc = await _localDbService.GetRecipeById(_editID);
            Nombre.Text = cc.Name;
            Prep.Text = cc.Prep;
            Note.Text = "Note: " + cc.Note;
            punt = cc.Calif;
            cargarList(cc);
        }
        
    }
    private async void cargarList(Recipe i)
    {
        Cups a = await _localDbService.GetCupById(i.Glass);
        Methods b = await _localDbService.GetMethodById(i.Method);
        string aa=a!=null?a.Name:null;
        string bb = b != null ? b.Name : null;
        Glass.Text ="Vaso: "+aa;
        Meth.Text ="Metodo: "+bb;

        if (File.Exists(i.Photo))
        {
            var stream = File.OpenRead(i.Photo);
            var imageSource = ImageSource.FromStream(() => stream);
            imageView.Source = imageSource;
        }
        else
        {
            imageView.Source = "unavailable.png";
        }

        

        List<AddIng> lis = new List<AddIng>();
        List<IngXRec> ixr = await _localDbService.GetIngXRecById(_editID);

        foreach (IngXRec bar in ixr)
        {
            Ingredients ingr = await _localDbService.GetIngredientById(bar.IngID);
            Sizes s = await _localDbService.GetSizeById(ingr.Unit);
            string ca = bar.Cant == 0 ? null : bar.Cant.ToString("0.00");
            string no = bar.Note == "zwz" ? null : bar.Note;
            AddIng test = new AddIng { RecID = _editID, IngID = bar.IngID, Cant = ca, Unit = s.Name, Note = no, NameIng = ingr.Name };
            lis.Add(test);
        }

        var contactos = new ObservableCollection<AddIng>(lis);
        ListaIngre.ItemsSource = contactos;
        List<TagXRec> txr = await _localDbService.GetTagXRecById(_editID);
        if (txr.Count != 0)
        {
            string tagstext="";
            foreach (TagXRec hg in txr)
            {
                Tags tad = await _localDbService.GetTagById(hg.TagID);
                if (tad.Name=="Fav")
                {
                    fava.Source = "favselect.png";
                }
                else
                {
                    if (tagstext.Equals(""))
                    {
                        tagstext = tad.Name;
                    }
                    else
                    {
                        tagstext = tagstext + " - " + tad.Name;
                    }
                }
            }
            tagsSelect.Text = tagstext;
        }
        switch (punt)
        {
            case 0:
                a1.Text = "☆";
                a2.Text = "☆";
                a3.Text = "☆";
                a4.Text = "☆";
                a5.Text = "☆";
                break;
            case 1:
                a1.Text = "★";
                a2.Text = "☆";
                a3.Text = "☆";
                a4.Text = "☆";
                a5.Text = "☆";
                break;
            case 2:
                a1.Text = "★";
                a2.Text = "★";
                a3.Text = "☆";
                a4.Text = "☆";
                a5.Text = "☆";
                break;
            case 3:
                a1.Text = "★";
                a2.Text = "★";
                a3.Text = "★";
                a4.Text = "☆";
                a5.Text = "☆";
                break;
            case 4:
                a1.Text = "★";
                a2.Text = "★";
                a3.Text = "★";
                a4.Text = "★";
                a5.Text = "☆";
                break;
            case 5:
                a1.Text = "★";
                a2.Text = "★";
                a3.Text = "★";
                a4.Text = "★";
                a5.Text = "★";
                break;
            default: break;
        }
    }
    private async void Editar(object sender, EventArgs e)
    {
        Recipe c = await _localDbService.GetRecipeById(_editID);
        await Navigation.PushAsync(new coctailsNew(_localDbService, c));
        
    }
    private async void Eliminar(object sender, EventArgs e)
    {
        bool flag = false;
        Recipe c = await _localDbService.GetRecipeById(_editID);
        List<Party> parties = await _localDbService.GetPartys();
        foreach (Party item in parties)
        {
            List<CoctelxParty> aa = await _localDbService.GetCoctelxPartyById(item.Id);
            List<Recipe> a = new List<Recipe>();
            foreach (CoctelxParty ccc in aa)
            {
                Recipe recipe = await _localDbService.GetRecipeById(ccc.RecID);
                a.Add(recipe);
            }
            foreach (Recipe a2 in a)
            {
                if (a2.Id == c.Id)
                {
                    flag = true;
                }
            }

        }
        if (flag)
        {
            await DisplayAlert("Error", "Coctel registrado en fiesta activa", "OK");
        }
        else
        {
            List<IngXRec> li = await _localDbService.GetIngXRecById(_editID);
            List<TagXRec> lt = await _localDbService.GetTagXRecById(_editID);
            foreach (IngXRec x in li)
            {
                await _localDbService.DeleteIngXRec(x);
            }
            foreach (TagXRec x in lt)
            {
                await _localDbService.DeleteTagXRec(x);
            }
            await _localDbService.DeleteRecipe(c);
            await Navigation.PopAsync();
        }

    }
    private async void Favo(object sender, EventArgs e)
    {
        List<TagXRec> txr = await _localDbService.GetTagXRecById(_editID);
        List<Tags> ta = await _localDbService.GetTags();
        Tags fa=ta.Where(x=>x.Name=="Fav").First();
        int f = 0;
        if (txr.Count != 0)
        {
            foreach (TagXRec hg in txr)
            {
                Tags tad = await _localDbService.GetTagById(hg.TagID);
                if (tad.Name == "Fav")
                {
                    f = 1;
                }
            }
        }
        if (f==0)
        {
            fava.Source = "favselect.png";
            //añadir tag fav
            await _localDbService.CreateTagXRec(new TagXRec { RecID=_editID,TagID=fa.Id});
        }
        else
        {
            fava.Source = "favorito.png";
            //eliminar tag fav
            if (txr.Count != 0)
            {
                foreach (TagXRec hg in txr)
                {
                    Tags tad = await _localDbService.GetTagById(hg.TagID);
                    if (tad.Name == "Fav")
                    {
                        await _localDbService.DeleteTagXRec(hg);
                    }
                }
            }
            
        }
        
    }
}