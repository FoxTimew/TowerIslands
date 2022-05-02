using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefenseSupportSO", menuName = "ScriptableObjects/Support/DefenseSupportSO", order = 1)]
public class DefenseSupportSO : SupportSO
{

    public int healthValue;

    private Building tower;
    public void Effect(int dmg)
    {
    }

    public override void RemoveEffect(GameObject go)
    {
        tower.takeDamage += tower.BaseTakeDamage;
    }
    public override void Enter(Collider2D other, ref Dictionary<GameObject,int> dic)
    {
        if (other.transform.CompareTag("Block"))
        {
            tower = other.gameObject.GetComponent<Building>();
            dic[other.gameObject] = healthValue;
            AddEffect(other.gameObject);
            tower.takeDamage -= tower.BaseTakeDamage;
        }
    }
}
