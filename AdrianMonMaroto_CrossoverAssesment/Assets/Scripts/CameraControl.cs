using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float _rotSpeed = 1.0f;
    [SerializeField] float _distanceToNewTarget = 15f;

    [SerializeField] bool _invertX = false;
    [SerializeField] bool _invertY = false;

    private Camera _cam;
    private Transform _target;
    private Vector3 _previousPosition;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (_target == null) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            _previousPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) {
            Vector3 deltaMousePosition = Input.mousePosition - _previousPosition;
            float rotationX = deltaMousePosition.y * _rotSpeed * (_invertY ? -1 : 1);
            float rotationY = -deltaMousePosition.x * _rotSpeed * (_invertX ? -1 : 1);
            Debug.Log($"{rotationX}, {rotationY}");

            transform.RotateAround(_target.position, Vector3.up, rotationY);
            transform.RotateAround(_target.position, transform.right, rotationX);
        }

        _previousPosition = Input.mousePosition;
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
        transform.position = new Vector3(_target.position.x + _distanceToNewTarget, _target.position.y, _target.position.z);
        transform.LookAt(_target);
    }
}
