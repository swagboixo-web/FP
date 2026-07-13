using UnityEngine;

public class MosquitoController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("Screen Bounds Padding")]
    [SerializeField] private float paddingX = 0.5f;
    [SerializeField] private float paddingZ = 0.5f;

    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 movementVector;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        CalculateScreenBounds();
    }

    void Update()
    {
        // Read arcade layout input axes (WASD / Arrow Keys)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movementVector = new Vector3(moveX, 0f, moveZ).normalized;
    }

    void FixedUpdate()
    {
        // movementVector matching the lecture logic perfectly: position + velocity * time
        Vector3 targetPosition = rb.position + movementVector * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }

    void LateUpdate()
    {
        CalculateScreenBounds();
        ClampPosition();
    }

    private void CalculateScreenBounds()
    {
        if (mainCamera == null) return;

        float yDistance = mainCamera.transform.position.y - transform.position.y;
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, yDistance));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, yDistance));

        minBounds = new Vector2(bottomLeft.x, bottomLeft.z);
        maxBounds = new Vector2(topRight.x, topRight.z);
    }

    private void ClampPosition()
    {
        Vector3 currentPosition = transform.position;

        // Lock player inside the screen viewport boundaries
        currentPosition.x = Mathf.Clamp(currentPosition.x, minBounds.x + paddingX, maxBounds.x - paddingX);
        currentPosition.z = Mathf.Clamp(currentPosition.z, minBounds.y + paddingZ, maxBounds.y - paddingZ);

        transform.position = currentPosition;
    }
}