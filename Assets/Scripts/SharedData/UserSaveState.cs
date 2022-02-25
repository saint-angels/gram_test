using System.Collections;
using System.Collections.Generic;
using Tactics.Configs;
using UnityEngine;

namespace Tactics.SharedData
{
    [System.Serializable]
    public class UserSaveState
    {
        public UnitState[] availableUnits;

        public static UserSaveState Default(UnitsCollectionConfig unitsCollectionConfig)
        {
            var state = new UserSaveState();
            state.availableUnits = new UnitState[]
            {
                    unitsCollectionConfig.startingStates[0],
                    unitsCollectionConfig.startingStates[1],
                    unitsCollectionConfig.startingStates[2],
            };
            return state;
        }

        public UnitState GetUnitForType(UnitType unitType)
        {
            foreach (var unitState in availableUnits)
            {
                if (unitState.unitType == unitType)
                {
                    return unitState;
                }
            }
            Debug.LogError($"Can't find starting unit state for type {unitType}");
            return new UnitState();
        }
    }
}
