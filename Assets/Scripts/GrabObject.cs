using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public ConfigurableJoint LeftHand;
    public ConfigurableJoint RightHand;
    public ConfigurableJoint grabObject;

    public Transform GrabOffset;
    public Vector3 Offset;

    public Hand HandGrable;

    public bool IsGrabbed;

    [System.Serializable]
    public enum Hand
    {
        LeftHand,
        RightHand
    }

    private void Start()
    {
        Offset = GrabOffset.position - transform.position;
    }

    void Update()
    {
        //grabObject.connectedAnchor = GrabOffset.localPosition;
        if (IsGrabbed)
        {
            if(HandGrable == Hand.LeftHand)
            {
                grabObject.targetPosition = LeftHand.targetPosition + Offset;
                grabObject.targetRotation = LeftHand.targetRotation;
                grabObject.connectedBody = LeftHand.gameObject.GetComponent<Rigidbody>();
            }
            else if (HandGrable == Hand.RightHand)
            {
                grabObject.targetPosition = RightHand.targetPosition + Offset;
                grabObject.targetRotation = RightHand.targetRotation;
                grabObject.connectedBody = RightHand.gameObject.GetComponent<Rigidbody>();
            }
        }
        else
        {
            grabObject.connectedBody = null;
        }
    }
}
