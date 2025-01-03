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

    // Переменная для хранения созданного PrefabView
    private GameObject activePrefabView;

    // Флаг для отслеживания состояния PrefabView
    private bool isPrefabViewActive = false;

    // Метод для инициализации карты
    public void Initialize(string name, int power, Texture2D cardArt, GameObject prefab)
    {
        cardName = name;
        originalPower = power;
        currentPower = power;
        art = cardArt;
        prefabToView = prefab;
        UpdateCardVisuals();
    }

    // Метод для обновления визуального отображения карты
    public void UpdateCardVisuals()
    {
        if (plane != null)
        {
            planeRenderer = plane.GetComponent<MeshRenderer>();
        }

        if (powerObj != null)
        {
            powerText = powerObj.GetComponent<TextMeshPro>();
        }

        if (planeRenderer != null && art != null)
        {
            Material planeMaterial = planeRenderer.material;
            planeMaterial.mainTexture = art;
        }

        if (powerText != null)
        {
            powerText.text = currentPower.ToString();
        }
    }

    // Метод для изменения текущей силы карты
    public void ChangePower(int amount)
    {
        currentPower += amount;
        UpdateCardVisuals();
    }

    // Метод для установки позиции карты
    public void SetPosition(Vector3 position, int lineIndex, int positionInLine)
    {
        transform.position = position;
        curLine = lineIndex;
        curPosInLine = positionInLine;
    }

    // Метод для управления PrefabView
    private void HandlePrefabView()
    {
        if (activePrefabView == null) // Если объект не создан
        {
            activePrefabView = Instantiate(prefabToView, new Vector3(0, 20, 44), Quaternion.Euler(270, 0, 0));
            isPrefabViewActive = true; // Устанавливаем флаг активности
        }
        else // Если объект уже создан
        {
            Destroy(activePrefabView);
            activePrefabView = null;
            isPrefabViewActive = false; // Сбрасываем флаг
        }
    }

    // Unity событие: вызывается при наведении мыши
    public void OnMouseOver()
    {
        Debug.Log("Mouse is over the object.");
        if (Input.GetMouseButtonDown(1)) // ПКМ
        {
            Debug.Log("Right Mouse Button clicked.");
            HandlePrefabView();
        }
    }

    // Проверяем глобальные клики
    private void Update()
    {
        if (isPrefabViewActive && Input.GetMouseButtonDown(0)) // ЛКМ в любом месте
        {
            Debug.Log("Click outside, closing PrefabView.");
            Destroy(activePrefabView);
            activePrefabView = null;
            isPrefabViewActive = false; // Сбрасываем флаг
        }
    }
}
