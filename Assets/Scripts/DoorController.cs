using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

namespace Spellect
{
    public class DoorController : MonoBehaviour
    {

        public bool isOpen;
        public string connectedSceneName = "undefinedScene"; // can later change this to room number or something and have a list of rooms data for procedural generation
        public Door.Direction direction;
        private bool disabled = false;
        public Transform spawnPosition;
        public void Init()
        {
            // TODO: Check if should be locked or not, also configuring the attached room if doing procedural
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!disabled && isOpen && other.CompareTag("Player"))
            {
                GameManager.Instance.GoToRoom(connectedSceneName, direction);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (disabled && other.CompareTag("Player"))
            {
                disabled = false;
            }
        }
        public void DisableOnEntry()
        {
            disabled = true;
        }

    

    }
}