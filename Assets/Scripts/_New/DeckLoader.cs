using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DeckData
{
    public string deckName;
    public List<string> cards; // Храним только ID карт


}

public class DeckLoader : MonoBehaviour
{

    public static DeckLoader Instance;
    public Transform deckParent; // Куда добавлять карты на сцене
    public string deckName = "defaultDeck";

    // Словарь для хранения префабов по их ID
    public Dictionary<string, GameObject> cardPrefabs = new Dictionary<string, GameObject>();


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
        LoadCardPrefabs(); // Загружаем префабы в словарь
    }

    private void Start()
    {
        
        //List<GameObject> myDeck = LoadDeck("MyFirstDeck");  // Загружаем колоду, но не спавним её сразу
        // foreach (GameObject cardPrefab in myDeck)
        // {
        //     Debug.Log("Card in deck: " + cardPrefab.GetComponent<CardBase>().cardID);
        // }

    }

    // Загрузка всех префабов в словарь
    private void LoadCardPrefabs()
    {
        // Предположим, что все префабы хранятся в Resources/Cards/
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("Cards");

        foreach (var prefab in loadedPrefabs)
        {
            CardBase cardBase = prefab.GetComponent<CardBase>();
            if (cardBase != null && !cardPrefabs.ContainsKey(cardBase.cardID))
            {
                cardPrefabs.Add(cardBase.cardID, prefab);
            }
        }
    }

    // Загрузка колоды из JSON по названию
    public List<GameObject> LoadDeck(string deckName)
    {
        //Debug.LogError(deckName);
        List<GameObject> loadedCards = new List<GameObject>();  // Создаём список для хранения префабов

        string path = Application.persistentDataPath + "/" + deckName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DeckData loadedDeck = JsonUtility.FromJson<DeckData>(json);

            Debug.Log("Loading deck: " + loadedDeck.deckName);

            foreach (string cardID in loadedDeck.cards)
            {
                if (cardPrefabs.ContainsKey(cardID))
                {
                    GameObject cardPrefab = cardPrefabs[cardID];
                    loadedCards.Add(cardPrefab);  // Добавляем найденный префаб в список
                    Debug.Log("Loaded card prefab: " + cardID);
                }
                else
                {
                    Debug.LogWarning("Card with ID " + cardID + " not found in prefabs!");
                }
            }
        }
        else
        {
            Debug.LogWarning("Deck file not found at: " + path);
        }

        return loadedCards;  // Возвращаем список префабов
    }


    // Метод для загрузки колоды через код
    public void SetDeckNameAndLoad(string newDeckName)
    {
        deckName = newDeckName;
        LoadDeck(deckName);
    }
}
