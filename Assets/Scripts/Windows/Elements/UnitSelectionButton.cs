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
        [SerializeField] private Image image = null;

        public UnitState UnitState { get; private set; }

        private float clickTimer;
        private bool activatingClick;
        private const float clickMaxDuration = .5f;

        public void HandlePointerDown()
        {
            activatingClick = true;
            clickTimer = 0;
        }

        public void HandlePointerUp()
        {
            if (activatingClick && clickTimer < clickMaxDuration)
            {
                OnClicked?.Invoke(this);
            }
            activatingClick = false;
        }

        public void Init(UnitState unitState)
        {
            //Clear previous subscriptions
            OnClicked = null;
            this.UnitState = unitState;
            SetFrameVisible(false);

            image.sprite = Root.Configs.UnitSprites.GetSpriteForUnit(unitState.unitType);
        }

        public void SetFrameVisible(bool isVisible)
        {
            selectionFrame.SetActive(isVisible);
        }

        public string GetTooltipText()
        {
            string levelLabel = $"level: {UnitState.unitParams.level}";
            string experienceLabel = $"experience: {UnitState.unitParams.experience}";
            string attackLabel = $"attack: {UnitState.unitParams.attack}";
            string maxHealthLabel = $"health: {UnitState.unitParams.maxHealth}";
            return $"{UnitState.unitType}\n{levelLabel}\n{experienceLabel}\n{attackLabel}\n{maxHealthLabel}";
        }

        private void Update()
        {
            if (activatingClick)
            {
                clickTimer += Time.deltaTime;
            }
        }

    }
}
