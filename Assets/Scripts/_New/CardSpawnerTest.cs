using UnityEngine;

public class CardSpawnerTest : MonoBehaviour {
    public GameObject cardPrefab; // Префаб карты
    public Transform handArea; // Куда добавлять карты (например, панель руки)


    public Sprite placeholderArt;
    public Sprite flag;

    void Start() {
        for (int i = 0; i < 5; i++)
        {
            SpawnCard();
        }
        

    }

    void SpawnCard(string name, string description, string quote, int power) {
        GameObject newCard = Instantiate(cardPrefab, handArea);
        CardBase cardUI = newCard.GetComponent<CardBase>();

        // Заглушка для арты (пока без картинки)
        //Sprite placeholderArt = null;
        
        cardUI.SetupCard(name, description, quote, power, placeholderArt, flag, CardType.Eye);
    }

    
    void SpawnCard() {
        GameObject newCard = Instantiate(cardPrefab, handArea);
        CardBase cardUI = newCard.GetComponent<CardBase>();

        // Заглушка для арты (пока без картинки)
        //Sprite placeholderArt = null;
        
        cardUI.SetupCard();
    }
}
