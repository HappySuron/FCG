using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [Header("Player Line Positions")]
    public List<Transform> RedPlayerLinePositions = new List<Transform>();
    public List<Transform> GreenPlayerLinePositions = new List<Transform>();
    public List<Transform> BluePlayerLinePositions = new List<Transform>();
    public List<Transform> YellowPlayerLinePositions = new List<Transform>();

    [Header("AI Line Positions")]
    public List<Transform> RedAILinePositions = new List<Transform>();
    public List<Transform> GreenAILinePositions = new List<Transform>();
    public List<Transform> BlueAILinePositions = new List<Transform>();
    public List<Transform> YellowAILinePositions = new List<Transform>();

    [Header("Card Lists")]
    public List<CardBase> RedPlayerCards = new List<CardBase>();
    public List<CardBase> GreenPlayerCards = new List<CardBase>();
    public List<CardBase> BluePlayerCards = new List<CardBase>();
    public List<CardBase> YellowPlayerCards = new List<CardBase>();

    public List<CardBase> RedAICards = new List<CardBase>();
    public List<CardBase> GreenAICards = new List<CardBase>();
    public List<CardBase> BlueAICards = new List<CardBase>();
    public List<CardBase> YellowAICards = new List<CardBase>();

    public GameObject cardPrefab; // Префаб карты

    // Метод для добавления карты на линию
    public void AddCardToLine(CardBase card, List<Transform> linePositions, List<CardBase> cards)
    {
        if (cards.Count < linePositions.Count)
        {
            Transform targetPosition = linePositions[cards.Count];
            card.SetPosition(targetPosition.position, cards.Count, cards.Count); // Устанавливаем позицию и параметры
            card.isInHand = false; // Карта больше не в руке
            cards.Add(card); // Добавляем карту в список
            card.transform.SetParent(targetPosition); // Устанавливаем родителя
        }
        else
        {
            Debug.LogWarning("Линия заполнена, не могу добавить карту.");
        }
    }

    // Метод для удаления карты с линии
    public void RemoveCardFromLine(CardBase card, List<Transform> linePositions, List<CardBase> cards)
    {
        if (cards.Contains(card))
        {
            int removedIndex = cards.IndexOf(card); // Находим индекс карты
            cards.Remove(card); // Удаляем карту из списка
            Destroy(card.gameObject); // Уничтожаем объект карты

            // Обновляем позиции оставшихся карт
            UpdateCardPositions(linePositions, cards, removedIndex);
        }
    }

    // Метод для обновления позиций карт на линии
    private void UpdateCardPositions(List<Transform> linePositions, List<CardBase> cards, int startIndex = 0)
    {
        for (int i = startIndex; i < cards.Count; i++)
        {
            Transform targetPosition = linePositions[i];
            cards[i].SetPosition(targetPosition.position, i, i); // Обновляем позицию карты
            cards[i].transform.SetParent(targetPosition); // Обновляем родителя
        }
    }

    // Тестирование добавления карт
    private void Start()
    {
        // Создаём новую карту
        // CardBase newCard = Instantiate(cardPrefab).GetComponent<CardBase>();
        // newCard.Initialize("Test Card", 5, null, cardPrefab); // Инициализация карты
        // AddCardToLine(newCard, RedPlayerLinePositions, RedPlayerCards);

        // // Пример удаления карты
        // RemoveCardFromLine(newCard, RedPlayerLinePositions, RedPlayerCards);
    }
}
