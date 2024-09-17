namespace BarParty.Views;
using BarParty.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public partial class ingredientsNew : ContentPage
{
    private readonly LocalDbService _localDbService;
    private int _editID;
    private List<TypeIng> opciones;
    private int tipoSelect;
    private List<Sizes> opcionesUnidad;
    private int unidadSelect;
    public ingredientsNew(LocalDbService dbService, Ingredients cc)
    {
        InitializeComponent();
        _localDbService = dbService;
        if (cc != null)
        {
            _editID = cc.Id;
            Nombre.Text = cc.Name;
            tipoSelect = cc.Type;
            unidadSelect = cc.Unit;
        }
        cargarList(cc!=null?cc:null);
        
    }
    private async void cargarList(Ingredients i)
    {
        List<TypeIng> a = await _localDbService.GetTypeIng();
        opciones = a;
        foreach (var opcion in a)
        {
            Tipo.Items.Add(opcion.Name.ToString());
        }
        List<Sizes> b = await _localDbService.GetSizes();
        opcionesUnidad = b;
        foreach (var opcion in b)
        {
            Unidad.Items.Add(opcion.Name.ToString());
        }
        if (i != null)
        {
            Tipo.SelectedIndex = opciones.FindIndex(x => x.Id == i.Type);
            Unidad.SelectedIndex = opcionesUnidad.FindIndex(x => x.Id == i.Unit);
        }

    }
    private void Picker_Tipo(object sender, EventArgs e)
    {
            var selectedIndex = Tipo.SelectedIndex;
            tipoSelect = opciones[selectedIndex].Id;
    }
    private void Picker_Unidad(object sender, EventArgs e)
    {
            var selectedIndex = Unidad.SelectedIndex;
            unidadSelect = opcionesUnidad[selectedIndex].Id;
    }
    private async void Save(object sender, EventArgs e)
    {
        int error = 0;
            if (tipoSelect == 0|| unidadSelect == 0)
            {
                error = 1;
            }
        if (error == 0)
        {
            if (_editID == 0)
            {
                //add
                await _localDbService.CreateIngredient(new Models.Ingredients
                {
                    Name = Nombre.Text,
                    Type = tipoSelect,
                    Unit = unidadSelect
                });
            }
            else
            {
                //edit
                await _localDbService.UpdateIngredients(new Models.Ingredients
                {
                    Id = _editID,
                    Name = Nombre.Text,
                    Type = tipoSelect,
                    Unit = unidadSelect
                });
                _editID = 0;
            }
            Nombre.Text = string.Empty;
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Rellene todos los campos.", "OK");
        }

        
    }
}