using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.SharedData;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics.Windows.Elements
{
    public class UnitSelectionButton : MonoBehaviour
    {
        public event Action<UnitSelectionButton, UnitState> OnClicked;

        [SerializeField] private GameObject selectionFrame = null;
        [SerializeField] private Button button = null;

        public UnitState UnitState { get; private set; }

        private void Awake()
        {
            button.onClick.AddListener(() =>
            {
                OnClicked?.Invoke(this, UnitState);
            });
        }

        public void Init(UnitState unitState)
        {
            this.UnitState = unitState;
            SetFrameVisible(false);
        }

        public void SetFrameVisible(bool isVisible)
        {
            selectionFrame.SetActive(isVisible);
        }
    }
}
