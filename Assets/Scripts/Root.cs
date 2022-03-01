﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tactics.SharedData;
using Tactics.Helpers.Promises;

namespace Tactics
{
    public class Root : MonoBehaviour
    {
        public static ConfigManager Configs => _instance.configManager;

        [SerializeField] private Battle.BattleManager battleManager = null;
        [SerializeField] private View.LevelView levelView = null;
        [SerializeField] private CameraController cameraController = null;
        [SerializeField] private InputController inputController = null;

        [SerializeField] private UIManager uiManager = null;
        [SerializeField] private LocalCacheManager cacheManager = null;
        [SerializeField] private ProfileManager profileManager = null;
        [SerializeField] private ConfigManager configManager = null;

        private static Root _instance;

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            profileManager.Init(battleManager, cacheManager, configManager);
            levelView.Init(battleManager);
            battleManager.Init(inputController);
            battleManager.OnBattleFinished += () => GoMeta();
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
                IPromise battleUIProcessingPromise = uiManager.ShowHUD(battleManager, cameraController, inputController, profileManager);
                battleUIProcessingPromise.Done(() => battleManager.Clean());
                int battleCount = profileManager.GetBattleCount();
                var aiUnitStates = configManager.EnemyUnits.GetAIUnitsForBattleIndex(battleCount);
                battleManager.StartBattle(selectedUnits, aiUnitStates);
            }
        }
    }
}
