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
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    UnitShell unit = hit.collider.GetComponent<UnitShell>();
                    if (unit != null)
                    {
                        OnUnitClick?.Invoke(unit);
                    }
                }
            }
        }
    }
}
