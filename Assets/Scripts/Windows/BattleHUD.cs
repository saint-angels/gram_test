using System.Collections;
using System.Collections.Generic;
using Tactics.Battle;
using Tactics.Helpers.ObjectPool;
using Tactics.Windows.Elements;
using UnityEngine;

namespace Tactics
{
    public class BattleHUD : MonoBehaviour
    {
        [SerializeField] private Healthbar healthbarPrefab = null;
        [SerializeField] private RectTransform healthbarContainerRect = null;
        public float multiplier;

        private Dictionary<UnitShell, Healthbar> unitsHealth;

        private CameraController cameraController;

        public void Init(Battle.BattleManager battleManager, CameraController cameraController)
        {
            this.cameraController = cameraController;
            unitsHealth = new Dictionary<UnitShell, Healthbar>();

            battleManager.OnBattleInit += (userUnits, enemyUnits) =>
            {
                InitForUnits(userUnits);
                InitForUnits(enemyUnits);

                void InitForUnits(List<UnitShell> units)
                {
                    foreach (var unit in units)
                    {
                        Healthbar healthBar = ObjectPool.Spawn(healthbarPrefab, Vector3.zero, Quaternion.identity, transform, true);
                        healthBar.SetValue(unit.HealthState.Value, unit.MaxHealth);
                        unitsHealth.Add(unit, healthBar);

                        unit.HealthState.OnValueChanged += (healthValue) =>
                        {
                            unitsHealth[unit].SetValue(healthValue, unit.MaxHealth);
                        };
                        unit.OnDeath += (deadUnit) =>
                        {
                            ObjectPool.Despawn(unitsHealth[deadUnit]);
                            unitsHealth.Remove(deadUnit);
                        };
                    }
                }
            };

        }

        void LateUpdate()
        {
            foreach (KeyValuePair<UnitShell, Healthbar> kvp in unitsHealth)
            {
                var unit = kvp.Key;
                Healthbar healthbar = kvp.Value;
                Vector2 screenPoint = cameraController.WorldToScreenPoint(unit.transform.position + Vector3.up * multiplier);
                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(healthbarContainerRect, screenPoint, null, out localPoint))
                {
                    healthbar.transform.localPosition = localPoint;
                }
            }
        }
    }
}
