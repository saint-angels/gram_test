using System.Collections;
using System.Collections.Generic;
using Tactics.Helpers.Promises;
using Tactics.SharedData;
using Tactics.Windows;
using UnityEngine;

namespace Tactics
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UnitSelectionWindow unitSelectionWindow = null;
        [SerializeField] private BattleHUD battleHUD = null;

        public IPromise ShowHUD(Battle.BattleManager battleManager, CameraController cameraController, InputController inputController, ProfileManager profile)
        {
            unitSelectionWindow.Clear();
            unitSelectionWindow.gameObject.SetActive(false);

            IPromise battleProcessingPromise = battleHUD.Init(battleManager, cameraController, inputController, profile);
            battleHUD.gameObject.SetActive(true);
            return battleProcessingPromise;
        }

        public UnitSelectionWindow ShowUnitSelection(UnitState[] availableUnits)
        {
            battleHUD.Clear();
            battleHUD.gameObject.SetActive(false);

            unitSelectionWindow.gameObject.SetActive(true);
            unitSelectionWindow.Init(availableUnits);
            return unitSelectionWindow;
        }
    }
}
