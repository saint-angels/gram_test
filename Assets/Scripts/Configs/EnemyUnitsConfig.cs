using System.Collections;
using System.Collections.Generic;
using Tactics.SharedData;
using UnityEngine;

namespace Tactics.Configs
{
    [CreateAssetMenu(fileName = "EnemyUnitsConfig", menuName = "Config/EnemyUnitsConfig")]
    public class EnemyUnitsConfig : ScriptableObject
    {
        //Every battle has just one enemy
        public UnitState[] enemyStates;
    }
}
