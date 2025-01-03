using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab; // Префаб карты
    private Vector3 spawnPosition = new Vector3(0, 20, 45); // Позиция для спавна карты
    private Vector3 spawnRotation = new Vector3(270, 0, 0); // Поворот для карты

    void Start()
    {
        // Проверяем, установлен ли префаб карты
        if (cardPrefab == null)
        {
            Debug.LogError("Card prefab is not assigned in the inspector.");
            return;
        }

        // Указываем ротацию в Quaternion
        Quaternion rotation = Quaternion.Euler(spawnRotation);

        // Создаём экземпляр карты
        GameObject spawnedCard = Instantiate(cardPrefab, spawnPosition, rotation);

        // Убедимся, что объект не наследует никакого родителя
        spawnedCard.transform.SetParent(null);

        // Логирование для проверки координат
        Debug.Log($"Card spawned at position: {spawnedCard.transform.position}, rotation: {spawnedCard.transform.rotation.eulerAngles}");

    }
}
