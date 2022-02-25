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
        }

        public static void ShowTooltip(RectTransform targetRect, string text)
        {
            instance.ShowTooltipInternal(targetRect, text);
        }

        private void ShowTooltipInternal(RectTransform targetRect, string text)
        {
            // RectTransformUtil.SnapToParent(tooltipsContainerRect, windowRect);

            tooltip.Initialize(targetRect, tooltipsContainerRect, text);
            tooltip.Show();
            // transform.SetAsLastSibling();
        }

    }
}