using UnityEngine;

public class LightCollider : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private bool _isEnable = false;
    private RaycastHit hit;

    void OnDrawGizmos()
    {
        if (_isEnable == false) return;

        Vector3 direction = transform.TransformDirection(Vector3.forward);

        if (Physics.SphereCast(transform.position, _radius, direction, out hit))
        {
            Gizmos.DrawRay(transform.position, direction * hit.distance);
            Debug.Log("Ray starts at: " + transform.position + " and ends at: " + (transform.position + direction * hit.distance));
        }
        else
        {
            Gizmos.DrawRay(transform.position, direction * 100);
            Debug.Log("Ray starts at: " + transform.position + " and ends at: " + (transform.position + direction * 100));
        }
    }
}
