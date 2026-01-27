using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardExaminator : CardBase
{
    public GameObject chosenCardGO;  // Переменная для хранения выбранного объекта карты

    // Переопределяем ApplyEffect
    public override void ApplyEffect()
    {
        List<CardBase> allCards = GetAllCardsFromRows();
        if (allCards.Count == 0)
            return;
        int minPower = allCards.Min(card => card.cardPowerCurrent);  // Находим минимальную силу карты

        List<CardBase> cardsWithMinPower = allCards.Where(card => card.cardPowerCurrent == minPower).ToList();

        if (cardsWithMinPower.Count > 0)
        {
            int randomIndex = Random.Range(0, cardsWithMinPower.Count);  // Выбираем случайную карту с минимальной силой
            CardBase chosenCard = cardsWithMinPower[randomIndex];

            // Записываем GameObject выбранной карты в переменную
            chosenCardGO = chosenCard.gameObject;
            chosenCard.DestroyCard();
            GameManager.Instance.UpdateAllRowStats();
        }
        else
        {
            Debug.Log("No cards found with the minimum power.");
        }
    }

    // Метод для получения всех карт из рядов
    private List<CardBase> GetAllCardsFromRows()
    {
        List<CardBase> allCards = new List<CardBase>();

        // Собираем карты игрока или противника в зависимости от флага isPlayerAlly
        if (!this.isPlayerAlly)
        {
            foreach (var row in GameManager.Instance.playerRows)
            {
                foreach (Transform cardTransform in row.transform)
                {
                    CardBase card = cardTransform.GetComponent<CardBase>();
                    if (card != null)
                    {
                        allCards.Add(card);
                    }
                }
            }
        }
        else
        {
            foreach (var row in GameManager.Instance.opponentRows)
            {
                foreach (Transform cardTransform in row.transform)
                {
                    CardBase card = cardTransform.GetComponent<CardBase>();
                    if (card != null)
                    {
                        allCards.Add(card);
                    }
                }
            }
        }

        return allCards;
    }
}
