using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.Helpers.StatefulEvent;
using UnityEngine;

namespace Tactics.Battle
{
    public class UnitShell : MonoBehaviour
    {
        public IStatefulEvent<int> HealthState => healthState;

        public event Action<UnitShell, int> OnAttack;
        public event Action OnDeath;

        private readonly StatefulEventInt<int> healthState = StatefulEventInt.Create(0);

        public int MaxHealth { get; private set; }
        public Faction Faction { get; private set; }

        public void Init(Faction faction)
        {
            this.Faction = faction;
            MaxHealth = 10;
            healthState.Set(10);
        }

        public void Attack()
        {
            OnAttack(this, 1);
        }

        public void Damage(int damage)
        {
            int newHealth = healthState.Value - damage;
            newHealth = Mathf.Clamp(newHealth, 0, MaxHealth);
            healthState.Set(newHealth);
            if (newHealth == 0)
            {
                OnDeath?.Invoke();
            }
        }
    }
}
