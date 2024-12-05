namespace RecipeRaccoon.Models
{
    public class Recipe
    {
        public int Id { get; set; } // Recipe ID (to fetch details)
        public string Title { get; set; } // Title of the recipe
        public string ImageUrl { get; set; } // URL to the recipe's image
    }
}
