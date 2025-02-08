using JetBrains.Annotations;
using UnityEngine;
public class CardAI : CardBase
{
    [Header("Uniqe")]
    public GameObject zombiePrefab;
    public override void ApplyEffect()
    {
        if (isPlayerAlly){
            foreach (GameObject row in GameManager.Instance.playerRows)
            {
                if (row.transform.childCount < 5)
                    Player.Instance.AddCardToARow(row, zombiePrefab);
            }
        }
        else {
            foreach (GameObject row in GameManager.Instance.opponentRows)
            {
                if (row.transform.childCount < 5)
                    Opponent.Instance.AddCardToOpponentRow(row, zombiePrefab);
            }
        }
    }
}
