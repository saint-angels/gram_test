using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.SharedData;
using UnityEngine;

namespace Tactics.Battle
{
    public class BattleManager : MonoBehaviour
    {
        public event Action<List<UnitShell>, List<UnitShell>> OnBattleInit;
        [SerializeField] private Transform unitContainer;

        private List<UnitShell> unitsUser;
        private List<UnitShell> unitsEnemy;

        public void Init(InputController input)
        {
            input.OnUnitClick += (unit) =>
            {
                if (unit.Faction == Faction.User)
                {
                    unit.Attack();
                }
            };
        }

        public void StartBattle(UnitState[] selectedUnits, UnitState[] enemyUnitTypes)
        {
            unitsUser = new List<UnitShell>();
            unitsEnemy = new List<UnitShell>();
            InitUnitsForFaction(Faction.User, selectedUnits);
            InitUnitsForFaction(Faction.Enemy, enemyUnitTypes);

            OnBattleInit?.Invoke(unitsUser, unitsEnemy);


            void InitUnitsForFaction(Faction faction, UnitState[] units)
            {
                foreach (UnitState unitState in units)
                {
                    UnitShell unitPrefab = Resources.Load<UnitShell>($"Units/{unitState.unitType}");

                    UnitShell unit = GameObject.Instantiate(unitPrefab, Vector3.zero, Quaternion.identity, unitContainer);
                    unit.OnAttack += (attacker, damage) =>
                    {
                        var opposingUnits = faction switch
                        {
                            Faction.User => unitsEnemy,
                            Faction.Enemy => unitsUser,
                            _ => throw new Exception($"Unexpected faction {faction}!"),
                        };

                        if (opposingUnits.Count == 0)
                        {
                            Debug.LogError($"{unit.Faction} has no units to attack!");
                        }
                        else
                        {
                            opposingUnits[0].Damage(damage);
                        }

                        if (opposingUnits.Count == 0)
                        {
                            print($"{unit.Faction} won the battle");
                        }
                        else if (attacker.Faction == Faction.User)
                        {
                            unitsEnemy[0].Attack();
                        }
                    };
                    unit.OnDeath += (deadUnit) =>
                    {
                        var allies = faction switch
                        {
                            Faction.User => unitsUser,
                            Faction.Enemy => unitsEnemy,
                            _ => throw new Exception($"Unexpected faction {faction}!"),
                        };
                        allies.Remove(deadUnit);
                        Destroy(deadUnit.gameObject);

                    };
                    unit.Init(faction, unitState.unitParams);
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
