using System.Collections;
using System.Collections.Generic;
using Tactics.SharedData;
using UnityEngine;

namespace Tactics.Configs
{
    [CreateAssetMenu(fileName = "UnitsCollectionConfig", menuName = "Config/UnitsCollectionConfig")]
    public class UnitsCollectionConfig : ScriptableObject
    {
        [SerializeField] public UnitState[] startingStates;

        public UnitState GetUnitForType(UnitType unitType)
        {
            foreach (var unitState in startingStates)
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
