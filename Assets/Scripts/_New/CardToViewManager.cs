using System;
using UnityEngine;

public class CardToViewManager : MonoBehaviour {


    // Shitty code i know i know but fuck it its gonna be like that fuck it
    // it shouldn't be here but is deal with it
    public GameObject chosenCardToPlay;
    public static CardToViewManager Instance;

    private void Awake()
    {
        // Если в сцене уже есть TooltipManager, можно добавить проверку
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static implicit operator CardToViewManager(GameObject v)
    {
        throw new NotImplementedException();
    }
}