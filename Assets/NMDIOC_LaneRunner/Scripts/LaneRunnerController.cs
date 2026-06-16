using UnityEngine;
using System;

namespace NMDIOC.LaneRunner
{
    public class LaneRunnerController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float laneDistance = 3.0f;
        [SerializeField] private float moveSpeed = 10.0f;   
        [SerializeField] private float jumpForce = 5.0f;    
        
        private int currentLane = 1; 
        private Vector3 targetPosition;
        private Rigidbody rb;
        private bool isGrounded = true;
        private bool isDead = false;

        public static event Action OnPlayerDeath;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            targetPosition = transform.position;
        }

        void Update()
        {
            if (isDead) return;

            if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
            {
                currentLane--;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
            {
                currentLane++;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }

            targetPosition = new Vector3((currentLane - 1) * laneDistance, transform.position.y, transform.position.z);
        }

        void FixedUpdate()
        {
            if (isDead) return;

            Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            newPos.y = transform.position.y;
            transform.position = newPos;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
            
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            Debug.Log("GAME OVER! The player has collided with an obstacle.");
            
            OnPlayerDeath?.Invoke();

            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}