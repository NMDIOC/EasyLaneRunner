using UnityEngine;

namespace NMDIOC.LaneRunner
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Prefabs to Spawn")]
        public GameObject[] obstaclePrefabs; 
        public GameObject[] coinPrefabs; 

        [Header("Generator Settings")]
        public float laneDistance = 3.0f;    
        public float spawnInterval = 1.5f;   
        public float objectSpeed = 10.0f;    
        public float spawnZPosition = 40.0f; 

        private float timer;
        private bool isGameActive = true;

        void OnEnable()
        {
            LaneRunnerController.OnPlayerDeath += StopSpawning;
        }

        void OnDisable()
        {
            LaneRunnerController.OnPlayerDeath -= StopSpawning;
        }

        void StopSpawning()
        {
            isGameActive = false;
        }

        void Update()
        {
            if (!isGameActive) return; 

            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                SpawnObject();
                timer = 0f;
            }
        }

        void SpawnObject()
        {
            if (obstaclePrefabs.Length == 0 || coinPrefabs.Length == 0) return;

            bool isCoin = Random.value < 0.3f;
            GameObject prefabToSpawn = isCoin ? 
                coinPrefabs[Random.Range(0, coinPrefabs.Length)] : 
                obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            int randomLane = Random.Range(0, 3);
            float spawnXPosition = (randomLane - 1) * laneDistance;

            Vector3 spawnPosition = new Vector3(spawnXPosition, transform.position.y, transform.position.z + spawnZPosition);
            GameObject newObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            if (!isCoin) 
            {
                MovingObject movingScript = newObject.AddComponent<MovingObject>();
                movingScript.speed = objectSpeed;
            }
            else 
            {
                Coin coinScript = newObject.GetComponent<Coin>();
                if (coinScript != null)
                {
                    coinScript.speed = objectSpeed;
                }
            }
        }
    }

    // ========================================================================
    // COMPONENTE AUXILIAR MODIFICADO PARA DETENERSE AL MORIR
    // ========================================================================
    public class MovingObject : MonoBehaviour
    {
        public float speed;

        void OnEnable()
        {
            // Escucha el evento de muerte para congelarse
            LaneRunnerController.OnPlayerDeath += Freeze;
        }

        void OnDisable()
        {
            // Limpieza de suscripción para evitar fugas de memoria
            LaneRunnerController.OnPlayerDeath -= Freeze;
        }

        void Freeze()
        {
            speed = 0f; // La velocidad cae a cero inmediatamente
        }

        void Update()
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
            
            // Solo se destruyen si siguen avanzando (evita que se borren al congelarse en pantalla)
            if (speed > 0f && transform.position.z < -10f) 
            {
                Destroy(gameObject);
            }
        }
    }
}