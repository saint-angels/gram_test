using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Windows.Tooltips
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private float yOffset = 30f;
        [SerializeField] private UnityEngine.UI.Text tooltipText;

        private Canvas canvas;
        private RectTransform rectTransform;
        private RectTransform ownerRectTransform;
        private RectTransform containerRectTransform;

        public void Init()
        {
            canvas = GetComponentInParent<Canvas>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(RectTransform ownerRectTransform, RectTransform containerRect, string tooltipText)
        {
            this.ownerRectTransform = ownerRectTransform;
            this.containerRectTransform = containerRect;

            this.tooltipText.text = tooltipText;
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

            SetupTooltipPosition(ownerRectTransform, rectTransform);
            gameObject.SetActive(true);
        }

        private void SetupTooltipPosition(RectTransform ownerRectTransform, RectTransform tooltipRectTransform)
        {
            Vector2 halfContainerSize = containerRectTransform.rect.size / 2f;
            Vector3 positionOnCanvas = canvas.transform.InverseTransformPoint(ownerRectTransform.position);
            Vector2 tooltipHalfSize = tooltipRectTransform.rect.size / 2;
            Vector2 ownerHalfSize = ownerRectTransform.rect.size / 2f;
            Vector2 tooltipPosition = tooltipRectTransform.anchoredPosition;

            tooltipPosition.y = positionOnCanvas.y + ownerHalfSize.y + tooltipHalfSize.y + yOffset;
            tooltipPosition.x = positionOnCanvas.x;

            if ((tooltipPosition.y + tooltipHalfSize.y) > halfContainerSize.y) // out of bounds on y
            {
                tooltipPosition.y = positionOnCanvas.y - ownerHalfSize.y - tooltipHalfSize.y - yOffset;
            }

            float xSide = Mathf.Abs(tooltipPosition.x) + tooltipHalfSize.x;
            if (xSide > halfContainerSize.x) // ouf of bounds on x
            {
                float diff = xSide - halfContainerSize.x;
                tooltipPosition.x += diff * (-Mathf.Sign(tooltipPosition.x));
            }

            tooltipRectTransform.anchoredPosition = tooltipPosition;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
