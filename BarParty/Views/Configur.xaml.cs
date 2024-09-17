using SQLite;
using System.IO.Compression;
using static SQLite.SQLite3;

namespace BarParty.Views;

public partial class Configur : ContentPage
{
    private readonly LocalDbService _localDbService;
    public Configur()
	{
		InitializeComponent();
        LocalDbService dbService = new LocalDbService();
        _localDbService = dbService;

    }
    public async void Vasos(object sender, EventArgs e) { await Navigation.PushAsync(new cups(_localDbService)); }
    public async void Metodos(object sender, EventArgs e) { await Navigation.PushAsync(new methods(_localDbService)); }
    public async void Ingredientes(object sender, EventArgs e) { await Navigation.PushAsync(new ingredients(_localDbService)); }
    public async void TypeIng(object sender, EventArgs e) { await Navigation.PushAsync(new typeing(_localDbService)); }
    public async void Medidas(object sender, EventArgs e) { await Navigation.PushAsync(new sizes(_localDbService)); }
    public async void Tags(object sender, EventArgs e) { await Navigation.PushAsync(new tags(_localDbService)); }
    public async void TagTy(object sender, EventArgs e) { await Navigation.PushAsync(new TypeTag(_localDbService)); }
    public async void exportar(object sender, EventArgs e) {
        string directorioExportacion = "/storage/emulated/0/Documents";
        string rutaCompletaZip = Path.Combine(directorioExportacion, "BarPartyData.zip");
        using (ZipArchive archive = ZipFile.Open(rutaCompletaZip, ZipArchiveMode.Create))
        {
            // Agregar la base de datos SQLite
            archive.CreateEntryFromFile(Path.Combine(FileSystem.AppDataDirectory, "BarParty.db3"), "BarParty.db3");

            // Agregar la carpeta de imágenes
            string directorioLocal = Path.Combine(AppContext.BaseDirectory, "Cimagenes");
            foreach (string filePath in Directory.EnumerateFiles(directorioLocal))
            {
                string relativePath = Path.GetRelativePath(directorioLocal, filePath);
                archive.CreateEntryFromFile(filePath, $"Cimagenes/{relativePath}");
            }
        }
        await DisplayAlert("Exportación", "La exportación exitosa", "OK");
    }
    public async void importar(object sender, EventArgs e)
    {
        _localDbService.import();
        bool doo = false;
        // Verificar si ya tienes el permiso
        var status = Permissions.CheckStatusAsync<Permissions.StorageRead>();
        if (status.Result == PermissionStatus.Granted)
        {
            // Tienes el permiso, puedes leer el archivo aquí
            doo = true;
        }
        else
        {
            // Solicitar permiso
            var perm = await Permissions.RequestAsync<Permissions.StorageRead>();
            status = Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status.Result == PermissionStatus.Granted)
            {
                // Permiso concedido, puedes leer el archivo aquí
                doo = true;
            }
            else
            {
                // Permiso denegado, manejar esto según tus necesidades
                doo = false;
            }
        }
        if (doo)
        {
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    Console.WriteLine(result.FileName);
                    // Verificar si el archivo seleccionado es un archivo ZIP
                    if (result.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        if (result.FileName== "BarPartyData.zip")
                        {
                            var action = await DisplayActionSheet("La información actual se perderá...", "Cancelar", null, "Continuar");
                            switch (action)
                            {
                                case "Continuar":
                                    await DescomprimirArchivo(result);
                                    break;
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "Archivo zip erroneo", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Cargar el archivo .zip", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al acceder al archivo ZIP: {ex.Message}");
            }
        }
    }
    private async Task DescomprimirArchivo(FileResult archivo)
    {
        try
        {
            using (var stream = await archivo.OpenReadAsync())
            {
                string directorioLocal = Path.Combine(AppContext.BaseDirectory, "Cimagenes");
                var rutaBaseDatosExistente = Path.Combine(FileSystem.AppDataDirectory, "BarParty.db3");
                var directorioExtraccion = Path.Combine(AppContext.BaseDirectory, "Extraccion");
                Directory.CreateDirectory(directorioExtraccion);
                var rutaArchivoZip = Path.Combine(directorioExtraccion, archivo.FileName);
                using (var fileStream = File.Create(rutaArchivoZip))
                {
                    await stream.CopyToAsync(fileStream);
                }
                ZipFile.ExtractToDirectory(rutaArchivoZip, directorioExtraccion);
                var rutaBaseDatosZip = Path.Combine(directorioExtraccion, "BarParty.db3");
                File.Copy(rutaBaseDatosZip, rutaBaseDatosExistente, true);
                var imagenesZip = Path.Combine(directorioExtraccion, "Cimagenes");
                if (Directory.Exists(directorioLocal))
                {
                    Directory.Delete(directorioLocal, true);
                }
                Directory.Move(imagenesZip, directorioLocal);
            }
            await DisplayAlert("Importación", "Importación exitosa", "OK");
        }
        catch (Exception ex)
        {
            // error al descomprimir
        }
    }
}



