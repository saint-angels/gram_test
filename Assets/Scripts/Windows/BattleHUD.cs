using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tactics.Battle;
using Tactics.Helpers.ObjectPool;
using Tactics.Helpers.Promises;
using Tactics.SharedData;
using Tactics.Windows.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics.Windows
{
    public class BattleHUD : MonoBehaviour
    {
        [SerializeField] private Healthbar healthbarPrefab = null;
        [SerializeField] private Text paramIncreaseLabelPrefab = null;
        [SerializeField] private RectTransform healthbarContainerRect = null;

        public float multiplier;

        private Dictionary<UnitShell, Healthbar> unitsHealth;
        private CameraController cameraController;
        private BattleManager battleManager;
        private InputController inputController;
        private ProfileManager profileManager;

        private Deferred battleProcessDeferred;

        public IPromise Init(Battle.BattleManager battleManager, CameraController cameraController, InputController inputController, ProfileManager profileManager)
        {
            this.cameraController = cameraController;
            this.battleManager = battleManager;
            this.inputController = inputController;
            this.profileManager = profileManager;

            battleManager.OnBattleInit += HandleBattleInit;
            inputController.OnUnitLongTap += HandleUnitLongTap;
            profileManager.OnUnitsParamUpgrade += HandleUnitsParamUpgrade;
            unitsHealth = new Dictionary<UnitShell, Healthbar>();
            battleProcessDeferred = Deferred.GetFromPool();

            return battleProcessDeferred;
        }

        private void HandleUnitsParamUpgrade(List<UnitState> unitStateDelta)
        {
            Sequence mainSeq = DOTween.Sequence();
            foreach (var stateDelta in unitStateDelta)
            {
                foreach (KeyValuePair<UnitShell, Healthbar> kvp in unitsHealth)
                {
                    UnitShell unit = kvp.Key;
                    if (unit.UnitType == stateDelta.unitType)
                    {
                        Vector2? localPoint = GetLocalUIPointForWorld(unit.transform.position, 1f);
                        if (localPoint.HasValue)
                        {
                            Sequence unitSequence = DOTween.Sequence();
                            TryHandleParamDelta(stateDelta.unitParams.attack, "attack", localPoint.Value, unitSequence);
                            TryHandleParamDelta(stateDelta.unitParams.maxHealth, "HP", localPoint.Value, unitSequence);
                            TryHandleParamDelta(stateDelta.unitParams.experience, "experience", localPoint.Value, unitSequence);
                            TryHandleParamDelta(stateDelta.unitParams.level, "level", localPoint.Value, unitSequence);
                            //Make all unit sequences run in parallel
                            mainSeq.Insert(0, unitSequence);
                        }
                        break;
                    }
                }
            }
            mainSeq.OnComplete(() => battleProcessDeferred.Resolve());

            void TryHandleParamDelta(int paramDelta, string paramName, Vector2 uiLocalPoint, Sequence seq)
            {
                if (0 < paramDelta)
                {
                    // print($"{paramName} +{paramDelta}!");
                    Text paramIncreaseLabel = ObjectPool.Spawn(paramIncreaseLabelPrefab, uiLocalPoint, Quaternion.identity, transform, true);
                    //hide the label and show only when it starts moving
                    paramIncreaseLabel.gameObject.SetActive(false);
                    seq.AppendCallback(() =>
                    {
                        paramIncreaseLabel.gameObject.SetActive(true);
                    });
                    paramIncreaseLabel.text = $"{paramName} +{paramDelta}!";
                    var tween = paramIncreaseLabel.rectTransform.DOLocalMoveY(uiLocalPoint.y + 10f, 1f);
                    tween.OnComplete(() =>
                    {
                        ObjectPool.Despawn(paramIncreaseLabel, true);
                    });
                    seq.Append(tween);
                }
            }
        }

        public void Clear()
        {
            if (unitsHealth != null)
            {
                foreach (KeyValuePair<UnitShell, Healthbar> kvp in unitsHealth)
                {
                    Healthbar healthbar = kvp.Value;
                    ObjectPool.Despawn(healthbar, true);
                }
                unitsHealth.Clear();
            }

            if (battleManager != null)
            {
                battleManager.OnBattleInit -= HandleBattleInit;
            }

            if (inputController != null)
            {
                inputController.OnUnitLongTap -= HandleUnitLongTap;
            }
            if (profileManager != null)
            {
                profileManager.OnUnitsParamUpgrade -= HandleUnitsParamUpgrade;
            }
        }

        void LateUpdate()
        {
            foreach (KeyValuePair<UnitShell, Healthbar> kvp in unitsHealth)
            {
                var unit = kvp.Key;
                Healthbar healthbar = kvp.Value;
                Vector2? localPoint = GetLocalUIPointForWorld(unit.transform.position, 0.6f);
                if (localPoint.HasValue)
                {
                    healthbar.transform.localPosition = localPoint.Value;
                }
            }
        }

        private Vector2? GetLocalUIPointForWorld(Vector3 position, float offset)
        {
            Vector2 screenPoint = cameraController.WorldToScreenPoint(position + Vector3.up * offset);
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(healthbarContainerRect, screenPoint, null, out localPoint))
            {
                return localPoint;
            }
            else
            {
                return null;
            }
        }

        private void HandleUnitLongTap(UnitShell unit)
        {
            string text = unit.GetTooltipText();
            Vector2 screenPoint = cameraController.WorldToScreenPoint(unit.transform.position);
            Tooltips.TooltipController.ShowTooltipForSceneObject(screenPoint, text);
        }

        private void HandleBattleInit(List<UnitShell> userUnits, List<UnitShell> enemyUnits)
        {
            InitForUnits(userUnits);
            InitForUnits(enemyUnits);

            void InitForUnits(List<UnitShell> units)
            {
                for (int i = 0; i < units.Count; i++)
                {
                    UnitShell unit = units[i];
                    Healthbar healthBar = ObjectPool.Spawn(healthbarPrefab, Vector3.zero, Quaternion.identity, transform, true);
                    healthBar.SetValue(unit.HealthState.Value, unit.Params.maxHealth);
                    unitsHealth.Add(unit, healthBar);

                    unit.HealthState.OnValueChanged += (healthValue) =>
                    {
                        unitsHealth[unit].SetValue(healthValue, unit.Params.maxHealth);
                    };
                    unit.OnDeath += (deadUnit) =>
                    {
                        ObjectPool.Despawn(unitsHealth[deadUnit], true);
                        unitsHealth.Remove(deadUnit);
                    };
                }
            }
        }

    }
}
