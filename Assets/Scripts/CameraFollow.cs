using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public bool followRotation = true;

    [SerializeField] private Transform camTransform;
    [SerializeField] private float SmoothTimeShip = 0.3f;
    [SerializeField] private float SmoothTimeSun = 1.0f;
    

    private float smoothTimeActive;
    private Vector3 Offset;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        useSmoothTimeShip();
        Offset = new Vector3(0, 0, camTransform.position.z - Target.position.z);
    }

    private void LateUpdate() {
        if (Target) {
            camTransform.position = Vector3.SmoothDamp(transform.position, Target.position + Offset, ref velocity, smoothTimeActive);
            if (followRotation) camTransform.rotation = Target.rotation;
        }
    }

    public void useSmoothTimeShip()
    {
        smoothTimeActive = SmoothTimeShip;
    }

    public void useSmoothTimeSun()
    {
        smoothTimeActive = SmoothTimeSun;
    }
}