using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tactics.SharedData;
using Tactics.Windows;
using Tactics.Configs;
using Tactics.Helpers.StatefulEvent;

namespace Tactics
{
    public class Root : MonoBehaviour
    {
        public enum GameState
        {
            None,
            SelectingUnits,
            Battle,
        }

        public IStatefulEvent<GameState> State => state;

        public static CameraController CameraController => _instance.cameraController;

        [SerializeField] private Battle.BattleManager battleManager = null;
        [SerializeField] private View.LevelView levelView = null;
        [SerializeField] private CameraController cameraController = null;
        [SerializeField] private InputController inputController = null;

        [SerializeField] private UnitsCollectionConfig unitsCollectionConfig = null;
        [SerializeField] private EnemyUnitsConfig enemiesConfig = null;
        [SerializeField] private UIManager uiManager = null;

        private static Root _instance;

        private readonly StatefulEventInt<GameState> state = StatefulEventInt.CreateEnum<GameState>(GameState.None);

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            levelView.Init(battleManager);
            battleManager.Init(inputController);
            battleManager.OnUserUnitsSurvived += (survivedUserUnits) =>
            {
                GoMeta();
            };

            GoMeta();

            void GoMeta()
            {
                var unitSelectionWindow = uiManager.ShowUnitSelection(unitsCollectionConfig.startingStates);
                unitSelectionWindow.OnUnitsSelected += (selectedUnits) =>
                {
                    GoBattle(selectedUnits);
                };
            }

            void GoBattle(UnitState[] selectedUnits)
            {
                uiManager.ShowHUD(battleManager, CameraController);
                var enemyStates = new UnitState[] { enemiesConfig.enemyStates[0] };
                battleManager.StartBattle(selectedUnits, enemyStates);
            }
        }
    }
}
