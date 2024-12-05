using RecipeRaccoon.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace RecipeRaccoon.Pages;


public partial class HomePage : ContentPage
{
    private const string ApiKey = "GIU6wvv7XncKUhLj7cifH0PQ0FI6Yo80"; // Replace with your actual API key
    private const string BaseUrl = "https://api.apilayer.com/spoonacular/recipes/complexSearch";
    private List<Recipe> allRecipes;
    public Command<Recipe> CardTappedCommand { get; }


    public HomePage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        //allRecipes = new List<Recipe>();
        CardTappedCommand = new Command<Recipe>(async (selectedRecipe) =>
        {
            if (selectedRecipe != null)
            {
                await Navigation.PushAsync(new RecipeDetailsPage(recipeId: selectedRecipe.Id, imageUrl: selectedRecipe.ImageUrl));
            }
        });
        BindingContext = this;
    }
    


    private async void OnSearchClicked(object sender, EventArgs e)
    {
        var userIngredients = IngredientEntry.Text;
        if (string.IsNullOrWhiteSpace(userIngredients))
        {
            await DisplayAlert("Input Error", "Please enter at least one ingredient.", "OK");
            return;
        }

        await LoadRecipesFromApi(userIngredients);
    }


    private async Task LoadRecipesFromApi(string query)
    {
        try
        {
            // Build the URL for the API
            var url = $"{BaseUrl}?query=&ignorePantry=true&includeIngredients={query}&instructionsRequired=true&addRecipeInformation=true";
            Console.WriteLine($"Request URL: {url}");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", ApiKey);

            // Make the API request
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Details: {errorDetails}");

                var errorMessage = response.StatusCode == System.Net.HttpStatusCode.InternalServerError
                    ? "Internal Server Error. Please try again later."
                    : $"Error: {response.StatusCode} - {errorDetails}";

                await DisplayAlert("Error", errorMessage, "OK");
                return;
            }

            // Parse the API response
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Raw Response: {content}");

            var apiResponse = JsonSerializer.Deserialize<RecipeApiResponse>(content);

            if (apiResponse?.Results == null || !apiResponse.Results.Any())
            {
                await DisplayAlert("No Results", "No recipes found for the provided query.", "OK");
                return;
            }

            // Directly bind API results to the CollectionView
            RecipeCollection.ItemsSource = apiResponse.Results;

            // Debugging: Log parsed results
            foreach (var result in apiResponse.Results)
            {
                Console.WriteLine($"Parsed Result: ID={result.Id}, Title={result.Title}, Image={result.Image}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to fetch data: {ex.Message}", "OK");
        }
    }

    private async void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Recipe selectedRecipe)
        {
            // Pass both ID and ImageUrl to the RecipeDetailsPage
            await Navigation.PushAsync(new RecipeDetailsPage(selectedRecipe.Id, selectedRecipe.ImageUrl));
        }

    // Clear the selection to allow future selections
    ((CollectionView)sender).SelectedItem = null;
    }


}