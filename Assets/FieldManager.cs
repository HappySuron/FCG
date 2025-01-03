using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public Transform playerLine;  // Линия для карт игрока
    public Transform aiLine;      // Линия для карт ИИ
    public GameObject cardPrefab; // Префаб карты

    // Метод для разыгрывания карты
    public void PlayCard(bool isPlayer)
    {
        Transform line = isPlayer ? playerLine : aiLine;

        // Позиция карты на линии (немного случайно для разнообразия)
        Vector3 cardPosition = line.position;
        
        // Размещение карты
        Instantiate(cardPrefab, cardPosition, Quaternion.identity);
    }


    void Start()
    {
        PlayCard(true);  // Игрок играет карту
        PlayCard(false); // ИИ играет карту
    }
}
