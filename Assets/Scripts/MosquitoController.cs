using UnityEngine;
using UnityEngine.InputSystem;

public class MosquitoController : MonoBehaviour
{
    [Header("Touch Settings")]
    [Tooltip("How fast the mosquito catches up to your finger")]
    [SerializeField] private float trackingSpeed = 20f;
    [Tooltip("Pushes the mosquito slightly above your finger so you can see it!")]
    [SerializeField] private float fingerOffsetZ = 2f;

    [Header("Phase 1: Scrolling Boundaries")]
    [Tooltip("Bottom screen bumper: Pushes player forward if they fall behind")]
    [SerializeField] private float minZOffsetFromCamera = 2f;
    [Tooltip("Top screen bumper: Prevents flying miles ahead of the camera")]
    [SerializeField] private float maxZOffsetFromCamera = 16f;

    [Header("Phase 2: Boss Boundaries")]
    [Tooltip("How far forward you can fly when fighting the boss. Make this BIG so you can get close to the Iguana!")]
    [SerializeField] private float bossMaxZOffset = 50f;

    [Header("Map Boundaries")]
    [Tooltip("Left and Right limits across the garden path")]
    [SerializeField] private float minX = -85f;
    [SerializeField] private float maxX = 85f;

    [Header("Game State")]
    public bool bossFightActive = false; // BossTrigger.cs switches this on!

    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 targetPosition;
    private bool isTouchingScreen;
    private Plane movementPlane;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        targetPosition = transform.position;
    }

    void Update()
    {
        if (mainCamera == null) return;

        // Keep the invisible plane aligned with the player's flying height
        movementPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            isTouchingScreen = true;
            Ray ray = mainCamera.ScreenPointToRay(Pointer.current.position.ReadValue());

            if (movementPlane.Raycast(ray, out float hitDistance))
            {
                Vector3 worldPosition = ray.GetPoint(hitDistance);
                targetPosition = new Vector3(worldPosition.x, transform.position.y, worldPosition.z + fingerOffsetZ);
            }
        }
        else
        {
            isTouchingScreen = false;
        }
    }

    void FixedUpdate()
    {
        if (mainCamera == null) return;

        Vector3 currentPos = rb.position;

        if (isTouchingScreen)
        {
            // Smoothly glide towards touch target
            currentPos = Vector3.Lerp(currentPos, targetPosition, trackingSpeed * Time.fixedDeltaTime);
        }

        // 1. CLAMP LEFT & RIGHT (Always active so you don't fly off the sides of the path)
        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);

        // 2. DYNAMIC Z-AXIS CLAMPING
        float minAllowedZ = mainCamera.transform.position.z + minZOffsetFromCamera;
        float maxAllowedZ;

        if (!bossFightActive)
        {
            // LEVEL SCROLLING MODE: Standard top boundary
            maxAllowedZ = mainCamera.transform.position.z + maxZOffsetFromCamera;
        }
        else
        {
            // BOSS FIGHT MODE: Massive top boundary so you can fly up to the boss
            maxAllowedZ = mainCamera.transform.position.z + bossMaxZOffset;
        }

        // Apply the bounds. This naturally pushes the player forward if they touch the bottom bound!
        if (currentPos.z < minAllowedZ) currentPos.z = minAllowedZ;
        else if (currentPos.z > maxAllowedZ) currentPos.z = maxAllowedZ;

        // Apply movement physics cleanly
        rb.MovePosition(currentPos);
    }
}