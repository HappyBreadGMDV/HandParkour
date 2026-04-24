using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    public float distance = 0.1f;

    public GameObject Hand;
    public ConfigurableJoint HandJoint;
    public Rigidbody enpty2;
    public Rigidbody Player;

    public LayerMask layer;

    public bool climbEnabled;

    public KeyCode grabKey = KeyCode.Mouse0;

    private ConfigurableJoint grabJoint;
    private ConfigurableJoint grabJoint2;

    void FixedUpdate()
    {
        HandJoint.targetPosition = Hand.transform.localPosition;

        if (Input.GetKey(grabKey))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance, layer))
            {
                climbEnabled = true;

                if (grabJoint == null)
                {
                    grabJoint = HandJoint.gameObject.AddComponent<ConfigurableJoint>();
                    grabJoint.autoConfigureConnectedAnchor = false;
                    grabJoint.connectedBody = enpty2;

                    // Фикс: используем локальное смещение относительно руки
                    grabJoint.anchor = Vector3.zero;
                    // Фикс: переводим мировую точку в локальные координаты enpty2
                    grabJoint.connectedAnchor = enpty2.transform.InverseTransformPoint(hit.point);

                    grabJoint.xMotion = ConfigurableJointMotion.Locked;
                    grabJoint.yMotion = ConfigurableJointMotion.Locked;
                    grabJoint.zMotion = ConfigurableJointMotion.Locked;
                    grabJoint.angularXMotion = ConfigurableJointMotion.Locked;
                    grabJoint.angularYMotion = ConfigurableJointMotion.Locked;
                    grabJoint.angularZMotion = ConfigurableJointMotion.Locked;

                    JointDrive drive = new JointDrive
                    {
                        positionSpring = 50000f,
                        positionDamper = 50000f,
                        maximumForce = Mathf.Infinity
                    };
                    grabJoint.xDrive = drive;
                    grabJoint.yDrive = drive;
                    grabJoint.zDrive = drive;
                }
            }
        }
        else if (climbEnabled)
        {
            climbEnabled = false;
            Hand.transform.localPosition = Vector3.zero;
            HandJoint.targetPosition = Vector3.zero;

            if (grabJoint != null)
            {
                Destroy(grabJoint);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;

        Gizmos.color = Color.red;
        Vector3 direction = Camera.main.transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + direction * distance);
    }
}