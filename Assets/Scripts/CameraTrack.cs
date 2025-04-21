using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Vector3 offset;    // How far the camera is from the player
    public float smoothSpeed = 0.125f;  // How smoothly the camera follows

    void LateUpdate()
    {
        // Create the desired position based on the player's position and the offset
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position to the smoothed position
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
