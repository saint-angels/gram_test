using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tactics.Battle;
using UnityEngine;

namespace Tactics.View
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private new SpriteRenderer renderer;

        private UnitShell unitShell;

        private void Awake()
        {
            unitShell = GetComponent<UnitShell>();
            unitShell.OnInit += (unitType) =>
            {
                var sprite = Root.Configs.UnitSprites.GetSpriteForUnit(unitType);
                renderer.sprite = sprite;
            };

            unitShell.OnAttack += (attacker, damage, deferred) =>
            {
                float attackOffset = unitShell.Faction == Faction.User ? 1f : -1f;
                float attackPoint = transform.position.x + attackOffset;
                var tween = transform.DOMoveX(attackPoint, .3f)
                                        .SetEase(Ease.InFlash, overshoot: 2);
                tween.OnComplete(() =>
                {
                    deferred.Resolve();
                });
            };
        }
    }
}
