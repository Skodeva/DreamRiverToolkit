using UnityEngine;

public class DreamRiverTools_3DCollisionLimit : MonoBehaviour
{
    // Careful when setting this to true - it might cause double
    // events to be fired - but it won't pass through the trigger
    public bool sendTriggerMessage = false;

    public LayerMask layerMask = -1; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 

    private float _minimumExtent;
    private float _partialExtent;
    private float _sqrMinimumExtent;
    private Vector3 _previousPosition;
    private Rigidbody _myRigidbody;
    private Collider _myCollider;

    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<Collider>();
        _previousPosition = _myRigidbody.position;
        _minimumExtent = Mathf.Min(Mathf.Min(_myCollider.bounds.extents.x, _myCollider.bounds.extents.y),
            _myCollider.bounds.extents.z);
        _partialExtent = _minimumExtent * (1.0f - skinWidth);
        _sqrMinimumExtent = _minimumExtent * _minimumExtent;
    }

    void FixedUpdate()
    {
        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = _myRigidbody.position - _previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > _sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitInfo;

            //check for obstructions we might have missed 
            if (Physics.Raycast(_previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
            {
                if (!hitInfo.collider)
                    return;

                if (hitInfo.collider.isTrigger)
                    hitInfo.collider.SendMessage("OnTriggerEnter", _myCollider);

                if (!hitInfo.collider.isTrigger)
                    _myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * _partialExtent;
            }
        }

        _previousPosition = _myRigidbody.position;
    }
}