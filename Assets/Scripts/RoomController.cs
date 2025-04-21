using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class RoomController : MonoBehaviour
    {
        public List<DoorController> doors = new();
        public List<Spawner> spawners;
        public int enemiesAlive = 0;
        public int roomId = 0;

        public int numWaves = 3;
        private int _wavesRemaining = 0;


        
        // Start is called before the first frame update
        void Start()
        {
            InitDoors();
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.switchingRooms)
                {
                    GameManager.Instance.playerObject.transform.position = GetDoorPosition(GameManager.Instance.SpawnDirection);
                    GameManager.Instance.switchingRooms = false;
                    
                }
                if (GameManager.Instance.clearedRooms.Contains(roomId) && !GameManager.Instance.SpawnBoss)
                {
                    DisableSpawners();
                }
                else
                {
                    LockAllDoors();
                }
            }
            foreach (Spawner spawner in spawners)
            {
                spawner.OnEnemySpawned += OnEnemySpawned;
                spawner.OnEnemyDied += OnEnemyDied;
            }
            _wavesRemaining = numWaves;
            SpawnNextWave();

        }

        private void OnEnemySpawned(object o, EventArgs e)
        {
            enemiesAlive++;
        }

        private void OnEnemyDied(object o, EventArgs e)
        {
            enemiesAlive--;
            if (enemiesAlive == 0)
            {
                if (_wavesRemaining == 0)
                {
                    MarkRoomCompelted();
                }
                else
                {
                    SpawnNextWave();
                }
            }
        }

        private void SpawnNextWave()
        {
            Debug.Log("Spawning wave" + _wavesRemaining);
            foreach (Spawner spawner in spawners)
            {
                spawner.SpawnNextWave();
            }
            _wavesRemaining--;
        }

        private void MarkRoomCompelted()
        {
            foreach (DoorController door in doors)
            {
                door.isOpen = true;
            }
            GameManager.Instance.AddClearedRoom(roomId);
        }

        private bool CheckRoomCompleted()
        {
            if (GameManager.Instance.clearedRooms.Contains(roomId))
            {
                return true;
            }
            return false;
        }
        
        private void DisableSpawners()
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.Disable();
            }
        }
        // Update is called once per frame
        void Update()
        {

        }

        private void InitDoors()
        {
            foreach (DoorController door in doors)  
            {
                door.Init();
            }
        }
        private void LockAllDoors()
        {
            foreach (DoorController door in doors)
            {
                door.Lock();
            }

        }

        public Vector3 GetDoorPosition(Door.Direction direction)
        {
            foreach (DoorController door in doors)
            {
                if (door.direction == direction)
                {
                    return door.spawnPosition.position;
                }
            }
            return Vector3.zero;
        }
        public void DisableDoorOnEntry(Door.Direction direction)
        {
            foreach (DoorController door in doors)
            {
                if (door.direction == direction)
                {
                    door.DisableOnEntry();
                }
            }
        }
    }
}
