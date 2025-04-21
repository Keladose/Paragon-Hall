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
        public CameraTrack cameraTrack;
        public bool switchingRooms = false; // used to give invuln on room switching?
        public Door.Direction SpawnDirection = Door.Direction.Undefined;


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
            transform.position = playerObject.transform.position;
        }
        public void GoToRoom(string roomName, Door.Direction fromDoorDirection)
        {
            switchingRooms = true;
            playerObject.GetComponent<PlayerController>().canMove = false;
            SpawnDirection = Door.GetOppositeDirection(fromDoorDirection);
            // TODO: make player invincible/invisible
            SceneManager.LoadScene(roomName);

            // TODO: move player to door location vulnerable again
        }
    }

}
