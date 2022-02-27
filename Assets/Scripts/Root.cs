﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tactics.SharedData;
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

        [SerializeField] private EnemyUnitsConfig enemiesConfig = null;
        [SerializeField] private UIManager uiManager = null;
        [SerializeField] private LocalCacheManager cacheManager = null;
        [SerializeField] private ProfileManager profileManager = null;

        private static Root _instance;

        private readonly StatefulEventInt<GameState> state = StatefulEventInt.CreateEnum<GameState>(GameState.None);

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            profileManager.Init(battleManager, cacheManager);
            levelView.Init(battleManager);
            battleManager.Init(inputController);
            battleManager.OnUserUnitsSurvived += (survivedUserUnits) =>
            {
                GoMeta();
            };

            GoMeta();

            void GoMeta()
            {
                var unlockedUnits = profileManager.GetUnlockedUnits();
                var unitSelectionWindow = uiManager.ShowUnitSelection(unlockedUnits);
                unitSelectionWindow.OnUnitsSelected += (selectedUnits) =>
                {
                    GoBattle(selectedUnits);
                };
            }

            void GoBattle(UnitState[] selectedUnits)
            {
                uiManager.ShowHUD(battleManager, CameraController, inputController);
                var enemyStates = new UnitState[] { enemiesConfig.enemyStates[0] };
                battleManager.StartBattle(selectedUnits, enemyStates);
            }
        }
    }
}
