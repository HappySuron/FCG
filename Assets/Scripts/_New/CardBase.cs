using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    public enum CardType
    {
        Feather,
        Scroll,
        Eye,
        Letter,
        Special
    }

public class CardBase : MonoBehaviour, IPointerClickHandler
{
    [Header("BaseStats")]
    public string cardID = "000";
    public string cardName;
    public string cardDescription;
    public string cardQuote;
    public int cardPowerBase;
    public int cardPowerCurrent;
    public CardType cardType;

    [Header("ArtsSprites")]
    public Sprite cardArtSprite;
    public Sprite cardFlagSprite;


    [Header("ArtChildObjects")]
    public Image cardArt;
    public Image cardFlag;
    public TextMeshProUGUI cardPower;


    [Header("Other")]
    public bool isCardinHand = false;

    public bool isPlayerAlly = false;

    public void SetupCard(string name, string description, string quote, int power, Sprite art, Sprite flag, CardType type)
    {
        cardName = name;
        cardDescription = description;
        cardQuote = quote;
        cardPowerBase = power;
        cardPowerCurrent = power;
        cardArtSprite = art;
        cardFlagSprite = flag;
        cardType = type;
        
        // Обновление UI можно сделать отдельно, например, вызовом UpdateUI()
    }

    public void SetupCard()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        cardArt.sprite = cardArtSprite;
        cardFlag.sprite = cardFlagSprite;
        cardPower.text = cardPowerCurrent.ToString();
    }

    // Обработчик клика (IPointerClickHandler)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isCardinHand && isPlayerAlly && GameManager.Instance.currentGameState == GameState.PlayerTurn)
                SelectCard();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ShowTooltip();
        }
    }


    public void SelectCard()
    {
        Debug.Log("Selected card: " + cardName);
        // Дополнительная логика выделения...
        Player.Instance.chosenCardToPlay = gameObject;
    }

    public void ShowTooltip()
    {
        Debug.Log("Card Description: " + cardDescription);
        if (ViewManager.Instance != null)
        {
            ViewManager.Instance.ShowTooltip(cardName, cardDescription, cardQuote, cardPowerBase, cardArtSprite, cardFlagSprite);
        }
        else
        {
            Debug.LogWarning("TooltipManager не найден на сцене!");
        }
    }
    public virtual void ApplyEffect()
    {
        //GameManager.Instance.goFurther =
        // Debug.Log("It seems that that card doesnt have a override " + cardName);
    }
}
