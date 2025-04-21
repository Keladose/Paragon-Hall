using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class RoomController : MonoBehaviour
    {
        public List<DoorController> doors = new();
        public List<GameObject> enmies;
        public int roomId = 0;
        // Start is called before the first frame update
        private void Awake()
        {
            
        }
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
