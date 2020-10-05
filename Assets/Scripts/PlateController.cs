using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
   
    public class PlateController : MonoBehaviour
    {
        public List<Ingredients> currentIngredients = new List<Ingredients>();
        private RecipeBook _recipeBook;
        private SpriteRenderer _sr;

        public Recipe currentRecipe = null;
        private void Start()
        {
            _recipeBook = FindObjectOfType<RecipeBook>();
            _sr = GetComponent<SpriteRenderer>();
        }

        public void Eaten()
        {
            currentIngredients = new List<Ingredients>();
            _sr.sprite = null;
            currentRecipe = null;
            
        }

        public void TryAddIngrediant(GameObject otherGameObject)
        {
            IsPartOfRecipe potentialIngredient = otherGameObject.GetComponent<IsPartOfRecipe>();
            if (potentialIngredient != null)
            {
                List<Ingredients> newIngrediants = new List<Ingredients>();
                newIngrediants = newIngrediants.Concat(currentIngredients).ToList();
                newIngrediants.Add(potentialIngredient.ingredient);
                Recipe foundRecipe = _recipeBook.FindRecipe(newIngrediants);
                if (foundRecipe)
                {
                    _sr.sprite = foundRecipe.outPut;
                    currentIngredients.Add(potentialIngredient.ingredient);
                    currentRecipe = foundRecipe;
                    Destroy(otherGameObject);
                }
            }
        }
    }
}