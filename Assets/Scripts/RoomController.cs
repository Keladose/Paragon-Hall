using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class RoomController : MonoBehaviour
    {
        public List<DoorController> doors = new();
        // Start is called before the first frame update
        void Start()
        {
            InitDoors();
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
