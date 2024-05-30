using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManagerV2
{
     public class Recipe
    {
        // protperties for the recipe class
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }

        // event for the calorie notification
        public event CalorieNotification OnCalorieExceed;

        // constructor to initialize the recipe with name, ingredient and steps
        public Recipe(string name, List<Ingredient> ingredients, List<string> steps)
        {
            Name = name;
            Ingredients = ingredients;
            Steps = steps;
        }
        //-------------------------------------------------------------------------------------------------
        // method to display the recipe details
        public void DisplayRecipe()
        {
            Console.WriteLine($"Recipe: {Name}"); // display the recipe name

            Console.WriteLine("Ingredients:"); // display the ingredients

            foreach (var ingredient in Ingredients) // loop through the ingredients
            {
                Console.WriteLine($"- {ingredient.Quantity} {ingredient.Unit} {ingredient.Name} ({ingredient.Calories} calories, {ingredient.FoodGroup})"); // display the ingredient details
            }
            Console.WriteLine("\nSteps:"); // display the steps

            for (int i = 0; i < Steps.Count; i++) // loop through the steps
            {
                Console.WriteLine($"{i + 1}. {Steps[i]}"); // display the step number and the step details
            }
            // calculate the total calories and display them
            double totalCalories = Ingredients.Sum(i => i.Calories);

            Console.WriteLine($"\nTotal Calories: {totalCalories}"); // display the total calories

            // trigger the calorie notification event if the calories exceed 300 calories
            OnCalorieExceed?.Invoke(totalCalories);
        }
        //--------------------------------------------------------------------------------------------------------------
        // method to scale the ingredients by a given factor selected by the user
        public void ScaleIngredients(double factor)
        {
            foreach (var ingredient in Ingredients) // loop through the ingredients
            {
                if (double.TryParse(ingredient.Quantity, out double quantity)) // parse the quantity of the ingredient
                {
                    quantity *= factor;
                    ingredient.Quantity = quantity.ToString(); // scale the quantity of the ingredient
                }
                else
                {
                    Console.WriteLine($"Warning: Unable to scale ingredient {ingredient.Name} with quantity {ingredient.Quantity}"); // display a warning message if the quantity cannot be parsed
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //method to calculate the total number of calories of the recipe
        public double TotalCalories()
        {
            return Ingredients.Sum(i => i.Calories); // calculate the total calories of the recipe
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------------------
