using UnityEngine;

public class Warrior : CardBase
{
    [Header("Warrior Specific")]
    public string weapon;  // Оружие воина

    // Переопределенный метод Spawn для воина
    // public override void Spawn(Vector3 position)
    // {
    //     base.Spawn(position);  // Вызов базового метода

    //     // Дополнительная специфика для воина
    //     Debug.Log($"Воин {cardName} использует оружие: {weapon}");
    // }
}
