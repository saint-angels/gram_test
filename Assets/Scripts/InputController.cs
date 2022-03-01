using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.Battle;
using UnityEngine;

namespace Tactics
{
    public class InputController : MonoBehaviour
    {
        public event Action<UnitShell> OnUnitClick;
        public event Action<UnitShell> OnUnitLongTap;

        private float buttonDownTimer;
        // private bool activatingClick;
        private UnitShell unitInFocus = null;
        private const float clickMaxDuration = 3f;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                UnitShell unit = TryGetUnitAtPoint(Input.mousePosition);
                if (unit != null)
                {
                    unitInFocus = unit;
                    buttonDownTimer = 0;
                }
            }
            else if (Input.GetMouseButtonUp(0) && unitInFocus != null)
            {
                UnitShell unit = TryGetUnitAtPoint(Input.mousePosition);
                if (unit != null)
                {
                    OnUnitClick?.Invoke(unit);
                    unitInFocus = null;
                }
            }

            if (unitInFocus != null)
            {
                buttonDownTimer += Time.deltaTime;
                if (clickMaxDuration < buttonDownTimer)
                {
                    OnUnitLongTap?.Invoke(unitInFocus);
                    unitInFocus = null;
                }
            }

            UnitShell TryGetUnitAtPoint(Vector3 position)
            {
                Ray ray = Camera.main.ScreenPointToRay(position);
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    UnitShell unit = hit.collider.GetComponent<UnitShell>();
                    //unit could be null
                    return unit;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
