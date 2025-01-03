using UnityEngine;

public class Mage : CardBase
{
    [Header("Mage Specific")]
    public string spell;  // Заклинание мага

    // Переопределенный метод Spawn для мага
    // public override void Spawn(Vector3 position)
    // {
    //     base.Spawn(position);  // Вызов базового метода

    //     // Дополнительная специфика для мага
    //     Debug.Log($"Маг {cardName} использует заклинание: {spell}");
    // }
}
