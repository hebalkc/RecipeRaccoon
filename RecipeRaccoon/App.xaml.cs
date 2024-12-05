using RecipeRaccoon.Pages;

namespace RecipeRaccoon;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Set the main page of your application
        MainPage = new NavigationPage(new Pages.HomePage());
    }
}
