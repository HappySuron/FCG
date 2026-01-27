using DG.Tweening;
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

    public virtual void EnterAnimation(){
        Debug.Log("SLV");
        transform.localScale = Vector3.one * 1.5f;
        transform.DOScale(Vector3.one * 1.3f, 0.3f).SetEase(Ease.OutQuad);
    }

    public void DestroyCard()
    {   
    transform.DOScale(Vector3.one * 1.1f, 0.3f).SetEase(Ease.InOutSine)
        .OnComplete(() => 
            transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutSine)
                .OnComplete(() => 
                {
                    // Убираем объект из дочерних объектов текущего родителя
                    transform.SetParent(null);
                    GameManager.Instance.UpdateAllRowStats();
                    // Удаляем объект из сцены после анимации
                    Destroy(gameObject);
                }));
    }


    public void ChangeCurCardPower(int amount){
        cardPowerCurrent = cardPowerCurrent + amount;
        cardPowerCurrent = Mathf.Max(0, cardPowerCurrent);
        if (cardPowerCurrent == cardPowerBase){
            cardPower.color = Color.black;
        }
        else if (cardPowerCurrent > cardPowerBase){
            cardPower.color = new Color(0, 0.5f, 0);;
        }
        else if (cardPowerCurrent < cardPowerBase){
            cardPower.color = new Color(0.5f, 0, 0);
        }
        cardPower.text = cardPowerCurrent.ToString();
    }
}
