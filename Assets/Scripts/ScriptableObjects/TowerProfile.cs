using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerProfile", menuName = "Taktika/Tower Profile", order = 0)]
public class TowerProfile : ScriptableObject
{
    public string towerName;
    public string towerDescription;

    public List<TowerLevel> levels = new List<TowerLevel>();
}

[System.Serializable]
public class TowerLevel
{
    public int attack;
    public float fireSpeed;
    public float attackDistance;
    public int price;
}
