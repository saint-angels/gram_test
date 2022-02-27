using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Windows.Tooltips
{
    public class TooltipController : MonoBehaviour
    {
        [SerializeField] private Tooltip tooltip;

        private static TooltipController instance;

        private RectTransform tooltipsContainerRect;

        void Awake()
        {
            instance = this;
            tooltipsContainerRect = GetComponent<RectTransform>();

            tooltip.Init();
        }

        public static void ShowTooltip(RectTransform targetRect, string text)
        {
            instance.ShowTooltipInternal(targetRect, text);
        }

        public static void ShowTooltipForSceneObject(Vector3 positionOnCanvas, string text)
        {
            instance.tooltip.ShowForSceneObject(positionOnCanvas, instance.tooltipsContainerRect, text);
        }

        private void ShowTooltipInternal(RectTransform targetRect, string text)
        {
            // RectTransformUtil.SnapToParent(tooltipsContainerRect, windowRect);
            tooltip.ShowForUIElement(targetRect, tooltipsContainerRect, text);
            // transform.SetAsLastSibling();
        }

    }
}
