using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Follow Settings")]
    public Transform target; // The target the camera will follow
    public float followSpeed = 7.5f; // The speed of the camera's movement
    public Vector2 offset = Vector2.zero; // The offset from the target's position
    
    [Header("Camera Bounds Settings")]
    public Vector2 minBounds = new(-500f, -500f); // Minimum bounds for the camera's position
    public Vector2 maxBounds = new(500f, 500f); // Maximum bounds for the camera's position

    private void LateUpdate()
    {
        if (target == null) return; // If no target is assigned, do nothing

        
        Vector2 desiredPosition = (Vector2)target.position + offset; // Calculate the desired position of the camera based on the target's position

        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x); // Clamp the x position within the defined bounds
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y); // Clamp the y position within the defined bounds
        
        Vector3 clampedPosition = new(clampedX, clampedY, transform.position.z); // Create a new position vector with the clamped x and y values
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, 1 - Mathf.Exp(-followSpeed * Time.deltaTime)); // Smoothly interpolate the camera's position towards the clamped position

        transform.position = smoothedPosition; // Update the camera's position
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the camera bounds in the editor for visualization
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(minBounds.x, minBounds.y), new Vector2(minBounds.x, maxBounds.y)); // Bottom edge
        Gizmos.DrawLine(new Vector2(minBounds.x, maxBounds.y), new Vector2(maxBounds.x, maxBounds.y)); // Top edge
        Gizmos.DrawLine(new Vector2(maxBounds.x, maxBounds.y), new Vector2(maxBounds.x, minBounds.y)); // Left edge
        Gizmos.DrawLine(new Vector2(maxBounds.x, minBounds.y), new Vector2(minBounds.x, minBounds.y)); // Right edge
    }

    private void OnValidate()
    {
        if (followSpeed < 0) followSpeed = 0; // Ensure follow speed is not negative
        
    }
}
