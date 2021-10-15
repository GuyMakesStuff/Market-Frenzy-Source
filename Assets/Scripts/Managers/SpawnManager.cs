using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarketFrenzy.Managers
{
    public class SpawnManager : Manager<SpawnManager>
    {
        [System.Serializable]
        public class ObstacleType
        {
            [Range(0f, 1f)]
            public float MinRatio;
            public GameObject Prefab;
        }
        public ObstacleType[] Obstacles;

        [Header("Difficulty")]
        public float StartSpawnDelay;
        public float SpawnDelayDecreaseAmountPerRound;
        public float MinSpawnDelay;
        float SpawnTimer;
        float SpawnDelay;

        [Header("Spawn Placement")]
        public float SpawnHeight;
        public float SpawnRange;
        Vector3 BaseSpawnPos
        {
            get { return transform.position; }
        }

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            SpawnDelay = StartSpawnDelay;
            SpawnTimer = SpawnDelay;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            SpawnTimer -= Time.deltaTime;
            if (SpawnTimer <= 0f)
            {
                Spawn();
                SpawnTimer = SpawnDelay;
            }
        }
        void Spawn()
        {
            Vector3 SpawnPos = BaseSpawnPos + new Vector3(Random.Range(-SpawnRange, SpawnRange), SpawnHeight, 0f);
            float Ratio = Random.Range(0f, 1f);
            GameObject NewObstacle = null;
            for (int O = 0; O < Obstacles.Length; O++)
            {
                ObstacleType NextObstacle = Obstacles[O + 1];
                if (Ratio >= Obstacles[O].MinRatio && Ratio <= NextObstacle.MinRatio)
                {
                    NewObstacle = Obstacles[O].Prefab;
                    break;
                }
            }
            Instantiate(NewObstacle, SpawnPos, Random.rotation);
        }

        public void DecreaseSpawnDelay()
        {
            if (SpawnDelay > MinSpawnDelay)
            {
                SpawnDelay -= SpawnDelayDecreaseAmountPerRound;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(BaseSpawnPos + new Vector3(-SpawnRange, SpawnHeight, 0f), BaseSpawnPos + new Vector3(SpawnRange, SpawnHeight, 0f));
        }
    }
}