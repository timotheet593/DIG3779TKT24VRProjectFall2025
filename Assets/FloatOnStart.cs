using UnityEngine;

public class MagicFloat : MonoBehaviour
{
    [Header("Behavior Toggles")]
    [Tooltip("If checked, the object will be pushed upwards when the game starts.")]
    public bool applyLiftOffForce = true;

    [Header("Lift-Off Settings")]
    [Tooltip("The strength of the initial upward push.")]
    public float liftOffForce = 5f;

    [Header("Floating Bob Motion")]
    [Tooltip("How high and low the object bobs while floating.")]
    public float floatAmplitude = 0.5f;
    [Tooltip("How fast the object bobs up and down.")]
    public float floatSpeed = 1f;

    [Header("Rotational Wiggle Motion")]
    [Tooltip("How much the object tilts back and forth, in degrees.")]
    public float rotationAmplitude = 5f;
    [Tooltip("How fast the object wiggles.")]
    public float rotationSpeed = 1.2f; // Slightly different speed for variation

    // Private variables for script operation
    private Rigidbody rb;
    private Vector3 startPosition;

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();

        // Error check to make sure a Rigidbody is present
        if (rb == null)
        {
            Debug.LogError("MagicFloat script requires a Rigidbody component on " + gameObject.name);
            this.enabled = false; // Disable the script if no Rigidbody is found
            return;
        }

        // --- Core Floating Setup ---
        // 1. Disable gravity to allow it to float
        rb.useGravity = false;

        // 2. Store the initial position as an anchor for the bobbing motion
        startPosition = transform.position;

        // --- Initial Lift-Off ---
        // 3. If enabled, apply the initial upward force
        if (applyLiftOffForce)
        {
            rb.AddForce(Vector3.up * liftOffForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // --- Continuous Floating and Wiggling ---

        // Calculate the bobbing motion using a sine wave for smooth up-and-down movement
        float yOffset = Mathf.Sin(Time.fixedTime * floatSpeed) * floatAmplitude;
        Vector3 targetPosition = startPosition + new Vector3(0, yOffset, 0);

        // Move the Rigidbody to the calculated bobbing position
        rb.MovePosition(targetPosition);

        // Calculate the rotational wiggle using another sine wave
        float zRotation = Mathf.Sin(Time.fixedTime * rotationSpeed) * rotationAmplitude;
        Quaternion targetRotation = Quaternion.Euler(0, 0, zRotation);

        // Apply the wiggle rotation to the Rigidbody
        rb.MoveRotation(targetRotation);
    }
}