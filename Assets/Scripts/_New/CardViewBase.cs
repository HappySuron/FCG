using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardViewBase : MonoBehaviour
{
    public Image cardArt;
    public Image cardFlag;
    public TextMeshProUGUI cardPower;
    
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardQuote;

    public void ViewCard(string name, string description, string quote, int power, Sprite art, Sprite flag)
    {
        cardArt.sprite = art;
        cardFlag.sprite = flag;
        cardPower.text = power.ToString();

        cardName.text = name.ToString();
        cardDescription.text = description.ToString();
        cardQuote.text = quote.ToString();
    }
}