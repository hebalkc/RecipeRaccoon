using RecipeRaccoon.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace RecipeRaccoon.Pages;

public partial class RecipeDetailsPage : ContentPage
{
    public string RecipeImageUrl { get; private set; }
    public ObservableCollection<string> Ingredients { get; set; } = new();
    public ObservableCollection<RecipeStep> Steps { get; set; } = new();

    private const string ApiKey = "GIU6wvv7XncKUhLj7cifH0PQ0FI6Yo80";
    private const string BaseUrl = "https://api.apilayer.com/spoonacular/recipes";

    public RecipeDetailsPage(int recipeId, string imageUrl)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);


        // Set properties
        RecipeImageUrl = imageUrl;

        // Bind data to the page
        BindingContext = this;

        // Load recipe details
        LoadRecipeDetails(recipeId);
    }

    private async void LoadRecipeDetails(int recipeId)
    {
        try
        {
            // Build the URL for the API request
            var url = $"{BaseUrl}/{recipeId}/information?includeNutrition=false";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", ApiKey);

            // Fetch data from the API
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Failed to fetch recipe details.", "OK");
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            var recipeDetails = JsonSerializer.Deserialize<RecipeDetails>(content);

            if (recipeDetails == null)
            {
                await DisplayAlert("Error", "Recipe details are missing.", "OK");
                return;
            }

            // Bind Title and Image
            BindingContext = recipeDetails;

            // Populate Steps
            Steps.Clear();
            foreach (var step in recipeDetails.Steps)
            {
                Steps.Add(step);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Navigate back to the previous page
    }

}
