using System.Collections;
using System.Collections.Generic;
using Tactics.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tactics.Windows.Tooltips
{
    public class TooltipTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] GameObject textProvider;

        private void Awake()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ITooltipTextProvider provider = this.textProvider.GetComponent<ITooltipTextProvider>();
            string text = provider.GetTooltipText();
            // Debug.Log(name + "pointer down");
            Debug.Log(name + "showing tooltip:" + text);
            // TooltipsController.ShowTooltip(windowRectTransform, rectTransform, title, text);

            RectTransform rectTransform = GetComponent<RectTransform>();

            TooltipController.ShowTooltip(rectTransform, text);
        }

        public void OnPointerUp(PointerEventData pointerEventData)
        {
            // Debug.Log(name + "pointer up");
        }
    }
}
