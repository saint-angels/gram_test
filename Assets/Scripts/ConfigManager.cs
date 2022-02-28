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
        public UnitSpritesConfig UnitSprites => unitSpritesConfig;

        [SerializeField] private EnemyUnitsConfig enemyUnitsConfig = null;
        [SerializeField] private UnitsCollectionConfig unitsCollectionConfig = null;
        [SerializeField] private UnitSpritesConfig unitSpritesConfig = null;
    }
}
