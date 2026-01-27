using UnityEngine;
public class CardOperator : CardBase
{
    [Header("Uniqe")]
    public int amountToDraw;
    public override void ApplyEffect()
    {
        
        if (this.isPlayerAlly){
            Player.Instance.DrawCards(1);
        } else {
            Opponent.Instance.DrawCards(1);
        }
    }
}
