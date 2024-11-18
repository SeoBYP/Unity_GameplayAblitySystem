using System;
using UnityEngine;

namespace UnityGUI
{
    public class LookAtCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (Camera.main != null) transform.LookAt(Camera.main.transform);
        }
    }
}