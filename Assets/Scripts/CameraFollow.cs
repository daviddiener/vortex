using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Transform camTransform;
    [SerializeField] private float SmoothTime = 0.3f;
    [SerializeField] private bool followRotation = true;

    private Vector3 Offset;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        Offset = camTransform.position - Target.position;
    }

    private void LateUpdate() {
        if (Target) {
            camTransform.position = Vector3.SmoothDamp(transform.position, Target.position + Offset, ref velocity, SmoothTime);
            if (followRotation) camTransform.rotation = Target.rotation;
        }
    }
}