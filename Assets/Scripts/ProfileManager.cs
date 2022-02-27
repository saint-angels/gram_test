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
        [SerializeField] private UnitsCollectionConfig unitsCollectionConfig = null;

        private LocalCacheManager cacheManager = null;

        public void Init(BattleManager battleManager, LocalCacheManager cacheManager)
        {
            this.cacheManager = cacheManager;


            battleManager.OnUserUnitsSurvived += (survivedUserUnits) =>
            {
                UserSaveState saveState = GetUserSaveState();
                foreach (UnitType unitType in survivedUserUnits)
                {
                    UnitState unitState = saveState.GetUnitForType(unitType);
                    unitState.unitParams.experience++;
                    //Level up the unit as much as possible
                    while (5 <= unitState.unitParams.experience)
                    {
                        unitState.unitParams.experience -= 5;
                        unitState.unitParams.level += 1;
                        unitState.unitParams.attack += Mathf.CeilToInt(unitState.unitParams.attack * 0.1f);
                        unitState.unitParams.maxHealth += Mathf.CeilToInt(unitState.unitParams.maxHealth * 0.1f);
                    }
                }
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
