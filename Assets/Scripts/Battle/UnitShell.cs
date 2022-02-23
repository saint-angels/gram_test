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

        public event Action<UnitShell, UnitShell, int> OnAttack;
        public event Action<UnitShell> OnDeath;

        private readonly StatefulEventInt<int> healthState = StatefulEventInt.Create(0);

        public int MaxHealth { get; private set; }

        public void Init()
        {
            MaxHealth = 10;
            healthState.Set(10);
        }
    }
}
