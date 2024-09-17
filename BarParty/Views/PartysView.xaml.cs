using BarParty.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BarParty.Views;

public partial class PartysView : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int _editID;
    public PartysView(LocalDbService dbService, Party cc)
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
            Party cc = await _localDbService.GetPartyById(_editID);
            Nombre.Text = cc.Name;
            fecha.Text = cc.FechaEvento.ToString("dd-MM-yyyy");
            personas.Text=cc.people.ToString()+ " #Per.";
            tragos.Text=cc.drinks.ToString()+ " #Drinks";
            cargarList(cc);
        }

    }
    private async void cargarList(Party i)
    {
        List<CoctelxParty> aa=await _localDbService.GetCoctelxPartyById(i.Id);
        List<Recipe> a = new List<Recipe>();
        foreach (CoctelxParty c in aa)
        {
            Recipe recipe=await _localDbService.GetRecipeById(c.RecID);
            a.Add(recipe);
        }
        int txc=0;
        int totalDrinks = 0;
        List<Recipe> present=new List<Recipe>();
        foreach (Recipe r in a)
        {
            Recipe b=await _localDbService.GetRecipeById(r.Id);
            present.Add(b);
        }
        var contactos = new ObservableCollection<Recipe>(present);
        ListaIngre.ItemsSource = contactos;
        if (a.Count != 0)
        {
            totalDrinks = i.people * i.drinks;
            while (totalDrinks % (present.Count) != 0)
            {
                totalDrinks++;
            }
            txc = totalDrinks / present.Count;
        }

        //logica de compra
        List<Compras> compras = new List<Compras>();
        int cch = 0;
        foreach (Recipe r in a)
        {
            List<IngXRec> ir=await _localDbService.GetIngXRecById(r.Id);
            foreach (IngXRec x in ir)
            {
                Ingredients ing=await _localDbService.GetIngredientById(x.IngID);
                Sizes sss=await _localDbService.GetSizeById(ing.Unit);
                Compras ca=compras.Where(x=>x.Id==ing.Id).FirstOrDefault();
                double tca = x.Cant * txc;
                int cca= (int)Math.Ceiling(tca);
                if (sss.Name=="ICE")
                {
                    cch = cch + txc;
                }
                else
                {
                    if (ca != null)
                    {
                        //ingrediente ya en lista
                        ca.Cantidad = ca.Cantidad + cca;
                    }
                    else
                    {
                        //ingrediente nuevo
                        compras.Add(new Compras { Id = ing.Id, type = ing.Type, Cantidad = cca });
                    }
                }
                
            }
        }

        //procesar calculo
        if (a.Count != 0)
        {
            
            foreach (Compras cm in compras)
            {
                Ingredients im = await _localDbService.GetIngredientById(cm.Id);
                Sizes s = await _localDbService.GetSizeById(im.Unit);
                int pre;
                int vepre;
                switch (s.Name)
                {
                    case "Oz":
                        double pref = cm.Cantidad * 1.4 * 30;
                        double bot = pref / 750.00;

                        int parteEntera = (int)bot;
                        int decimales = (int)((bot - parteEntera) * 100);
                        int fijo = parteEntera;
                        if (decimales > 30)
                        {
                            fijo++;
                        }
                        if (fijo == 0 && bot > 0)
                        {
                            fijo++;
                        }
                        cm.Calculo = fijo + " botella(s) de " + im.Name + " (" + (int)(pref) + "ml)";
                        
                        break;
                    case "Dash":
                        int resultado = cm.Cantidad / 150;
                        int resto = cm.Cantidad % 150;
                        if (resto<=0)
                        {
                            resto = 0;
                        }else if (resto<=25)
                        {
                            resto = 25;
                        }else if (resto<=50)
                        {
                            resto = 50;
                        }
                        else if (resto<=75)
                        {
                            resto = 75;
                        }
                        else
                        {
                            resultado++;
                        }
                        if (resultado!=0)
                        {
                            if (resto!=0)
                            {
                                cm.Calculo = resultado + " botella(s) y "+ resto + "% de " + im.Name;
                            }
                            else
                            {
                                cm.Calculo = resultado + " botella(s) de " + im.Name;
                            }
                        }
                        else
                        {
                            if (resto != 0)
                            {
                                cm.Calculo = resto + "% de 1 botellas de " + im.Name;
                            }
                            else
                            {
                                cm.Calculo = "0 botellas de " + im.Name;
                            }
                        }
                        break;
                    case "Trozo":
                        pre = cm.Cantidad / 5;

                        if (cm.Cantidad % 5 != 0)
                        {
                            pre++;
                        }
                        cm.Calculo = pre.ToString() + " " + im.Name;
                        break;
                    case "Cucharada":
                        pre = cm.Cantidad * 6;
                        cm.Calculo = pre.ToString() + " Gramos de " + im.Name;
                        break;
                    case "Pizca":
                        cm.Calculo = cm.Cantidad.ToString() + " Gramos de " + im.Name;
                        break;
                    case "Cantidad":
                        cm.Calculo = cm.Cantidad.ToString()+" "+ im.Name;
                        break;
                    case "Capa":
                        double prev= cm.Cantidad * 1.4 * 30;
                        double veprev = prev / 750.00;

                        if (prev % 750 != 0)
                        {
                            veprev++;
                        }
                        cm.Calculo = veprev + " botella(s) de " + im.Name+" ("+(int)(prev) +"ml)";
                        break;
                    case "Completar":
                        pre = cm.Cantidad * 8 * 30;
                        vepre = pre / 1000;

                        if (pre % 1000 != 0)
                        {
                            vepre++;
                        }
                        cm.Calculo = vepre + " Litro(s) de " + im.Name + " (" + pre + "ml)";
                        break;
                    case "ICE":
                        double preice = cch / 3.15;
                        double re = (preice * 100) % 100;
                        if (re>=20)
                        {
                            preice++;
                        }
                        cm.Calculo = (int)(preice) + " Kg de Hielo";
                        break;
                    default: cm.Calculo = cm.Cantidad.ToString() + " " + im.Name + " (SIN PROCESO)"; break;
                }
            }
        }
        

        //mostrar
        string Comprado = "";
        List<Compras> listaOrdenada = compras.OrderBy(objeto => objeto.type).ToList();
        int tipoActual = -1;
        foreach (Compras objeto in listaOrdenada)
        {
            if (objeto.type != tipoActual)
            {
                Ingredients inaa = await _localDbService.GetIngredientById(objeto.Id);
                TypeIng tinaa=await _localDbService.GetTypeIngById(inaa.Type);
                string tt = tinaa.Name;
                Comprado = Comprado + $"------ {tt} ------\n";
                tipoActual = objeto.type;
            }
            Comprado = Comprado + objeto.Calculo+"\n";
        }
        ListaCompras.Text = Comprado;

    }
    private async void Editar(object sender, EventArgs e)
    {
        Party c = await _localDbService.GetPartyById(_editID);
        await Navigation.PushAsync(new PartysNew(_localDbService, c));

    }
    private async void Eliminar(object sender, EventArgs e)
    {
        Party parties = await _localDbService.GetPartyById(_editID);
        var action = await DisplayActionSheet("¿Seguro desea eliminar la fiesta?", "Cancelar", null, "Eliminar");
        switch (action)
        {
            case "Eliminar":
                await _localDbService.DeleteParty(parties);
                break;
        }
        await Navigation.PopAsync();
    }
    private async void VerItem(object sender, SelectedItemChangedEventArgs e)
    {
        Recipe c = (Recipe)e.SelectedItem;
        await Navigation.PushAsync(new coctailsView(_localDbService, c));
    }
}