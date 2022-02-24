using System.Collections;
using System.Collections.Generic;
using Tactics.SharedData;
using Tactics.Windows;
using UnityEngine;

namespace Tactics
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UnitSelectionWindow unitSelectionWindow = null;
        [SerializeField] private BattleHUD battleHUD = null;

        // public void Init(Root root)
        // {
        // }

        public void ShowHUD(Battle.BattleManager battleManager, CameraController cameraController)
        {
            unitSelectionWindow.Clear();
            unitSelectionWindow.gameObject.SetActive(false);


            battleHUD.Init(battleManager, cameraController);
            battleHUD.gameObject.SetActive(true);
        }

        public UnitSelectionWindow ShowUnitSelection(UnitState[] availableUnits)
        {
            battleHUD.gameObject.SetActive(false);
            battleHUD.Clear();

            unitSelectionWindow.gameObject.SetActive(true);
            unitSelectionWindow.Init(availableUnits);
            return unitSelectionWindow;
        }
    }
}
