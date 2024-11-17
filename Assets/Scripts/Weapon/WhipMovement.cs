using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class WhipMovement : MonoBehaviour
{
    private Transform Owner = null;
    private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float timeSpeedUpFactor = 2.0f;
    private float coolTime = 5.0f;
    private float currentTime = 0;
    private bool start = true;
private EnemyController _enemyController = null;
    public void SetShow()
    {
        this.gameObject.SetActive(true);
    }

    public void SetHide()
    {
        this.gameObject.SetActive(false);
        _enemyController = null;
    }

    public void SetTimer(float timer)
    {
        coolTime = timer;
    }

    public void SetOwner(Transform owner)
    {
        Owner = owner;
        transform.position = Owner.position;
        currentTime = 0;
        start = true;
    }

    public void SetTarget(Transform target)
    {
        targetPosition = target.position;
    }

    public void SetDestination(Vector3 destination)
    {
        targetPosition = destination;
    }

    public void Update()
    {
        if (!start) return;
        currentTime += Time.deltaTime;
        if (currentTime <= coolTime / 2)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition,
                coolTime * timeSpeedUpFactor * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, Owner.position,
                coolTime * timeSpeedUpFactor * Time.deltaTime);
            if (_enemyController)
            {
                _enemyController.SetPosition(transform.position);
            }
        }

        if (currentTime >= coolTime)
        {
            SetHide();
            start = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemyController = other.GetComponentInParent<EnemyController>();
            if (!_enemyController)
            {
                Debug.LogError($"{nameof(EnemyController)}를 가져올 수 없습니다.");
                return;
            }

            currentTime = coolTime / 2;
            _enemyController.SetStun(true);
        }
    }
}