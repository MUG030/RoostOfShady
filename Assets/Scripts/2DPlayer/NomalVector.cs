using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalVector : MonoBehaviour
{
    public float rayDistance = 10.0f;

    void Update()
    {
        // Create a ray from the transform in the forward direction
        Ray ray = new Ray(transform.position, transform.forward);

        // Draw the ray in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        // Raycast and check if it hits something
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Output the name of the hit object to the console
            Debug.Log("Ray hit object: " + hit.transform.name);

            // Get the normal of the hit surface
            Vector3 hitNormal = hit.normal;

            // Calculate the rotation to be perpendicular to the hit surface
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hitNormal) * transform.rotation;

            // Save the current Z rotation
            float currentZRotation = transform.eulerAngles.z;

            // Apply the rotation
            transform.rotation = targetRotation;

            // Reset the Z rotation to the saved value
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, currentZRotation);
        }
    }
}
