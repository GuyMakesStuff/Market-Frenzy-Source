using System.Collections;
using MarketFrenzy.Gameplay.ConveyorItems;
using UnityEngine;
using MarketFrenzy.Managers;

namespace MarketFrenzy.Gameplay
{
    public class Conveyor : Manager<Conveyor>
    {
        [Header("Scrool")]
        public Material ConvayorMat;
        float Scrool;
        public float StartSpeed;
        float Speed;
        public float ScroolSpeedDevider;

        [Header("Objects")]
        public Transform SpawnPoint;
        public GameObject[] RegularItemPrefabs;
        public GameObject BombPrefab;
        public float StartSpawnDelay;
        public int MinRegularSpawnCount;
        public int MaxRegularSpawnCount;
        public float RoundRatio;
        float SpawnDelay;
        float SpawnTimer;
        int RegularSpawnCount;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            Speed = StartSpeed;
            SpawnDelay = StartSpawnDelay;
            RegularSpawnCount = Random.Range(MinRegularSpawnCount, MaxRegularSpawnCount + 1);
            ConvayorMat.SetTextureOffset("_MainTex", Vector2.zero);
            ConvayorMat.SetTextureOffset("_DetailAlbedoMap", Vector2.zero);
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            Scrool -= (Speed / ScroolSpeedDevider) * Time.deltaTime;
            ConvayorMat.SetTextureOffset("_MainTex", new Vector2(Scrool, 0));
            ConvayorMat.SetTextureOffset("_DetailAlbedoMap", new Vector2(Scrool, 0));

            SpawnTimer -= Time.deltaTime;
            if (SpawnTimer <= 0f)
            {
                SpawnTimer = SpawnDelay;
                if (RegularSpawnCount == 1)
                {
                    RegularSpawnCount = Random.Range(MinRegularSpawnCount, MaxRegularSpawnCount + 1);
                    Spawn(BombPrefab);
                }
                else
                {
                    RegularSpawnCount--;
                    GameObject NewItem = RegularItemPrefabs[Random.Range(0, RegularItemPrefabs.Length)];
                    Spawn(NewItem);
                }
            }
        }
        void Spawn(GameObject Prefab)
        {
            GameObject NewItem = Instantiate(Prefab, SpawnPoint.position, Prefab.transform.rotation);
            NewItem.GetComponent<ConveyorItem>().Speed = Speed;
        }

        public void EncreaseValues()
        {
            Speed *= RoundRatio;
            SpawnDelay /= RoundRatio;
        }
    }
}