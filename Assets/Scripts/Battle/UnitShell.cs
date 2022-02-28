using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.Helpers.Promises;
using Tactics.Helpers.StatefulEvent;
using Tactics.Interfaces;
using Tactics.SharedData;
using UnityEngine;

namespace Tactics.Battle
{
    public class UnitShell : MonoBehaviour, ITooltipTextProvider
    {
        public event Action<UnitType> OnInit;
        public event Action<int> OnDamaged;

        public IStatefulEvent<int> HealthState => healthState;

        public event Action<UnitShell, int, Deferred> OnAttack;
        public event Action<UnitShell> OnDeath;

        private readonly StatefulEventInt<int> healthState = StatefulEventInt.Create(0);

        public Faction Faction { get; private set; }
        public UnitType UnitType { get; private set; }
        public UnitParams Params { get; private set; }

        public void Init(Faction faction, UnitType unitType, UnitParams unitParams)
        {
            this.Faction = faction;
            this.UnitType = unitType;
            this.Params = unitParams;
            healthState.Set(unitParams.maxHealth);

            OnInit?.Invoke(unitType);
        }

        public void Attack()
        {
            OnAttack?.Invoke(this, Params.attack, Deferred.GetFromPool());
        }

        public void Damage(int damage)
        {
            int newHealth = healthState.Value - damage;
            newHealth = Mathf.Clamp(newHealth, 0, Params.maxHealth);
            healthState.Set(newHealth);
            OnDamaged?.Invoke(damage);
            if (newHealth == 0)
            {
                Die();
            }
        }

        public void Die()
        {
            OnDeath?.Invoke(this);
        }

        public string GetTooltipText()
        {
            string levelLabel = $"level: {Params.level}";
            string experienceLabel = $"experience: {Params.experience}";
            string attackLabel = $"attack: {Params.attack}";
            string maxHealthLabel = $"health: {Params.maxHealth}";
            return $"{UnitType}\n{levelLabel}\n{experienceLabel}\n{attackLabel}\n{maxHealthLabel}";
        }
    }
}
