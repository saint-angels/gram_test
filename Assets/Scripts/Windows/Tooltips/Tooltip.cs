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
        private RectTransform containerRectTransform;

        private float visibleTimer;
        private bool isVisible;

        public void Init()
        {
            canvas = GetComponentInParent<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            isVisible = false;
        }

        public void ShowForSceneObject(Vector3 uiPosition, RectTransform containerRect, string tooltipText)
        {
            ShowInternal(uiPosition, Vector2.zero, containerRect, tooltipText);
        }

        public void Show(RectTransform ownerRectTransform, RectTransform containerRect, string tooltipText)
        {
            ShowInternal(ownerRectTransform.position, ownerRectTransform.rect.size, containerRect, tooltipText);
        }

        private void ShowInternal(Vector3 ownerRectPos, Vector2 ownerRectSize, RectTransform containerRect, string tooltipText)
        {
            this.containerRectTransform = containerRect;

            this.tooltipText.text = tooltipText;
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

            SetupTooltipPosition(ownerRectPos, ownerRectPos, rectTransform);
            gameObject.SetActive(true);

            isVisible = true;
            visibleTimer = 2f;
        }

        private void SetupTooltipPosition(Vector3 ownerRectPos, Vector2 ownerRectSize, RectTransform tooltipRectTransform)
        {
            Vector2 halfContainerSize = containerRectTransform.rect.size / 2f;
            Vector3 positionOnCanvas = canvas.transform.InverseTransformPoint(ownerRectPos);
            Vector2 tooltipHalfSize = tooltipRectTransform.rect.size / 2;
            Vector2 ownerHalfSize = ownerRectSize / 2f;
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

        void Update()
        {
            if (isVisible)
            {
                visibleTimer -= Time.deltaTime;
                if (visibleTimer <= 0)
                {
                    Hide();
                }
            }

        }
    }
}
