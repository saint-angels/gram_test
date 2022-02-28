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

        public UnitState[] GetAIUnitsForBattleIndex(int battleIndex)
        {
            int enemyIndex = battleIndex % enemyStates.Length;
            var aiUnitStates = new UnitState[] { enemyStates[enemyIndex] };
            return aiUnitStates;
        }
    }
}
