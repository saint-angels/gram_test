using System.Collections;
using System.Collections.Generic;
using Tactics.Configs;
using UnityEngine;

namespace Tactics
{
    public class ConfigManager : MonoBehaviour
    {
        public EnemyUnitsConfig EnemyUnits => enemyUnitsConfig;
        public UnitsCollectionConfig UnitsCollection => unitsCollectionConfig;

        [SerializeField] private EnemyUnitsConfig enemyUnitsConfig = null;
        [SerializeField] private UnitsCollectionConfig unitsCollectionConfig = null;
    }
}
