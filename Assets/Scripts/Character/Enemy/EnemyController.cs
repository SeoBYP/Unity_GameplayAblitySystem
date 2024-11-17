using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        private Rigidbody rb;

        [SerializeField] private EnemyWaipoints[] waipoints;

        [SerializeField] private float normalAcceleration = 1;
        [SerializeField] private float normalDeceleration = 5;
        [SerializeField] private float walkSpeed = 5;

        [HideInInspector] public float acceleration;
        [HideInInspector] public float deceleration;

        [SerializeField] private Transform target;
        private Vector3 moveDirection = Vector3.zero;
        private int currentWaipoint = 0;
        
        [SerializeField] private bool isStuned = false;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            acceleration = normalAcceleration;
            deceleration = normalDeceleration;
            if (waipoints.Length > 0)
                SetDestination(waipoints[currentWaipoint].transform);
        }

        private void FixedUpdate()
        {
            if(target == null) 
                return;
            if(isStuned)
                return;
            // moveDirection = target.position - transform.position;
            // moveDirection.y = 0;
            Vector3 newVelocity = moveDirection * walkSpeed * Time.deltaTime;
            if (newVelocity.magnitude > 0)
            {
                rb.velocity = newVelocity * acceleration;
            }
            else
            {
                rb.velocity = newVelocity * deceleration;
            }
        }
        
        public void SetDestination(Transform destination)
        {
            target = destination;
        }

        public void SetVelocity(Vector3 velocity)
        {
            rb.velocity = velocity;
        }

        
        public void SetStun(bool state)
        {
            isStuned = state;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}