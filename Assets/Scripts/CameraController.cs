using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private new Camera camera = null;

        public Vector2 WorldToScreenPoint(Vector3 position)
        {
            return camera.WorldToScreenPoint(position);
        }
    }
}
