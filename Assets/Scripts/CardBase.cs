using UnityEngine;
using TMPro;

public class CardBase : MonoBehaviour
{
    [Header("Card Parameters")]
    public string cardName;           // Имя карты
    public int originalPower;         // Оригинальная сила карты
    public int currentPower;          // Текущая сила карты (может изменяться в игре)
    public Texture2D art;             // Текстура арта карты (Texture2D)
    public GameObject prefabToView;   // Префаб для отображения карты

    [Header("Card Status")]
    public bool isInHand;             // Флаг: карта находится в руке?
    public int curLine;               // Текущая линия карты (идентификатор)
    public int curPosInLine;          // Текущая позиция карты в линии

    [Header("Card Elements")]
    public GameObject plane;          // Дочерний объект для арта (плоскость)
    public GameObject powerObj;       // Дочерний объект для отображения силы (например, текст)

    private MeshRenderer planeRenderer;  // Ссылка на MeshRenderer плоскости
    private TextMeshPro powerText;       // Ссылка на TextMeshPro для силы


    // Метод для инициализации карты
    public void Initialize(string name, int power, Texture2D cardArt, GameObject prefab)
    {
        cardName = name;
        originalPower = power;
        currentPower = power;
        art = cardArt;
        prefabToView = prefab;
        currentPower = 8;
        UpdateCardVisuals();
    }

    // Метод для обновления визуального отображения карты
    public void UpdateCardVisuals()
    {
        if (plane != null)
        {
            planeRenderer = plane.GetComponent<MeshRenderer>();  // Получаем компонент MeshRenderer
        }

        if (powerObj != null)
        {
            powerText = powerObj.GetComponent<TextMeshPro>();  // Получаем компонент TextMeshPro
        }

        // Устанавливаем арт на плоскости (текстуру на MeshRenderer)
        if (planeRenderer != null && art != null)
        {
            Material planeMaterial = planeRenderer.material;  // Получаем материал
            planeMaterial.mainTexture = art;  // Устанавливаем текстуру арта
        }
        else
        {
            Debug.LogError("Plane или MeshRenderer не найдены.");
        }

        // Устанавливаем силу на объекте powerObj
        if (powerText != null)
        {
            powerText.text = currentPower.ToString();  // Отображаем текущую силу
        }
        else
        {
            Debug.LogError("PowerObj или TextMeshPro не найдены.");
        }
    }

    // Метод для изменения текущей силы карты
    public void ChangePower(int amount)
    {
        currentPower += amount;
        UpdateCardVisuals(); // Обновляем отображение после изменения силы
    }

    // Метод для установки позиции карты
    public void SetPosition(Vector3 position, int lineIndex, int positionInLine)
    {
        transform.position = position;
        curLine = lineIndex;
        curPosInLine = positionInLine;
    }
}
