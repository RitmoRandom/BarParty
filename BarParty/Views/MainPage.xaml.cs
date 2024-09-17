namespace BarParty.Views;
using BarParty.Models;



public partial class MainPage : ContentPage
{
	private readonly LocalDbService _localDbService;
	private int _editID;
    private ListView _listView;

    public MainPage(LocalDbService dbService)
	{
		InitializeComponent();
		_localDbService = dbService;
        //_listView.ItemSelected += ListView_ItemSelected;
        _listView = new ListView
        {
            ItemTemplate = new DataTemplate(() =>
            {
                // Crear celda personalizada según tus necesidades
                var textCell = new TextCell();
                textCell.SetBinding(TextCell.TextProperty, "Nombre");
                textCell.SetBinding(TextCell.DetailProperty, "Capacidad");
                return textCell;
            })
        };
        Lista = _listView;
		listar();
    }
	private void listar()
	{
        List<Cups> a = new List<Cups>();
        Task.Run(async () => a = await _localDbService.GetCups());
        _listView.ItemsSource = a;
    }
    /*private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Manejar la selección del elemento si es necesario
        if (e.SelectedItem != null)
        {
            var selectedCups = (Cups)e.SelectedItem;
            // Realizar acciones con el objeto Cups seleccionado
        }
    }*/
    private async void guardarB(object sender, EventArgs e)
	{
		if (_editID == 0)
		{
			//add
			await _localDbService.CreateCup(new Models.Cups
			{
				Name=nombre.Text
			});
		}
		else
		{
			//edit
			await _localDbService.UpdateCup(new Models.Cups { 
			Id=_editID,
			Name=nombre.Text
			});
			_editID = 0;
		}
		nombre.Text = string.Empty;
		listar();
	}
    private async void VerItem(object sender, ItemTappedEventArgs e)
	{
		var c = (Cups)e.Item;
		var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");
		switch (action)
		{
			case "Edit":
				_editID=c.Id;
				nombre.Text=c.Name;
				break;
			case "Delete":
				//await _localDbService.DeleteCup(c);
				listar();
				break;
		}
	}
    private async void anva(object sender, EventArgs e)
    {
        
    }

}

