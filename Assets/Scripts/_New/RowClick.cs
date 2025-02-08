using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.LowLevel;

public class RowClick : MonoBehaviour, IPointerClickHandler
{
    public Transform cardContainer; // Контейнер для карт в этом ряду (может быть этот же объект)
    public int maxCardsInRow = 5;   // Максимальное количество карт в ряду

    public CardType thisRowType;    // Тип карт, которые могут быть размещены в этом ряду (например, Eye или Feather)
    public bool playerAlliesRow;

    private void Start()
    {
        if (cardContainer == null)
        {
            cardContainer = this.transform; // Если контейнер не назначен, используем текущий объект
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // Левый клик
        {
            OnRowClick();
        }
    }

    public void OnRowClick()
    {
        // Проверка, не превышено ли максимальное количество карт
        if (cardContainer.childCount >= maxCardsInRow)
        {
            Debug.LogWarning("Максимальное количество карт в ряду достигнуто!");
            return;
        }
        Player.Instance.PlayCardToARow(gameObject);
    }
}
