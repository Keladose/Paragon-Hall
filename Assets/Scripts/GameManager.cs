using System;
using System.Collections.Generic;
using TMPro;
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
        private string previousRoom;

        public List<int> clearedRooms = new();

        public int NUM_CLEARABLE_ROOMS = 8;
        public bool SpawnBoss = false;

        public AudioClip doorClose;
        public AudioClip doorOpen;
        public AudioSource audioSource;

        public TMP_Text soundText;

        public TMP_Text topText;


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
            previousRoom = SceneManager.GetActiveScene().name;
        }

        public void ToggleSound()
        {
            if (playerObject.GetComponent<AudioListener>().enabled)
            {
                playerObject.GetComponent<AudioListener>().enabled = false;
                soundText.text = "Sound Off";
            }
            else
            {
                
                playerObject.GetComponent<AudioListener>().enabled = true;
                soundText.text = "Sound On";
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = playerObject.transform.position;
        }
        public void GoToRoom(string roomName, Door.Direction fromDoorDirection)
        {
            topText.text = "";
            audioSource.PlayOneShot(doorOpen);
            previousRoom = SceneManager.GetActiveScene().name;
            switchingRooms = true;
            playerObject.GetComponent<PlayerController>().canMove = false;
            SpawnDirection = Door.GetOppositeDirection(fromDoorDirection);
            // TODO: make player invincible/invisible
            SceneManager.LoadScene(roomName);

            // TODO: move player to door location vulnerable again
        }

        public void OnDeath(object o, EventArgs e)
        {
            GoToRoom(previousRoom, SpawnDirection);
        }

        public void AddClearedRoom(int roomId)
        {
            clearedRooms.Add(roomId);
            CheckIfAllRoomsCleared();

            topText.text = "Room Cleared!";
        }
        private void CheckIfAllRoomsCleared()
        {
            if (clearedRooms.Count == NUM_CLEARABLE_ROOMS)
            {
                SpawnBoss = true;
                GoToRoom("Level1", Door.Direction.Up);
            }
        }
    }

}
