using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Battle
{
    public class UnitShell : MonoBehaviour
    {

        public event Action<UnitShell, UnitShell, int> OnAttack;

        public void Init()
        {

        }
    }
}
