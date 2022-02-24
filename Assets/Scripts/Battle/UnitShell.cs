using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.Helpers.StatefulEvent;
using Tactics.SharedData;
using UnityEngine;

namespace Tactics.Battle
{
    public class UnitShell : MonoBehaviour
    {
        public IStatefulEvent<int> HealthState => healthState;

        public event Action<UnitShell, int> OnAttack;
        public event Action<UnitShell> OnDeath;

        private readonly StatefulEventInt<int> healthState = StatefulEventInt.Create(0);

        public Faction Faction { get; private set; }
        public UnitType UnitType { get; private set; }
        public UnitParams Params { get; private set; }

        public void Init(Faction faction, UnitType unitType, UnitParams unitParams)
        {
            this.Faction = faction;
            this.UnitType = unitType;
            healthState.Set(unitParams.maxHealth);
        }

        public void Attack()
        {
            OnAttack(this, 1);
        }

        public void Damage(int damage)
        {
            int newHealth = healthState.Value - damage;
            newHealth = Mathf.Clamp(newHealth, 0, Params.maxHealth);
            healthState.Set(newHealth);
            if (newHealth == 0)
            {
                Die();
            }
        }

        public void Die()
        {
            OnDeath?.Invoke(this);
        }
    }
}
