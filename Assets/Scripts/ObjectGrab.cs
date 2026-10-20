using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectGrab : MonoBehaviour
{
    [SerializeField] private MemoryData memory;
    [SerializeField] private string memoryLine = "This reminds me of...";
    [SerializeField] private bool showMemoryOnlyOnce = true;

    private Rigidbody objectRigidBody;
    private Collider[] objectColliders;
    private Collider playerCollider;
    private Transform originalParent;
    private RigidbodyInterpolation originalInterpolation;
    private bool hasShownMemory;

    public bool IsHeld { get; private set; }
    public bool IsPacked { get; private set; }

    public void MarkPacked()
    {
        IsPacked = true;
    }

    private void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        objectColliders = GetComponentsInChildren<Collider>();
        originalParent = transform.parent;
        originalInterpolation = objectRigidBody.interpolation;
    }

    public void Grab(Transform grabPoint, Collider player)
    {
        playerCollider = player;
        IsHeld = true;

        objectRigidBody.linearVelocity = Vector3.zero;
        objectRigidBody.angularVelocity = Vector3.zero;

        objectRigidBody.useGravity = false;
        objectRigidBody.isKinematic = true;
        objectRigidBody.interpolation = RigidbodyInterpolation.None;

        SetIgnorePlayer(true);

        transform.SetParent(grabPoint, true);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (!showMemoryOnlyOnce || !hasShownMemory)
        {
            hasShownMemory = true;
            StartCoroutine(ShowPopupAfterPickup());
        }
    }

    private IEnumerator ShowPopupAfterPickup()
    {
        yield return null;

        if (MemoryPopupManager.Instance == null) yield break;

        if (memory != null)
        {
            MemoryPopupManager.Instance.ShowMemory(memory);
        }
        else
        {
            MemoryPopupManager.Instance.ShowMemory(memoryLine);
        }
    }

    public void Drop()
    {
        IsHeld = false;

        transform.SetParent(originalParent, true);

        objectRigidBody.isKinematic = false;
        objectRigidBody.useGravity = true;
        objectRigidBody.interpolation = originalInterpolation;
        objectRigidBody.linearVelocity = Vector3.zero;
        objectRigidBody.angularVelocity = Vector3.zero;

        SetIgnorePlayer(false);
    }

    private void SetIgnorePlayer(bool ignore)
    {
        if (playerCollider == null) return;

        foreach (Collider objectCollider in objectColliders)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, ignore);
        }
    }
}