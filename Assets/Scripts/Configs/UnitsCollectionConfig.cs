using System.Collections;
using System.Collections.Generic;
using Tactics.SharedData;
using UnityEngine;

namespace Tactics.Configs
{
    [CreateAssetMenu(fileName = "UnitsCollectionConfig", menuName = "Config/UnitsCollectionConfig")]
    public class UnitsCollectionConfig : ScriptableObject
    {
        public UnitState[] startingStates;
    }
}
