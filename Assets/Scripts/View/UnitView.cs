using System.Collections;
using System.Collections.Generic;
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
        }
    }
}
