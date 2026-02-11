using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    [Header("Camera Follow Settings")]
    public Transform target; // The target the camera will follow
    public float followSpeed = 7.5f; // The speed of the camera's movement
    public Vector2 offset = Vector2.zero; // The offset from the target's position
    
    [Header("Camera Bounds Settings")]
    public Vector2 minBounds = new(-500f, -500f); // Minimum bounds for the camera's position
    public Vector2 maxBounds = new(500f, 500f); // Maximum bounds for the camera's position

    [Header("Pixel Snapping Settings")]
    public float pixelsPerUnit = 16f;

    [Header("Edge Follow Settings")]
    public bool boostSpeedNearEdges = true; // Whether to boost speed when near edges
    public float edgeBuffer = 0.5f; // Distance from the edge at which to start boosting speed
    public float edgeBoostMultiplier = 2f; // Multiplier for speed boost when near

    private void LateUpdate()
    {
        if (target == null) return;

        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        Vector2 velocity = rb ? rb.linearVelocity : Vector2.zero;

        float leadDistance = 2f;
        Vector2 lead = velocity.normalized * leadDistance;

        Vector2 targetPosition = (Vector2)target.position + offset + lead;

        Vector2 clampedTargetPos = new Vector2(
            Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y)
        );

        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            new Vector3(clampedTargetPos.x, clampedTargetPos.y, transform.position.z),
            1 - Mathf.Exp(-followSpeed * Time.deltaTime)
        );

        float unitPerPixel = 1f / pixelsPerUnit;
        smoothedPosition.x = Mathf.Round(smoothedPosition.x / unitPerPixel) * unitPerPixel;
        smoothedPosition.y = Mathf.Round(smoothedPosition.y / unitPerPixel) * unitPerPixel;
        
        transform.position = smoothedPosition;
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
        if (followSpeed < 0) followSpeed = 0;
        if (pixelsPerUnit <= 0) pixelsPerUnit = 16f;
        if (edgeBuffer < 0) edgeBuffer = 0;
        if (edgeBoostMultiplier < 1f) edgeBoostMultiplier = 1f;
    }
}
