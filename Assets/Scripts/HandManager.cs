using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardPosition
{
    public Vector3 Position;
    public CardBase Card;
    
}

public class HandManager : MonoBehaviour
{
    public List<CardPosition> HandCards = new List<CardPosition>();

    public CardBase cardPrefab;
    
    public void Start() {       
    }
    
    public void AddCard(CardBase cardPrefab)
    {
        // Находим первое свободное место
        for (int i = 0; i < HandCards.Count; i++)
        {
            if (HandCards[i].Card == null) // Если место свободно
            {
                // Создаем карту
                CardBase newCard = Instantiate(cardPrefab);

                // Устанавливаем её позицию
                newCard.transform.position = HandCards[i].Position;

                // Привязываем карту к позиции
                HandCards[i].Card = newCard;

                return;
            }
        }

        // Если все места заняты
        Debug.Log("Нет свободных мест для добавления карты!");
    }

    public void UpdatePos()
    {
        // Проверяем список позиций
        for (int i = 0; i < HandCards.Count; i++)
        {
            if (HandCards[i].Card == null) // Если нашли пустое место
            {
                for (int j = i + 1; j < HandCards.Count; j++)
                {
                    if (HandCards[j].Card != null) // Находим следующую карту
                    {
                        // Перемещаем карту в пустое место
                        HandCards[j].Card.transform.position = HandCards[i].Position;

                        // Обновляем ссылки в списке
                        HandCards[i].Card = HandCards[j].Card;
                        HandCards[j].Card = null;

                        break; // Двигаем одну карту за раз
                    }
                }
            }
        }
    }


}
