using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeManagerV2;
using System.Collections.Generic;

namespace RecipeManagerV2.UnitTest
{
    [TestClass]
    public class RecipeTests
    {
        [TestMethod]
        public void TestTotalCalories()
        {
            // Arrange: Create a list of ingredients with known calories
            var ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Ingredient1", Quantity = "100", Unit = "grams", Calories = 100, FoodGroup = "Protein" },
                new Ingredient { Name = "Ingredient2", Quantity = "200", Unit = "grams", Calories = 150, FoodGroup = "Carbohydrates" },
                new Ingredient { Name = "Ingredient3", Quantity = "50", Unit = "grams", Calories = 50, FoodGroup = "Fats" }
            };

            var steps = new List<string> { "Step1", "Step2" };
            var recipe = new Recipe("Test Recipe", ingredients, steps);

            // Act: Calculate the total calories
            double totalCalories = recipe.TotalCalories();

            // Assert: Verify that the total calories are correct
            Assert.AreEqual(300, totalCalories, "The total calories calculation is incorrect.");
        }
    }
}

