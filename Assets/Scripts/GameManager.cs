using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spellect
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance;
        public GameObject playerPrefab;
        public GameObject playerObject;
        private RoomController currentRoom;
        public bool switchingRooms = false; // used to give invuln on room switching?

        
        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log("Awoke GM");
            if (Instance != null)
            {
                Destroy(this.gameObject);
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
        public void GoToRoom(string roomName, Door.Direction fromDoorDirection)
        {
            switchingRooms = true;
            // TODO: make player invincible/invisible
            SceneManager.LoadScene(roomName);
            currentRoom = FindFirstObjectByType<RoomController>();
            if (currentRoom == null)
            {
                Debug.Log("Room controller not found");
            }
            Door.Direction spawnDirection = Door.GetOppositeDirection(fromDoorDirection);
            currentRoom.DisableDoorOnEntry(spawnDirection);

            playerObject.transform.position = currentRoom.GetDoorPosition(Door.GetOppositeDirection(fromDoorDirection));
            // TODO: move player to door location vulnerable again
        }
    }

}
