using UnityEngine;

public class CameraFollow25D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("2.5D Offset & Angle")]
    [Tooltip("Offset relative to player: (X=0, Y=Height above, Z=Distance behind)")]
    public Vector3 offset = new Vector3(0f, 12f, -8f);
    public Vector3 angleRotation = new Vector3(55f, 0f, 0f);
    public float smoothSpeed = 8f;

    [Header("Map Bounds")]
    public bool useBounds = true;
    public float minX = -50f;
    public float maxX = 50f;
    public float minZ = -10f;
    public float maxZ = 900f; // Long trek limit along your backyard

    void Start()
    {
        // Set the angled tilt on launch
        transform.rotation = Quaternion.Euler(angleRotation);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate target position based on player + offset
        Vector3 desiredPosition = target.position + offset;

        // Apply clamping bounds so camera doesn't show outside the map
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);
        }

        // Smoothly glide camera to desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}