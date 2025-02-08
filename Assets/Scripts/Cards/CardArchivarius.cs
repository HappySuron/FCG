using UnityEngine;
public class CardArchivarius : CardBase
{
    [Header("Uniqe")]
    public int amountToAdd;
    public override void ApplyEffect()
    {
        
        GameManager.Instance.ChangeCurTokens(this.isPlayerAlly, amountToAdd);
    }
}
