using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.Battle;
using Tactics.Helpers.ObjectPool;
using Tactics.SharedData;
using Tactics.Windows.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics.Windows
{
    public class UnitSelectionWindow : MonoBehaviour
    {
        public event Action<UnitState[]> OnUnitsSelected;

        [SerializeField] private UnitSelectionButton unitSelectionButtonPrefab = null;
        [SerializeField] private RectTransform unitSelectionButtonContainer = null;
        [SerializeField] private Button startBattleButton = null;

        private List<UnitSelectionButton> unitSelectionButtons = new List<UnitSelectionButton>();
        private List<UnitSelectionButton> selectedButtons = new List<UnitSelectionButton>();

        private void Awake()
        {
            startBattleButton.onClick.AddListener(() =>
            {
                var selectedUnits = new UnitState[selectedButtons.Count];
                for (int i = 0; i < selectedButtons.Count; i++)
                {
                    UnitSelectionButton button = selectedButtons[i];
                    selectedUnits[i] = button.UnitState;
                }
                OnUnitsSelected(selectedUnits);
            });
        }

        public void Init(UnitState[] availableUnits)
        {
            for (int i = 0; i < availableUnits.Length; i++)
            {
                UnitState unitState = availableUnits[i];

                var button = ObjectPool.Spawn(unitSelectionButtonPrefab, Vector3.zero, Quaternion.identity, unitSelectionButtonContainer, true);
                button.Init(unitState);
                button.OnClicked += (clickedButton, clickedUnitState) =>
                {
                    bool isSelectingUnit = selectedButtons.Contains(clickedButton) == false && selectedButtons.Count < 3;
                    if (isSelectingUnit)
                    {
                        selectedButtons.Add(clickedButton);
                    }
                    else
                    {
                        selectedButtons.Remove(clickedButton);
                    }
                    clickedButton.SetFrameVisible(isSelectingUnit);
                    startBattleButton.interactable = selectedButtons.Count == 3;
                };
            }
        }

        public void Clear()
        {
            //Clear previously spawned buttons
            foreach (var button in unitSelectionButtons)
            {
                ObjectPool.Despawn(button, true);
            }
            selectedButtons.Clear();

            OnUnitsSelected = null;
        }
    }
}
