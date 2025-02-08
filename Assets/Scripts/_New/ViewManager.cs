using Unity.VisualScripting;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance;  // Синглтон

    [Header("Настройки подсказки")]
    public Transform viewArea;           // Объект в сцене, куда будут добавляться подсказки (TooltipArea)
    public GameObject viewPrefab;      // Префаб подсказки, назначенный в инспекторе


    public GameObject blocker;

    public GameObject curViewCard;
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
    }

    public void ShowTooltip(string cardName, string cardDescription, string cardQuote, int cardPower, Sprite cardArt, Sprite cardFlag)
    {

        if (blocker != null)
        {
            blocker.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Blocker не назначен в ViewManager!");
        }

        if (viewPrefab == null || viewArea == null)
        {
            Debug.LogWarning("viewPrefab или viewArea не назначены!");
            return;
        }

        // Создаем подсказку как дочерний объект tooltipArea
        GameObject newView = Instantiate(viewPrefab, viewArea);
        CardViewBase cardView = newView.GetComponent<CardViewBase>();
        if (cardView == null)
        {
            Debug.LogError("CardViewBase не найден на созданном объекте подсказки!");
        }
        else
        {
            cardView.ViewCard(cardName, cardDescription, cardQuote, cardPower, cardArt, cardFlag);
            curViewCard = newView;
        }
    }

    public void CloseTooltip(){
        if (curViewCard!=null)
            Destroy(curViewCard);

        if (blocker != null)
        {
            blocker.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Blocker не назначен в ViewManager!");
        }

    }
}
