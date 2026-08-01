using UnityEngine;

public class TerrainScrollerCamera : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float scrollSpeed = 4f; // Speed at which the camera flies over the terrain
    public bool keepScrolling = true;

    [Header("Starting Camera Angle")]
    public Vector3 angleRotation = new Vector3(55f, 0f, 0f);

    void Start()
    {
        // Set the 2.5D tilt immediately
        transform.rotation = Quaternion.Euler(angleRotation);
    }

    void LateUpdate()
    {
        if (keepScrolling)
        {
            // Moves the camera smoothly straight forward along the Z axis
            transform.Translate(Vector3.forward * scrollSpeed * Time.deltaTime, Space.World);
        }
    }

    // Call this from your BossTrigger script to stop the camera at the arena!
    public void StopScrolling()
    {
        keepScrolling = false;
    }
}