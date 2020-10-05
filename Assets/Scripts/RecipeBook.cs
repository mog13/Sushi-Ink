using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
  
    public class RecipeBook : MonoBehaviour
    {
        public List<Recipe> recipes = new List<Recipe>();

        public Recipe FindRecipe(List<Ingredients> ingredients)
        {
            Recipe re = recipes.Find(r => r.ingredients.SequenceEqual(ingredients));

            return re;
        }
    }
}