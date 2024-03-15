using BulletParadise.Misc;
using BulletParadise.Player;
using BulletParadise.Visual.Drawing;
using BulletParadise.World;
using System.Collections.Generic;
using UnityEngine;

namespace BulletParadise.Components
{
    [System.Serializable]
    public class MobSpawnChanceData
    {
        public string name;
        public GameObject mob;
        public int chance;
    }

    public sealed class MobSpawner : SceneObject
    {
        /// <summary>
        /// TODO: 0 Do zmiany
        /// </summary>
        private PlayerController target;

        public List<MobSpawnChanceData> mobs = new();
        [HideInInspector] public List<int> chanceSpawn = new();

        [Header("Values")]
        public int minSpawnMobs = 1;
        public int maxSpawnMobs = 2;
        public int limitSpawn = 15;
        public float timeBetweenSpawn = 5;
        public float radiusSpawn = 8;
        public float radiusTrigger = 20;
        public float distanceMobSpawn = 1;

        [Header("Debug")]
        public bool isSpawnerWorking = true;
        public float currentTime;
        public List<GameObject> spawnedMobs = new();

        [Header("Draw Debug")]
        public Color colorTrigger = Color.blue;
        public Color colorSpawn = Color.red;

        [HideInInspector] public bool isMobSpawned = false;


        private void Start()
        {
            GameManager.Instance.worldManager.AddDynamicObjectToWorld(this);

            target = PlayerController.Instance;
            currentTime = timeBetweenSpawn;

            //ustalanie szansy spawna
            int chanceCount = 0;
            for (int i = 0; i < mobs.Count; i++)
                chanceCount += mobs[i].chance;

            for (int i = 0; i < chanceCount; i++)
            {
                chanceSpawn.Add(-1);
                do
                {
                    int random = Random.Range(0, mobs.Count);
                    if (mobs[random].chance > 0)
                    {
                        chanceSpawn[i] = random;
                        mobs[random].chance--;
                    }
                } while (chanceSpawn[i] == -1);
            }
        }
        private void Update()
        {
            if (spawnedMobs.Exists(item => item == null)) spawnedMobs.RemoveAll(item => item == null);
            if (!isSpawnerWorking || spawnedMobs.Count >= limitSpawn) return;

            currentTime += Time.deltaTime;
            if (currentTime >= timeBetweenSpawn)
            {
                isMobSpawned = false;
                currentTime = 0;
            }

            if (Vector2.Distance(transform.position, target.transform.position) <= radiusTrigger && !isMobSpawned)
            {
                SpawnMobs();
                isMobSpawned = true;
            }
        }

        public void SpawnMobs()
        {
            if (mobs.Count == 0 || mobs == null) return;

            var readyPositions = new List<Vector2>();
            var mobIndex = new List<int>();
            int ranAmount = Random.Range(minSpawnMobs, maxSpawnMobs + 1);
            int amount = ranAmount;

            if (spawnedMobs.Count + ranAmount > limitSpawn)
                amount = spawnedMobs.Count + ranAmount - limitSpawn;

            for (int i = 0; i < amount; i++)
            {
                var randomSize = Random.Range(0, chanceSpawn.Count);
                var randomIndex = chanceSpawn[randomSize];
                var randomPosition = Vector2.zero;
                var canSpawnHere = false;
                var attemps = 0;

                while (!canSpawnHere)
                {
                    randomPosition = (Vector2)transform.position + Random.insideUnitCircle * radiusSpawn;
                    canSpawnHere = Utils.PreventSpawnCircle(randomPosition, readyPositions.ToArray(), distanceMobSpawn);

                    if (canSpawnHere) break;

                    attemps++;
                    if (attemps >= 40)
                        SpawnMobs();
                }

                readyPositions.Add(randomPosition);
                mobIndex.Add(randomIndex);
            }

            for (int i = 0; i < readyPositions.Count; i++)
                spawnedMobs.Add(Instantiate(mobs[mobIndex[i]].mob, readyPositions[i], Quaternion.identity));
        }

        public override void Draw()
        {
            base.Draw();

            GLDraw.DrawCircle(transform.position, radiusSpawn, colorSpawn);
            GLDraw.DrawCircle(transform.position, radiusTrigger, colorTrigger);
        }
    }
}
