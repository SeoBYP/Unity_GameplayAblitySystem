using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyWaipoints : MonoBehaviour
{
    [SerializeField] private Color debugColor = Color.red;
    [SerializeField] private float radius = 2;
    private void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
