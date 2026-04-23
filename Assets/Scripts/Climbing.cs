using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("Settings")]
    public float grabDistance = 3f;
    public LayerMask climbLayer;
    public KeyCode grabKey = KeyCode.Mouse0;

    [Header("Auto Distance")]
    public float minHoldDistance = 0.8f;
    public float maxHoldDistance = 2.0f;

    [Header("Movement")]
    public float pullSpeed = 5f; // скорость подтягивания к точке

    private bool isGrabbing = false;
    private Vector3 grabPoint;
    private float currentHoldDistance;

    private Camera playerCamera;
    private Rigidbody playerRigidbody;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();

        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
            Debug.LogError("Нет Rigidbody на игроке!");

        playerRigidbody.freezeRotation = true;
        playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        if (Input.GetKeyDown(grabKey))
            TryGrab();

        if (Input.GetKeyUp(grabKey) && isGrabbing)
            Release();

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * grabDistance, Color.red);
    }

    void TryGrab()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, climbLayer))
        {
            grabPoint = hit.point;

            // Авто-дистанция от камеры до точки
            currentHoldDistance = Vector3.Distance(playerCamera.transform.position, grabPoint);
            currentHoldDistance = Mathf.Clamp(currentHoldDistance, minHoldDistance, maxHoldDistance);

            isGrabbing = true;
            Debug.Log("Схватился за: " + hit.collider.name);
        }
        else
        {
            Debug.Log("Нет объекта для захвата");
        }
    }

    void Release()
    {
        isGrabbing = false;
        Debug.Log("Отпустил");
    }

    void FixedUpdate()
    {
        // Движение руками
        if (isGrabbing)
        {
            // Направление от камеры к точке захвата
            Vector3 grabDir = (grabPoint - playerCamera.transform.position).normalized;
            Vector3 targetPos = grabPoint - grabDir * currentHoldDistance;

            // Плавно двигаем игрока к позиции
            playerRigidbody.MovePosition(Vector3.Lerp(playerRigidbody.position, targetPos, Time.fixedDeltaTime * pullSpeed));

            // Можно подтягивать или отдаляться колесом мыши
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                currentHoldDistance -= scroll * 2f;
                currentHoldDistance = Mathf.Clamp(currentHoldDistance, minHoldDistance, maxHoldDistance);
            }
        }

        // Движение WASD во время удержания
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = playerCamera.transform.right * h + playerCamera.transform.forward * v;
        playerRigidbody.AddForce(move * 200f);
    }
}