using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Opponent : MonoBehaviour
{
    public static Opponent Instance;

    [Header("Valuables")]
    int maxHandCount = 10;

    [Header("CardDeck")]
    public List<GameObject> curOpponentDeck;

    [Header("SceneReferences")]
    public Transform handArea;
    public TextMeshProUGUI deckCount;
    public TextMeshProUGUI handCount;

    [Header("Other")]
    public GameObject chosenCardToPlay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FirstSetUP();
    }

    public void FirstSetUP()
    {
        if (DeckLoader.Instance != null)
        {
            curOpponentDeck = DeckLoader.Instance.LoadDeck("MyFirstDeck");
        }
        else
        {
            Debug.LogError("No DeckLoader was found");
        }
        DrawCards(3);
        UpdateDeckHandCount();
    }

    public void DrawCards(int amount)
    {
        if (curOpponentDeck.Count ==0){
            //Debug.LogError("NO CARD TO DRAW CONSIDER AS LOSE");
            return;
        }
        if (handArea.childCount >= maxHandCount)
        {
            Debug.Log("Opponent can't draw any card because hand is too full");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            if (handArea.childCount >= maxHandCount)
            {
                Debug.Log("Cannot have more than 10 cards in hand.");
                break;
            }

            if (curOpponentDeck.Count == 0)
            {
                Debug.Log("No more cards in deck.");
                break;
            }

            GameObject newCard = Instantiate(curOpponentDeck[0], handArea);
            CardBase card = newCard.GetComponent<CardBase>();
            card.SetupCard();
            card.isCardinHand = true;
            curOpponentDeck.RemoveAt(0);
            UpdateDeckHandCount();
        }
    }

    public void UpdateDeckHandCount()
    {
        deckCount.text = curOpponentDeck.Count.ToString();
        handCount.text = handArea.childCount.ToString();
    }

    // Основная логика для разыгрывания карты оппонентом
    public void PlayOpponentTurn()
    {
        if (handArea.childCount == 0)
        {
            Debug.Log("No cards to play.");
            return;
        }

        // Выбираем случайную карту из руки оппонента
        int randomIndex = Random.Range(0, handArea.childCount);
        GameObject cardToPlay = handArea.GetChild(randomIndex).gameObject;
        CardBase cardBase = cardToPlay.GetComponent<CardBase>();

        if (cardBase != null)
        {
            // Выбираем подходящий ряд для карты
            GameObject selectedRow = ChooseRowForCard(cardBase);
            
            if (selectedRow != null)
            {
                PlayCardToRow(selectedRow, cardToPlay);
            }
            else
            {
                Debug.Log("No suitable row found for this card.");
            }
        }
    }

    // Выбор подходящего ряда для карты
    private GameObject ChooseRowForCard(CardBase card)
    {
        foreach (GameObject row in GameManager.Instance.opponentRows)
        {
            RowClick rowClick = row.GetComponent<RowClick>();

            // Проверяем, подходит ли тип карты для этого ряда
            if (!rowClick.playerAlliesRow &&
                (card.cardType == rowClick.thisRowType || card.cardType == CardType.Special))
            {
                // Проверка на количество карт в ряду (не более 5)
                if (rowClick.cardContainer.childCount < 5)
                {
                    return row; // Возвращаем подходящий ряд
                }
            }
        }
        return null; // Если подходящий ряд не найден
    }

    // Логика для разыгрывания карты в выбранный ряд
    public void PlayCardToRow(GameObject row, GameObject cardToPlay)
    {
        CardBase cardBase = cardToPlay.GetComponent<CardBase>();

        if (cardBase != null)
        {
            cardToPlay.transform.SetParent(row.GetComponent<RowClick>().cardContainer, false);
            //cardToPlay.transform.localPosition = Vector3.zero;
            // cardToPlay.transform.localScale = Vector3.one * 1.5f;

            // // Анимация уменьшения до нормального размера
            // cardToPlay.transform.DOScale(Vector3.one * 1.3f, 0.3f).SetEase(Ease.OutQuad);
            cardBase.GetComponent<CardBase>().EnterAnimation();
            cardBase.ApplyEffect();
            cardBase.isCardinHand = false;

            Debug.Log("Opponent played a card in row: " + row.name);

            UpdateDeckHandCount();
            GameManager.Instance.UpdateAllRowStats();
            GameManager.Instance.goFurther = true;
        }
        else
        {
            Debug.LogError("Card does not have CardBase component!");
        }
    }

    public void AddCardToOpponentRow(GameObject row, GameObject prefab) 
    {
        // Создаем экземпляр карты из префаба
        GameObject cardInstance = Instantiate(prefab);
        
        // Получаем компонент CardBase из созданной карты
        CardBase cardBase = cardInstance.GetComponent<CardBase>();

        if (cardBase != null)
        {
            cardBase.SetupCard();

            // Проверка на соответствие типа карты для ряда оппонента
            if (!row.GetComponent<RowClick>().playerAlliesRow) // Проверяем, что это ряд оппонента
            {
                if (cardBase.cardType == row.GetComponent<RowClick>().thisRowType || cardBase.cardType == CardType.Special)
                {
                    // Добавляем карту в контейнер ряда оппонента
                    cardInstance.transform.SetParent(row.GetComponent<RowClick>().cardContainer, false);
                    //cardInstance.transform.localPosition = Vector3.zero;

                    // // Увеличиваем масштаб карты при добавлении
                    // cardInstance.transform.localScale = Vector3.one * 1.5f;

                    // // Плавное уменьшение масштаба до нормального размера
                    // cardInstance.transform.DOScale(Vector3.one * 1.3f, 0.3f).SetEase(Ease.OutQuad);

                    // Применяем эффекты карты и меняем её статус
                    cardBase.GetComponent<CardBase>().EnterAnimation();
                    cardBase.ApplyEffect();
                    cardBase.isCardinHand = false;

                    Debug.Log("Карта добавлена в ряд оппонента: " + row.name);

                    // Обновляем количество карт в руке оппонента и силу его рядов
                    UpdateDeckHandCount(); 
                    GameManager.Instance.UpdateAllRowStats(); // Обновление статистики для оппонента
                }
                else
                {
                    Debug.LogWarning($"Карта типа {cardBase.cardType} не может быть добавлена в ряд типа {row.GetComponent<RowClick>().thisRowType} у оппонента!");
                }
            }
            else
            {
                Debug.LogWarning("Это ряд игрока, и карта не может быть размещена в этом ряду.");
            }
        }
        else
        {
            Debug.LogError("У выбранной карты отсутствует компонент CardBase!");
        }
    }

}
