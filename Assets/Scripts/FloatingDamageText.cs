using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float destroyTime = 1f;
    private TextMeshPro textMesh;

    void Awake()
    {
        // Get the 3D TextMeshPro component
        textMesh = GetComponent<TextMeshPro>();

        // Automatically destroy this object after 1 second
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // Float upwards
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Always face the camera (Billboard effect)
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    // The boss script will call this to set the specific damage number
    public void Setup(int damageAmount)
    {
        if (textMesh != null)
        {
            textMesh.text = "-" + damageAmount.ToString();
        }
    }
}