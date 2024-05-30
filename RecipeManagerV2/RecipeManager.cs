using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManagerV2
{
    // this class is responsible for managing the recipes, that includes: adding, displaying, searching, scaling, clearing, saving and loading recipes
    internal class RecipeManager
    {
        // list to store the recipes
        private List<Recipe> recipes = new List<Recipe>();
        // file path to save the recipes and load recipes
        private const string filePath = "recipes.txt";

        //--------------------------------------------------------------------------------------
        // method to add a recipe
        public void AddRecipe()
        {
            Console.WriteLine("\nEnter the details for the new recipe:"); // prompt the user to enter the details for the new recipe

            Console.Write("Enter the name of your recipe: "); // prompt the user to enter the recipe name

            string recipeName = Console.ReadLine(); // read the recipe name

            List<Ingredient> ingredients = GetIngredients(); // get the ingredients from the user

            List<string> steps = GetSteps(); // get the steps from the user

            // create a new recipe object and subsciibe to the event for the calorie notification
            Recipe recipe = new Recipe(recipeName, ingredients, steps);

            recipe.OnCalorieExceed += NotifyIfCaloriesExceed;  // Subscribe to the event
            recipes.Add(recipe);

            Console.WriteLine("\nRecipe added successfully!");
        }
        //-----------------------------------------------------------------------------------
        // method to display all the recipes
        public void DisplayAllRecipes()
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("\nNo recipes available."); // error message if no recipes are available
            }
            else
            {
                Console.WriteLine("\nAvailable Recipes:");
                recipes = recipes.OrderBy(r => r.Name).ToList();  // Sort recipes alphabetically
                int count = 1;
                foreach (var recipe in recipes)
                {
                    Console.WriteLine($"{count}. {recipe.Name} (Total Calories: {recipe.TotalCalories()})"); // display the recipe name and total calories
                    count++;
                }

                Console.Write("Enter the number of the recipe you want to view in detail: ");
                if (int.TryParse(Console.ReadLine(), out int recipeNumber) && recipeNumber > 0 && recipeNumber <= recipes.Count) // check if the user input is a valid integer
                {
                    recipes[recipeNumber - 1].DisplayRecipe(); // display the recipe details
                }
                else
                {
                    Console.WriteLine("Invalid recipe number."); // error message if the recipe number is invalid
                }
            }
        }
        //----------------------------------------------------------------------------------------
        // method to clear all the recipes
        public void ClearRecipes()
        {
            recipes.Clear();
            Console.WriteLine("\nAll recipes cleared.");
        }
        //-----------------------------------------------------------------------------------------------
        // method to search for a recipe by its name
        public void SearchRecipe(string name)
        {
            Recipe foundRecipe = recipes.Find(recipe => recipe.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); // find the recipe by name
            if (foundRecipe != null)
            {
                Console.WriteLine($"\nRecipe '{name}' Found:"); // message to notify the user that the recipe is found
                foundRecipe.DisplayRecipe();
            }
            else
            {
                Console.WriteLine($"\nRecipe '{name}' not found."); // error message if the recipe is not found
            }
        }
        //------------------------------------------------------------------------------------------
        // method to scale the ingredients of a recipe by a given factor selected by the user
        public void ScaleRecipeIngredients(string name, string scaleType)
        {
            double factor = GetScaleFactor(scaleType); // get the scale factor based on the user input
            if (factor == -1)
            {
                Console.WriteLine("\nInvalid scale type. Available options are 'half', 'double', 'triple', or a custom scale factor."); // error message if the scale type is invalid
                return;
            }

            Recipe foundRecipe = recipes.Find(recipe => recipe.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); // find the recipe by name
            if (foundRecipe != null)
            {
                foundRecipe.ScaleIngredients(factor);
                Console.WriteLine($"\nIngredients for recipe '{name}' scaled by a factor of {factor}."); // message to notify the user that the ingredients are scaled
            }
            else
            {
                Console.WriteLine($"\nRecipe '{name}' not found."); // error message if the recipe is not found
            }
        }
        //--------------------------------------------------------------------------------------------
        // method to save the recipes to a file
        public void SaveRecipesToFile()
        {
            using (StreamWriter writer = new StreamWriter(filePath)) // write the recipes to the file
            {
                foreach (var recipe in recipes)
                {
                    // write the recipe name
                    writer.WriteLine(recipe.Name);
                    // write the ingredients
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        writer.WriteLine($"{ingredient.Name}|{ingredient.Quantity}|{ingredient.Unit}|{ingredient.Calories}|{ingredient.FoodGroup}"); // write the ingredients to the file
                    }
                    // write the steps
                    writer.WriteLine("Steps:");
                    foreach (var step in recipe.Steps)
                    {
                        writer.WriteLine(step);
                    }
                    // separates each recipe with a line of dashes
                    writer.WriteLine("---");
                }
            }
            Console.WriteLine("\nRecipes saved to file.");
        }
        //----------------------------------------------------------------------------------------
        // method to load the recipes from a file
        public void LoadRecipesFromFile()
        {
            if (File.Exists(filePath)) // check if the file exists
            {
                recipes.Clear(); // clear the existing recipes
                using (StreamReader reader = new StreamReader(filePath)) 
                {
                    string line;
                    Recipe recipe = null;
                    while ((line = reader.ReadLine()) != null) // reads the file line by line
                    {
                        if (line == "---")
                        {
                            // end of recipe, add to list
                            recipes.Add(recipe);
                            recipe = null;
                        }
                        else if (recipe == null)
                        {
                            // start of a new recipe
                            recipe = new Recipe(line, new List<Ingredient>(), new List<string>()); // create a new recipe object
                        }
                        else if (line.StartsWith("Steps:"))
                        {
                            // read the steps
                            string step;
                            while ((step = reader.ReadLine()) != "---" && step != null) // read the steps until the end of the recipe
                            {
                                recipe.Steps.Add(step);
                            }
                            // add a recipe to the list
                            recipes.Add(recipe);
                            recipe = null; // reset the recipe
                        }
                        else
                        {
                            // read the ingredients
                            var parts = line.Split('|'); // split the line by the delimiter

                            if (parts.Length == 5) // check if the parts are valid
                            {
                                recipe.Ingredients.Add(new Ingredient // add the ingredients to the recipe
                                {
                                    Name = parts[0],// add the name to the ingredient

                                    Quantity = parts[1],// add the quantity to the ingredient

                                    Unit = parts[2],// add the unit to the ingredient

                                    Calories = double.Parse(parts[3]), // convert the calories to double

                                    FoodGroup = parts[4] // add the food group to the ingredient
                                });
                            }
                        }
                    }
                }
                Console.WriteLine("\nRecipes loaded from file."); // message to notify the user that the recipes are loaded from the file
            }
            else
            {
                Console.WriteLine("\nNo saved recipes found."); // error message if no saved recipes are found
            }
        }
        //-----------------------------------------------------------------------------------------
        // method to notify the user if the total calories exceed 300
        private void NotifyIfCaloriesExceed(double totalCalories) 
        {
            if (totalCalories > 300) // check if the total calories exceed 300
            {
                Console.WriteLine("Warning: This recipe exceeds 300 calories!"); // notify the user if the recipe exceeds 300 calories
            }
        } 
        //-----------------------------------------------------------------------------------------
        // method to get the ingredients from the user
        private static List<Ingredient> GetIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>(); // list to store the ingredients
            Console.Write("Enter the number of ingredients: ");
            if (int.TryParse(Console.ReadLine(), out int numIngredients)) // check if the user input is a valid integer
            {
                for (int i = 0; i < numIngredients; i++) // loop to get the ingredients from the user
                {
                    Console.Write($"Enter the name of ingredient {i + 1}: "); // prompt the user to enter the ingredient name
                    string name = Console.ReadLine();
                    Console.Write($"Enter the quantity of {name}: "); // prompt the user to enter the quantity
                    string quantity = Console.ReadLine();
                    Console.Write($"Enter the unit of measurement for {name}: "); // prompt the user to enter the unit of measurement
                    string unit = Console.ReadLine();
                    Console.Write($"Enter the calories for {name}: "); // prompt the user to enter the calories
                    double calories = double.Parse(Console.ReadLine());
                    Console.Write($"Enter the food group for {name} (Carbohydrates, Protein, Dairy, Fruit, Vegetables, Fats, Sugars): "); // prompt the user to enter the food group
                    string foodGroup = Console.ReadLine();
                    ingredients.Add(new Ingredient { Name = name, Quantity = quantity, Unit = unit, Calories = calories, FoodGroup = foodGroup });
                }
            }
            else
            {
                Console.WriteLine("Invalid number of ingredients."); // error message if the user enters an invalid number of ingredients
            }
            return ingredients; // return the list of ingredients
        }
        //-----------------------------------------------------------------------------------------

        // method to get the steps from the user
        private static List<string> GetSteps() 
        {
            List<string> steps = new List<string>(); // list to store the steps
            Console.Write("Enter the number of steps required to cook this meal: "); // prompt the user to enter the number of steps
            if (int.TryParse(Console.ReadLine(), out int numSteps)) // check if the user input is a valid integer
            {
                for (int i = 0; i < numSteps; i++) // loop to get the steps from the user
                {
                    Console.Write($"Enter step {i + 1}: "); // prompt the user to enter the step
                    steps.Add(Console.ReadLine());
                }
            }
            else
            {
                Console.WriteLine("Invalid number of steps."); // error message if the user enters an invalid number of steps
            }
            return steps; // return the list of steps
        }

        // method to get the scale factor based on the user input
        private double GetScaleFactor(string scaleType)
        {
            switch (scaleType.ToLower())
            {
                case "half": // scale the recipe to half
                    return 0.5;
                case "double": // scale the recipe to double
                    return 2.0;
                case "triple": // scale the recipe to triple
                    return 3.0;
                default:
                    if (double.TryParse(scaleType, out double customFactor)) // scale the recipe by a custom factor
                    {
                        return customFactor; // return the custom factor
                    }
                    return -1;
            }
        }
    }
}
//-----------------------------------------------------------------------------------------

