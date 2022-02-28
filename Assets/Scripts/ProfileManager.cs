using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.Battle;
using Tactics.SharedData;
using UnityEngine;

namespace Tactics
{
    public class ProfileManager : MonoBehaviour
    {
        public event Action<List<UnitState>> OnUnitsParamUpgrade;

        private LocalCacheManager cacheManager;
        private ConfigManager configManager;

        public void Init(BattleManager battleManager, LocalCacheManager cacheManager, ConfigManager configManager)
        {
            this.cacheManager = cacheManager;
            this.configManager = configManager;

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
                //If user unlocked everything, don't make the counter negative
                if (0 < saveState.battlesUntilNextUnitUnlock)
                {
                    saveState.battlesUntilNextUnitUnlock--;
                }
                bool allUnitsUnlocked = configManager.UnitsCollection.startingStates.Length == saveState.unlockedUnits.Count;
                bool canUnlockNewUnit = saveState.battlesUntilNextUnitUnlock <= 0;
                if (allUnitsUnlocked == false && canUnlockNewUnit)
                {
                    saveState.battlesUntilNextUnitUnlock = 5;
                    int newUnitIndex = saveState.unlockedUnits.Count;
                    UnitState newUnitState = configManager.UnitsCollection.startingStates[newUnitIndex];
                    saveState.unlockedUnits.Add(newUnitState);

                }
                saveState.battlesCount++;
                cacheManager.Save<UserSaveState>(saveState, allowOverwrite: true);
                OnUnitsParamUpgrade?.Invoke(unitParamsDelta);
            };
        }

        public int GetBattleCount()
        {
            return GetUserSaveState().battlesCount;
        }

        public UnitState[] GetUnlockedUnits()
        {
            return GetUserSaveState().unlockedUnits.ToArray();
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
                saveState = UserSaveState.Default(configManager.UnitsCollection);
            }
            return saveState;
        }
    }
}
