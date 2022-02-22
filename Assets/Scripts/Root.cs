using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Tactics
{
    public class Root : MonoBehaviour
    {
        private static Root _instance;

        void Awake()
        {
            _instance = this;
            DG.Tweening.DOTween.SetTweensCapacity(500, 100);
        }

        void Start()
        {
            print(gameObject.name);
        }
    }
}
