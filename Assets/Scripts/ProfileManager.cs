using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.Battle;
using Tactics.Configs;
using Tactics.SharedData;
using UnityEngine;

namespace Tactics
{
    public class ProfileManager : MonoBehaviour
    {
        public event Action<List<UnitState>> OnUnitsParamUpgrade;

        [SerializeField] private UnitsCollectionConfig unitsCollectionConfig = null;

        private LocalCacheManager cacheManager = null;

        public void Init(BattleManager battleManager, LocalCacheManager cacheManager)
        {
            this.cacheManager = cacheManager;


            battleManager.OnUserUnitsSurvived += (survivedUserUnits) =>
            {
                UserSaveState saveState = GetUserSaveState();
                var unitParamsDelta = new List<UnitState>();
                foreach (UnitType unitType in survivedUserUnits)
                {
                    UnitState prevUnitState = saveState.GetUnitForType(unitType);
                    UnitState newUnitState = prevUnitState.Clone();
                    newUnitState.unitParams.experience++;
                    //Level up the unit as much as possible
                    while (5 <= newUnitState.unitParams.experience)
                    {
                        newUnitState.unitParams.experience -= 5;
                        newUnitState.unitParams.level += 1;
                        newUnitState.unitParams.attack += Mathf.CeilToInt(newUnitState.unitParams.attack * 0.1f);
                        newUnitState.unitParams.maxHealth += Mathf.CeilToInt(newUnitState.unitParams.maxHealth * 0.1f);
                    }

                    saveState.UpdateUnitState(newUnitState);
                    UnitState unitUpgradeDelta = newUnitState.GetDelta(prevUnitState);
                    unitParamsDelta.Add(unitUpgradeDelta);
                }
                OnUnitsParamUpgrade?.Invoke(unitParamsDelta);
                cacheManager.Save<UserSaveState>(saveState, allowOverwrite: true);
            };

        }

        public UnitState[] GetUnlockedUnits()
        {
            return GetUserSaveState().unlockedUnits;
        }

        private UserSaveState GetUserSaveState()
        {
            UserSaveState saveState;
            if (cacheManager.FileExists<UserSaveState>())
            {
                saveState = cacheManager.Load<UserSaveState>();
            }
            else
            {
                saveState = UserSaveState.Default(unitsCollectionConfig);
            }
            return saveState;
        }
    }
}
