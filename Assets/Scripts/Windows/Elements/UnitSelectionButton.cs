using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.Interfaces;
using Tactics.SharedData;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics.Windows.Elements
{
    public class UnitSelectionButton : MonoBehaviour, ITooltipTextProvider
    {
        public event Action<UnitSelectionButton> OnClicked;

        [SerializeField] private GameObject selectionFrame = null;
        [SerializeField] private Button button = null;

        public UnitState UnitState { get; private set; }

        private void Awake()
        {
            button.onClick.AddListener(() =>
            {
                OnClicked?.Invoke(this);
            });
        }

        public void Init(UnitState unitState)
        {
            //Clear previous subscriptions
            OnClicked = null;
            this.UnitState = unitState;
            SetFrameVisible(false);
        }

        public void SetFrameVisible(bool isVisible)
        {
            selectionFrame.SetActive(isVisible);
        }

        public string GetTooltipText()
        {

            return $"{UnitState.unitType}\nlevel:{UnitState.unitParams.level}\nattack:{UnitState.unitParams.level}\nexperience:{UnitState.unitParams.level}";
        }
    }
}
