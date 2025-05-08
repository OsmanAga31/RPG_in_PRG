using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemy : MonoBehaviour
{
    public int Health;
    [SerializeField] private int MaxHealth;
    [SerializeField] private int Defense;
    [SerializeField] private int Level;
    [SerializeField] private bool isEnemyDeath;
    [SerializeField] private string EnemyName;

    public virtual void LoadEnemyPrefab(string enemyName)
    {
        SpawnManager.instance.SpawnObject();
    }

    public virtual void Attack(BattleCharacter target) { }
    public virtual void Defend(BattleCharacter target) { }
    public virtual void UseAbility(BattleCharacter target, string abilityName) { }
    public virtual void UseItem(string itemName) { }
    public virtual void GetDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            isEnemyDeath = true;
            Health = 0;
        }
    }

    public virtual void Heal(int healAmount)
    {
        Health += healAmount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }
}
