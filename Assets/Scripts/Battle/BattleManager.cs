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

        public void Init(InputController input)
        {
            input.OnUnitClick += (unit) =>
            {
                //Check for the correct state of the battle
                unit.Attack();
            };
        }

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

                    };
                    unit.Init(faction);
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
