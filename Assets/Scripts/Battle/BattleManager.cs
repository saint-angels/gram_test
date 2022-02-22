using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Battle
{
    public class BattleManager : MonoBehaviour
    {
        public event Action<List<UnitShell>, List<UnitShell>> OnBattleInit;
        [SerializeField] private Transform unitContainer;

        private List<UnitShell> unitsUser;
        private List<UnitShell> unitsEnemy;

        public void StartBattle(List<UnitType> selectedUnits)
        {
            unitsUser = new List<UnitShell>();
            unitsEnemy = new List<UnitShell>();
            InitUnitsForFaction(Faction.User, selectedUnits);
            InitUnitsForFaction(Faction.Enemy, selectedUnits);

            OnBattleInit?.Invoke(unitsUser, unitsEnemy);

            void InitUnitsForFaction(Faction faction, List<UnitType> units)
            {
                foreach (UnitType selectedType in units)
                {
                    UnitShell unitPrefab = Resources.Load<UnitShell>($"Units/{selectedType}");

                    UnitShell unit = GameObject.Instantiate(unitPrefab, Vector3.zero, Quaternion.identity, unitContainer);
                    unit.Init();
                    switch (faction)
                    {
                        case Faction.User:
                            unitsUser.Add(unit);
                            break;
                        case Faction.Enemy:
                            unitsEnemy.Add(unit);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
