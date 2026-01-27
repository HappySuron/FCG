using System.Collections.Generic;
using UnityEngine;
public class CardLexus : CardBase
{
    [Header("Uniqe")]
    public int amountToBuff;
    public override void ApplyEffect()
    {
        List<GameObject> cardsToBuff;
        if (this.isPlayerAlly)
             cardsToBuff = GameManager.Instance.playerRows;
        else
            cardsToBuff = GameManager.Instance.opponentRows;

        foreach (Transform child in cardsToBuff[1].transform)
        {
            CardBase card = child.GetComponent<CardBase>();
            card.ChangeCurCardPower(amountToBuff);
        }
    }
}
