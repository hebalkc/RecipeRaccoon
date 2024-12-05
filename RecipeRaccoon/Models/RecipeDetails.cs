using System.Text.Json.Serialization;

public class RecipeDetails
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("image")]
    public string ImageUrl { get; set; }

    [JsonPropertyName("analyzedInstructions")]
    public List<Instruction> Instructions { get; set; } = new();

    [JsonIgnore]
    public List<RecipeStep> Steps => Instructions.FirstOrDefault()?.Steps ?? new List<RecipeStep>();
    [JsonIgnore]
    public List<string> Ingredients
    {
        get
        {
            var ingredients = new HashSet<string>();
            foreach (var instruction in Instructions)
            {
                foreach (var step in instruction.Steps)
                {
                    foreach (var ingredient in step.Ingredients)
                    {
                        ingredients.Add(ingredient.Name);
                    }
                }
            }
            return ingredients.ToList();
        }
    }
}

public class Instruction
{
    [JsonPropertyName("steps")]
    public List<RecipeStep> Steps { get; set; } = new();
}

public class RecipeStep
{
    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("step")]
    public string Step { get; set; }
    [JsonPropertyName("ingredients")]
    public List<Ingredient> Ingredients { get; set; } = new();
}
public class Ingredient
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}