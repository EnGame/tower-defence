using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProfile", menuName = "Taktika/Enemy Profile", order = 1)]
public class EnemyProfile : ScriptableObject
{
    public int health;
    public int attack;
    public int gold;
    public float moveSpeed = 5;
}
