namespace BarParty.Views;
using BarParty.Models;
using System.Collections.ObjectModel;

public partial class coctailsNew : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int _editID;
    private List<Cups> opcionesGlass=new List<Cups>();
    private int cupSelect;
    private List<Methods> opcionesMeth=new List<Methods>();
    private int methSelect;
    private int punt = 0;
    private string preptext;
    private string tagstext="";
    private List<IngXRec> inglist=new List<IngXRec>();
    private List<TagXRec> taglist=new List<TagXRec>();
    private string pic="";
    private FileResult picSelect;
    public coctailsNew(LocalDbService dbService, Recipe cc)
    {
        InitializeComponent();
        _localDbService = dbService;
        if (cc != null)
        {
            _editID = cc.Id;
            Nombre.Text = cc.Name;
            cupSelect = cc.Glass;
            methSelect = cc.Method;
            preptext = cc.Prep;
            Note.Text = cc.Note;
            pic = cc.Photo;
            punt = cc.Calif;

        }
        else
        {
            _editID = 0;
        }
        cargarList(cc);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        MessagingCenter.Subscribe<coctailsIngredAdd, List<IngXRec>>(this, "NuevoI", (sender, nuevoLQT) =>
        {
            inglist = nuevoLQT;
        });
        MessagingCenter.Subscribe<coctailsTags, List<TagXRec>>(this, "NuevoT", (sender, nuevoLQT) =>
        {
            taglist = nuevoLQT;
        });
        actualizar();
    }
    void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        preptext = Prep.Text;
    }
    private async void actualizar()
    {
        List<AddIng> lis = new List<AddIng>();

        foreach (IngXRec bar in inglist)
        {
            Ingredients ingr = await _localDbService.GetIngredientById(bar.IngID);
            Sizes s = await _localDbService.GetSizeById(ingr.Unit);
            string ca = bar.Cant == 0 ? null : bar.Cant.ToString("0.00");
            string no = bar.Note == "zwz" ? null : bar.Note;
            if (ca==null&&s.Name!="ICE")
            {
                inglist.Remove(bar);
            }
            else
            {
                AddIng test = new AddIng { RecID = _editID, IngID = bar.IngID, Cant = ca, Unit = s.Name, Note = no, NameIng = ingr.Name };
                lis.Add(test);
            }
        }

        var contactos = new ObservableCollection<AddIng>(lis);
        ListaIngre.ItemsSource = contactos;

        tagstext = "";
        if (taglist.Count != 0)
        {
            foreach (TagXRec hg in taglist)
            {
                Tags tad = await _localDbService.GetTagById(hg.TagID);
                if (tad.Name != "Fav")
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
        }
        tagsSelect.Text = tagstext;
    }
    private async void cargarList(Recipe i)
    {
        List<Cups> ac= await _localDbService.GetCups();
        List<Methods> am= await _localDbService.GetMethods();
        opcionesGlass = ac;
        opcionesMeth = am;

        foreach (var opcion in ac)
        {
            Glass.Items.Add(opcion.Name);
        }
        foreach (var opcion in am)
        {
            Meth.Items.Add(opcion.Name);
        }
            
        if (i != null)
        {
            if (File.Exists(pic))
            {
                var stream = File.OpenRead(pic);
                var imageSource = ImageSource.FromStream(() => stream);
                imageView.Source = imageSource;
            }
            else
            {
                imageView.Source = "unavailable.png";
            }
            //Tipo.SelectedIndex = opcionesTipo.FindIndex(x => x.Id == i.tipo);
            Glass.SelectedIndex = opcionesGlass.FindIndex(x => x.Id == i.Glass);
            Meth.SelectedIndex = opcionesMeth.FindIndex(x => x.Id == i.Method);


            inglist = await _localDbService.GetIngXRecById(_editID);
            taglist=await _localDbService.GetTagXRecById(_editID);
            Prep.Text = preptext;
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

    }
    private void Picker_glass(object sender, EventArgs e)
    {
        var selectedIndex = Glass.SelectedIndex;
        cupSelect = opcionesGlass[selectedIndex].Id;
    }
    private void Picker_meth(object sender, EventArgs e)
    {
        var selectedIndex = Meth.SelectedIndex;
        methSelect = opcionesMeth[selectedIndex].Id;
    }
    private void OnStara(object sender, EventArgs e)
    {
        a1.Text = "★";
        a2.Text = "☆";
        a3.Text = "☆";
        a4.Text = "☆";
        a5.Text = "☆";
        punt = 1;
    }
    private void OnStarb(object sender, EventArgs e)
    {
        a1.Text = "★";
        a2.Text = "★";
        a3.Text = "☆";
        a4.Text = "☆";
        a5.Text = "☆";
        punt = 2;
    }
    private void OnStarc(object sender, EventArgs e)
    {
        a1.Text = "★";
        a2.Text = "★";
        a3.Text = "★";
        a4.Text = "☆";
        a5.Text = "☆";
        punt = 3;
    }
    private void OnStard(object sender, EventArgs e)
    {
        a1.Text = "★";
        a2.Text = "★";
        a3.Text = "★";
        a4.Text = "★";
        a5.Text = "☆";
        punt = 4;
    }
    private void OnStare(object sender, EventArgs e)
    {
        a1.Text = "★";
        a2.Text = "★";
        a3.Text = "★";
        a4.Text = "★";
        a5.Text = "★";
        punt = 5;
    }

    private async void Save(object sender, EventArgs e)
    {
        if (picSelect!=null)
        {
            string directorioLocal = Path.Combine(AppContext.BaseDirectory, "Cimagenes");
            // Copiar la imagen a un directorio local
            string nombreOriginal = Path.GetFileName(picSelect.FullPath);
            string rutaCopiaLocal = Path.Combine(directorioLocal, nombreOriginal);
            if (!File.Exists(rutaCopiaLocal))
            {
                // Si no existe, copiar la imagen al directorio local
                File.Copy(picSelect.FullPath, rutaCopiaLocal);
            }
            pic = rutaCopiaLocal;
        }
        if (_editID == 0)
        {
            //add
            await _localDbService.CreateRecipe(new Models.Recipe
            {
                Name = Nombre.Text,
                Glass = cupSelect,
                Method = methSelect,
                Prep = preptext,
                Note = Note.Text,
                Calif = punt,
                Photo=pic
            });
            List<Recipe> reca = await _localDbService.GetRecipes();
            Recipe actre=reca.Where(x=>x.Name==Nombre.Text&&x.Prep==preptext).First();
            foreach (IngXRec a in inglist)
            {
                a.RecID = actre.Id;
                await _localDbService.CreateIngXRec(a);
            }
            foreach (TagXRec a in taglist)
            {
                a.RecID = actre.Id;
                await _localDbService.CreateTagXRec(a);
            }
        }
        else
        {
            //edit
            await _localDbService.UpdateRecipe(new Models.Recipe
            {
                Id=_editID,
                Name=Nombre.Text,
                Glass=cupSelect,
                Method=methSelect,
                Prep= preptext,
                Note=Note.Text,
                Photo = pic,
                Calif=punt
            });
            List<IngXRec> inbd = await _localDbService.GetIngXRecById(_editID);
            List<TagXRec> tagbd = await _localDbService.GetTagXRecById(_editID);
            foreach (IngXRec item in inbd)
            {
                await _localDbService.DeleteIngXRec(item);
            }
            foreach (TagXRec item in tagbd)
            {
                await _localDbService.DeleteTagXRec(item);
            }
            foreach (IngXRec a in inglist)
            {
                await _localDbService.CreateIngXRec(a);
            }
            foreach (TagXRec a in taglist)
            {
                await _localDbService.CreateTagXRec(a);
            }
            _editID = 0;
        }
        await Navigation.PopAsync();
    }
    private async void addI(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new coctailsIngredAdd(_localDbService, inglist,_editID));
    }
    private async void addT(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new coctailsTags(_localDbService, taglist, _editID));
    }
    private async void OnSelectImageClicked(object sender, EventArgs e)
    {
        string directorioLocal = Path.Combine(AppContext.BaseDirectory, "Cimagenes");

        // Verificar si el directorio existe, si no, créalo
        if (!Directory.Exists(directorioLocal))
        {
            Directory.CreateDirectory(directorioLocal);
        }
        try
        {
            FileResult result = await MediaPicker.PickPhotoAsync();
            if (result != null)
            {
                picSelect = result;
                var stream = await result.OpenReadAsync();
                var imageSource = ImageSource.FromStream(() => stream);
                imageView.Source = imageSource;
            }
        }
        catch (Exception ex)
        {
            // Manejar errores
            Console.WriteLine($"Error al seleccionar la imagen: {ex.Message}");
        }
    }

}