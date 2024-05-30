using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManagerV2
{
    internal class Program
    {
        // main mehtod to run the program
        public static void Main()
        {
            // creating an instance of the RecipeManager class to manage the recipes
            RecipeManager recipeManager = new RecipeManager();
            bool exit = false;

            // while loop that runs the program until the user chooses to exit
            while (!exit)
            {
                // displaying the menu options

                Console.WriteLine("\nRecipe Management System"); // heading line
                Console.WriteLine("1. Add a new recipe"); // selects option 1 to add a recipe
                Console.WriteLine("2. Display all recipes"); // selects option 2 to display all recipes
                Console.WriteLine("3. Search for a recipe");// selects option 3 to search for a recipe
                Console.WriteLine("4. Scale recipe ingredients"); // selects option 4 to scale the recipe ingredients
                Console.WriteLine("5. Clear all recipes");// selects option 5 to clear all recipes
                Console.WriteLine("6. Save recipes to file");// selects option 6 to save recipes to a file
                Console.WriteLine("7. Load recipes from file");// selects option 7 to load recipes from a file
                Console.WriteLine("8. Exit");// selects option 8 to exit the program
                Console.Write("Choose an option: ");

                // switch statement to execute the selected option (reads the user intput and performs the corresponding action)
                switch (Console.ReadLine())
                {
                    case "1":
                        recipeManager.AddRecipe(); // calls the AddRecipe method to add a new recipe
                        break;
                    case "2":
                        recipeManager.DisplayAllRecipes(); // calls the DisplayAllRecipes method to display all recipes
                        break;
                    case "3":
                        Console.Write("Enter the name of the recipe to search for: ");// prompts the user to enter the name of the recipe to search for
                        string recipeName = Console.ReadLine();
                        recipeManager.SearchRecipe(recipeName); // calls the SearchRecipe method to search for the recipe by name
                        break;
                    case "4":
                        Console.Write("Enter the name of the recipe to scale: ");// prompts the user to enter the name of the recipe to scale
                        string recipeToScale = Console.ReadLine();
                        Console.Write("Enter the scale type (half, double, triple, or custom scale factor): ");// prompts the user to enter the scale type
                        string scaleType = Console.ReadLine();
                        recipeManager.ScaleRecipeIngredients(recipeToScale, scaleType);// calls the ScaleRecipeIngredients method to scale the recipe ingredients
                        break;
                    case "5":
                        recipeManager.ClearRecipes();// calls the ClearRecipes method to clear all recipes
                        break;
                    case "6":
                        recipeManager.SaveRecipesToFile();// calls the SaveRecipesToFile method to save the recipes to a file
                        break;
                    case "7":
                        recipeManager.LoadRecipesFromFile();// calls the LoadRecipesFromFile method to load the recipes from a file
                        break;
                    case "8":
                        exit = true; // sets the exit flag to true to exit the program and end the loop
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");// displays an error message for invalid input
                        break;
                }
            }
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------------
