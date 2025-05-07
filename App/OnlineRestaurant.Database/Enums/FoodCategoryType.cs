using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Enums
{
    public enum FoodCategoryType
    {
        InvalidFoodCategoryType = 0,
        Breakfast = 1,
        Appetizer = 2,
        Brunch = 4,
        Lunch = 8,
        Snack = 16,
        Soups = 32,
        Dinner = 64,
        Desserts = 128,
        AlcoholicBevarages = 256,
        NonAlcoholicBevarages = 512
    }
}
