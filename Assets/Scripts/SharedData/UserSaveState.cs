using System.Collections;
using System.Collections.Generic;
using Tactics.Configs;
using UnityEngine;

namespace Tactics.SharedData
{
    [System.Serializable]
    public class UserSaveState
    {
        public List<UnitState> unlockedUnits;
        public int battlesUntilNextUnitUnlock;

        public static UserSaveState Default(UnitsCollectionConfig unitsCollectionConfig)
        {
            var state = new UserSaveState();
            state.unlockedUnits = new List<UnitState>()
            {
                    unitsCollectionConfig.startingStates[0],
                    unitsCollectionConfig.startingStates[1],
                    unitsCollectionConfig.startingStates[2],
            };
            state.battlesUntilNextUnitUnlock = 5;
            return state;
        }

        public UnitState GetUnitForType(UnitType unitType)
        {
            foreach (var unitState in unlockedUnits)
            {
                if (unitState.unitType == unitType)
                {
                    return unitState;
                }
            }
            Debug.LogError($"Can't find unit state for type {unitType}");
            return new UnitState();
        }


        public void UpdateUnitState(UnitState newUnitState)
        {
            for (int i = 0; i < unlockedUnits.Count; i++)
            {
                if (unlockedUnits[i].unitType == newUnitState.unitType)
                {
                    unlockedUnits[i] = newUnitState;
                    return;
                }
            }
            Debug.LogError($"Can't find unit state for type {newUnitState.unitType}");
        }
    }
}
