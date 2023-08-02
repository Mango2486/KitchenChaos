using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance { get; private set; }
    
    
    [SerializeField] private RecipeListSO recipeListSO;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private bool isInstantiating = false;
    private List<RecipeSO> waitingRecipeSOList;


    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        if (!isInstantiating && waitingRecipeSOList.Count < waitingRecipesMax)
        {
            StartCoroutine("SpawnRecipeCoroutine");
        }
    }

    private IEnumerator SpawnRecipeCoroutine()
    {
        isInstantiating = true;
        yield return new WaitForSeconds(spawnRecipeTimerMax);
        RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
        waitingRecipeSOList.Add(waitingRecipeSO);
        
        OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
       
        isInstantiating = false;
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;
                // Has the same number of ingredients
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // Cycling through all ingredients in the Recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycling through all ingredients in the Plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {   
                            //Ingredient matches!
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        // This Recipe ingredient was not found on the plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    //Player delivered the correct recipe
                    waitingRecipeSOList.RemoveAt(i);
                    
                    OnRecipeCompleted?.Invoke(this,EventArgs.Empty);
                    return;
                }
            }
        }
        //No matches found!
        Debug.Log("Player didn't deliver a correct recipe!");
    }

    public List<RecipeSO> getWatingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
}
