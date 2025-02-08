using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PlayerDeckManager : MonoBehaviour
{
    public static PlayerDeckManager Instance;

    public List<GameObject> currentPlayerDeckCards;
    public bool test = false;

    public string deckName;
    public void SaveDeck(string deckName)
    {
        DeckData deckData = new DeckData
        {
            deckName = deckName,
            cards = new List<string>()
        };

        // Добавляем ID всех карт из текущей колоды
        foreach (GameObject card in currentPlayerDeckCards)
        {
            CardBase cardBase = card.GetComponent<CardBase>();
            if (cardBase != null)
            {
                deckData.cards.Add(cardBase.cardID);
            }
        }

        string json = JsonUtility.ToJson(deckData, true);
        File.WriteAllText(Application.persistentDataPath + "/" + deckName + ".json", json);
        Debug.Log("Deck saved: " + Application.persistentDataPath + "/" + deckName + ".json");
    }



    private void Start(){

    }

    
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


    private void Update() {
        if (test)
        {
            SaveDeck(deckName);
        }    
    }

    List<GameObject> GetFullDeck(string deckName){
        if (ViewManager.Instance != null)
        {
            currentPlayerDeckCards = DeckLoader.Instance.LoadDeck(deckName);
        }
        else{
            Debug.LogError("No DeckLoader was found");
        }
        return currentPlayerDeckCards;
    }
}