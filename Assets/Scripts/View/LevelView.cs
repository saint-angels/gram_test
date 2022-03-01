using System.Collections;
using System.Collections.Generic;
using Tactics.Battle;
using UnityEngine;

namespace Tactics.View
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private Transform[] userUnitPoints = null;
        [SerializeField] private Transform[] enemyUnitPoints = null;

        public void Init(BattleManager battleManager)
        {
            battleManager.OnBattleInit += (userUnits, enemyUnits) =>
            {
                for (int i = 0; i < userUnits.Count; i++)
                {
                    UnitShell userUnit = userUnits[i];
                    Transform userPoint = userUnitPoints[i];
                    userUnit.transform.position = userPoint.position;
                }

                for (int i = 0; i < enemyUnits.Count; i++)
                {
                    UnitShell enemyUnit = enemyUnits[i];
                    enemyUnit.transform.position = enemyUnitPoints[i].position;
                }
            };

        }

    }
}
