using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.SharedData
{
    [System.Serializable]
    public struct UserSaveState
    {
        public UnitState[] availableUnits;
    }
}
