using UnityEngine;
using System;

namespace NMDIOC.LaneRunner
{
    public class Coin : MonoBehaviour
    {
        public static event Action OnCoinCollected;
        
        [Header("Movement")]
        public float speed = 10f; 
        public float rotationSpeed = 100f;

        void Update()
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
            if (transform.position.z < -10f) Destroy(gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("P"))
            {
                OnCoinCollected?.Invoke();
                Destroy(gameObject);
            }
        }

        void OnEnable()
        {
            LaneRunnerController.OnPlayerDeath += FreezeCoin;
        }

        void OnDisable()
        {
            LaneRunnerController.OnPlayerDeath -= FreezeCoin;
        }

        void FreezeCoin()
        {
            speed = 0f;
            rotationSpeed = 0f;
        }
    }
}
