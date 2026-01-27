using UnityEngine;
public class CardCaller : CardBase
{
    [Header("Uniqe")]
    public int amountToSteal;
    public override void ApplyEffect()
    {
        
        GameManager.Instance.ChangeCurTokens(this.isPlayerAlly, amountToSteal);
        GameManager.Instance.ChangeCurTokens(!this.isPlayerAlly, -amountToSteal);
    }
}
