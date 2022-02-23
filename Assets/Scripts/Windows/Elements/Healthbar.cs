using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Windows.Elements
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Slider slider = null;

        public void SetValue01(float value)
        {
            slider.value = value;
        }

        public void SetValue(int healthValue, int maxHealth)
        {
            float healthFraction = (float)healthValue / (float)maxHealth;
            slider.value = healthFraction;
        }

    }
}
