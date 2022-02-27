using System.Collections;
using System.Collections.Generic;
using Tactics.Battle;
using Tactics.Helpers.ObjectPool;
using Tactics.Windows.Elements;
using UnityEngine;

namespace Tactics.Windows
{
    public class BattleHUD : MonoBehaviour
    {
        [SerializeField] private Healthbar healthbarPrefab = null;
        [SerializeField] private RectTransform healthbarContainerRect = null;

        public float multiplier;

        private Dictionary<UnitShell, Healthbar> unitsHealth;
        private CameraController cameraController;
        private BattleManager battleManager;
        private InputController inputController;

        public void Init(Battle.BattleManager battleManager, CameraController cameraController, InputController inputController)
        {
            this.cameraController = cameraController;
            this.battleManager = battleManager;
            this.inputController = inputController;
            battleManager.OnBattleInit += HandleBattleInit;
            inputController.OnUnitLongTap += HandleUnitLongTap;
            unitsHealth = new Dictionary<UnitShell, Healthbar>();
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
        }

        void LateUpdate()
        {
            foreach (KeyValuePair<UnitShell, Healthbar> kvp in unitsHealth)
            {
                var unit = kvp.Key;
                Healthbar healthbar = kvp.Value;
                Vector2 screenPoint = cameraController.WorldToScreenPoint(unit.transform.position + Vector3.up * 0.6f);
                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(healthbarContainerRect, screenPoint, null, out localPoint))
                {
                    healthbar.transform.localPosition = localPoint;
                }
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
