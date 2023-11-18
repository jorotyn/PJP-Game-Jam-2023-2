using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Serialized Fields
    [Header("Cinemachine Settings")]
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [Header("Movement Settings")]
    [SerializeField] private bool useEdgeScrolling = false;
    [SerializeField] private bool useDragPan = true;

    [Header("Dynamic Pan Speed Settings")]
    [SerializeField] private float panSpeedMin = 20f;
    [SerializeField] private float panSpeedMax = 100f;

    [Header("Dynamic Drag Pan Speed Settings")]
    [SerializeField] private float dragPanSpeedMin = 2f;
    [SerializeField] private float dragPanSpeedMax = 10f;

    [Header("Rotation Settings")]
    [SerializeField] private float keyboardRotationSpeed = 100f;
    [SerializeField] private float mouseRotationSpeed = 40f;

    [Header("Zoom Settings")]
    [SerializeField] private float scrollZoomSpeed = 2f;
    [SerializeField] private float keyboardZoomSpeed = 0.05f;
    [SerializeField] private float followOffsetMinY = 10f;
    [SerializeField] private float followOffsetMaxY = 50f;

    [Header("Boundary Settings")]
    [SerializeField] private Vector3 minBoundary;
    [SerializeField] private Vector3 maxBoundary;
    #endregion

    #region Private Fields
    private float panSpeed = 50f;
    private float currentZoomValue = 0;
    private float targetZoomValue = 0;
    private float originalPanSpeed;
    private float dragPanSpeedCurrent;
    private bool isSpeedBoostActive = false;
    private bool dragPanMoveActive = false;
    private Vector2 lastMousePosition;
    private bool middleMouseRotateActive = false;
    private Vector2 mouseRotateVector = Vector2.zero;
    private Vector3 followOffset;
    private CinemachineTransposer cinemachineTransposer;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        InitializeCamera();
    }

    private void Update()
    {
        CheckDragPanActivation();
        CheckMiddleMouseRotationActivation();
        AdjustPanSpeed();
        HandleCameraMovement();
        if (useEdgeScrolling) HandleCameraMovementEdgeScrolling();
        if (useDragPan) HandleCameraMovementDragPan();
        HandleCameraRotation();
        HandleCameraZoom();
    }

    #endregion

    #region Initialization

    private void InitializeCamera()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        followOffset = cinemachineTransposer.m_FollowOffset;
    }
    #endregion

    #region Camera Movement and Behavior
    private void CheckDragPanActivation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragPanMoveActive = false;
        }
    }

    private void CheckMiddleMouseRotationActivation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            middleMouseRotateActive = true;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            middleMouseRotateActive = false;
        }
    }

    private void AdjustPanSpeed()
    {
        if (isSpeedBoostActive)
        {
            if (originalPanSpeed == 0)
            {
                originalPanSpeed = panSpeed;
            }
            panSpeed = originalPanSpeed * 2;
        }
        else
        {
            if (originalPanSpeed != 0)
            {
                panSpeed = originalPanSpeed;
                originalPanSpeed = 0;
            }
            else
            {
                float t = Mathf.InverseLerp(followOffsetMinY, followOffsetMaxY, followOffset.y);
                panSpeed = Mathf.Lerp(panSpeedMin, panSpeedMax, t);

                dragPanSpeedCurrent = Mathf.Lerp(dragPanSpeedMin, dragPanSpeedMax, t);
            }
        }
    }

    private void HandleCameraMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;
        transform.position += moveDir * panSpeed * Time.deltaTime;

        ClampCameraPosition();
    }

    private void HandleCameraMovementEdgeScrolling()
    {
        Vector3 inputDir = Vector3.zero;

        int edgeScrollSize = 20;

        if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f;
        if (Input.mousePosition.y < edgeScrollSize) inputDir.z = -1f;
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1f;
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = +1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * panSpeed * Time.unscaledDeltaTime;

        ClampCameraPosition();
    }

    private void HandleCameraMovementDragPan()
    {
        if (dragPanMoveActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;
            Vector3 inputDir = new Vector3(-mouseMovementDelta.x, 0, -mouseMovementDelta.y) * dragPanSpeedCurrent;

            transform.position += inputDir * Time.deltaTime;
            lastMousePosition = Input.mousePosition;
        }
    }

    private void HandleCameraRotation()
    {
        if (middleMouseRotateActive)
        {
            mouseRotateVector.x = Input.GetAxis("Mouse X");
            transform.Rotate(0, mouseRotateVector.x * mouseRotationSpeed * Time.deltaTime, 0);
        }
    }

    private void HandleCameraZoom()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput != 0)
        {
            targetZoomValue -= zoomInput * scrollZoomSpeed;
            targetZoomValue = Mathf.Clamp(targetZoomValue, followOffsetMinY, followOffsetMaxY);
        }

        currentZoomValue = Mathf.Lerp(currentZoomValue, targetZoomValue, Time.unscaledDeltaTime * panSpeed);

        float zoomDifference = currentZoomValue - followOffset.y;
        followOffset.y += zoomDifference;

        followOffset.y = Mathf.Clamp(followOffset.y, followOffsetMinY, followOffsetMaxY);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, Time.unscaledDeltaTime * panSpeed);
    }

    private void ClampCameraPosition()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBoundary.x, maxBoundary.x),
            Mathf.Clamp(transform.position.y, minBoundary.y, maxBoundary.y),
            Mathf.Clamp(transform.position.z, minBoundary.z, maxBoundary.z)
        );
    }
    #endregion
}
