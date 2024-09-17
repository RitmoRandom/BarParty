using BarParty.Views;

namespace BarParty;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        MainPage = new AppShell();
        LocalDbService l=new LocalDbService();
        //MainPage = new NavigationPage(new MainPage(l));
    }
}
