using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tactics.SharedData;
using Tactics.Windows;
using Tactics.Configs;

namespace Tactics
{
    public class Root : MonoBehaviour
    {
        public static CameraController CameraController => _instance.cameraController;

        [SerializeField] private Battle.BattleManager battleManager = null;
        [SerializeField] private View.LevelView levelView = null;
        [SerializeField] private CameraController cameraController = null;
        [SerializeField] private BattleHUD battleHUD = null;
        [SerializeField] private InputController inputController = null;

        [SerializeField] private UnitSelectionWindow unitSelectionWindow = null;
        [SerializeField] private UnitsCollectionConfig unitsCollectionConfig = null;
        [SerializeField] private EnemyUnitsConfig enemiesConfig = null;

        private static Root _instance;

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            levelView.Init(battleManager);
            battleHUD.Init(battleManager, CameraController);
            battleManager.Init(inputController);

            unitSelectionWindow.Init(unitsCollectionConfig.startingStates);
            unitSelectionWindow.OnUnitsSelected += (selectedUnits) =>
            {
                var enemyStates = new UnitState[] { enemiesConfig.enemyStates[0] };
                battleManager.StartBattle(selectedUnits, enemyStates);
            };

        }
    }
}
