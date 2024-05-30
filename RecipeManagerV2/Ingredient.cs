using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManagerV2
{
    public class Ingredient
    {
        //properties for the ingredient class / getters/setters
            public string Name { get; set; }
            public string Quantity { get; set; }
            public string Unit { get; set; }
            public double Calories { get; set; }
            public string FoodGroup { get; set; }
        }
    }
