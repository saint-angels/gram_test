using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.SharedData
{
    [System.Serializable]
    public class UnitState
    {
        public UnitType unitType;
        public UnitParams unitParams;

        public UnitState Clone()
        {
            var cloneState = new UnitState();
            cloneState.unitType = unitType;
            cloneState.unitParams = unitParams;
            return cloneState;
        }

        //Return the difference between this unit state, and another one
        public UnitState GetDelta(UnitState state2)
        {
            UnitState deltaState = Clone();

            deltaState.unitParams.level -= state2.unitParams.level;
            deltaState.unitParams.experience -= state2.unitParams.experience;
            deltaState.unitParams.attack -= state2.unitParams.attack;
            deltaState.unitParams.maxHealth -= state2.unitParams.maxHealth;
            return deltaState;
        }
    }
}
