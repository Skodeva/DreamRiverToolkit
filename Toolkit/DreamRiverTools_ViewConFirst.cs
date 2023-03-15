using SKODE;
using UnityEngine;

/// <summary>
/// 绑定在摄像机上
/// 鼠标左键：摄像机绕着被观察物体转动；
/// 鼠标中键：缩放视野距离
/// </summary>
public class DreamRiverTools_ViewConFirst : BaseMonoBehaviour<DreamRiverTools_ViewConFirst>
{
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 20f;
    public float ySpeed = 20.0f;
    public float yMinLimit = -5f;
    public float yMaxLimit = 60f;
    public float distanceMin = .5f;
    public float distanceMax = 10f;
    public float smoothTime = 6f;

    private float _rotationYAxis;
    private float _rotationXAxis;
    private float _velocityX;
    private float _velocityY;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _rotationYAxis = angles.y;
        _rotationXAxis = angles.x;
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(0))
            {
                _velocityX += xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f;
                _velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
            }

            _rotationYAxis += _velocityX;
            _rotationXAxis -= _velocityY;
            _rotationXAxis = ClampAngle(_rotationXAxis, yMinLimit, yMaxLimit);
            Quaternion toRotation = Quaternion.Euler(_rotationXAxis, _rotationYAxis, 0);
            Quaternion rotation = toRotation;

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
            if (Physics.Linecast(target.position, transform.position, out var hit))
            {
                distance -= hit.distance;
            }

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
            _velocityX = Mathf.Lerp(_velocityX, 0, Time.deltaTime * smoothTime);
            _velocityY = Mathf.Lerp(_velocityY, 0, Time.deltaTime * smoothTime);
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}