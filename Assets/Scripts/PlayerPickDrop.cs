using UnityEngine;

public class PlayerPickDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCamTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private float pickUpDistance = 3f;
    [SerializeField] private float aimRadius = 0.1f;

    private Collider playerCollider;
    private ObjectGrab heldObject;

    private void Awake()
    {
        playerCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        // don't grab or drop while a memory has the game paused
        if (Time.timeScale == 0f) return;

        if (!Input.GetKeyDown(KeyCode.E)) return;

        if (heldObject != null)
        {
            heldObject.Drop();
            heldObject = null;
            return;
        }

        if (Physics.SphereCast(playerCamTransform.position, aimRadius, playerCamTransform.forward,
                out RaycastHit hit, pickUpDistance, pickUpLayerMask, QueryTriggerInteraction.Ignore))
        {
            ObjectGrab objectGrab = hit.collider.GetComponentInParent<ObjectGrab>();

            // packed objects stay in the crate
            if (objectGrab != null && !objectGrab.IsPacked)
            {
                objectGrab.Grab(objectGrabPointTransform, playerCollider);
                heldObject = objectGrab;
            }
        }
    }
}