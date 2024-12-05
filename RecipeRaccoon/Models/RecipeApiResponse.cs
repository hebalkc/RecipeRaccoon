using System.Text.Json.Serialization;

public class RecipeApiResponse
{
    [JsonPropertyName("results")]
    public List<Recipe> Results { get; set; }
}

public class Recipe
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; }

    [JsonIgnore]
    public string ImageUrl => Image; // Computed property for convenience
}
