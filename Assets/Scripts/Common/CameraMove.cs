using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("카메라 이동 속도 (units/sec)")]
    private float cameraSpeed = 15f;

    [Header("화면 가장자리 임계값 (pixels)")]
    public float edgeThreshold = 50f;

    [Header("이동 제한 범위")]
    public float minX = -10f;
    public float maxX =  10f;
    public float minY = -5f;
    public float maxY =  5f;

    [Header("줌 속도")]
    public float zoomSpeed = 10f;

    [Header("Perspective 카메라 시야(Field of View) 한계")]
    public float minFov = 15f;
    public float maxFov = 90f;

    [Header("Orthographic 카메라 Size 한계")]
    public float minOrthoSize = 2f;
    public float maxOrthoSize = 20f;

    private Camera cam;
    private bool isLocked = false;
    private Vector3 originCamPosition;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;
        originCamPosition = cam.transform.position;
    }

    /// <summary>
    /// direction:
    /// 0 = Left, 1 = Top, 2 = Right, 3 = Bottom
    /// </summary>
    public void MoveCamera(int direction)
    {
        Vector3 moveDir = Vector3.zero;
        switch (direction)
        {
            case 0: moveDir = Vector3.left;  break;
            case 1: moveDir = Vector3.up;    break;
            case 2: moveDir = Vector3.right; break;
            case 3: moveDir = Vector3.down;  break;
            default:
                Debug.LogWarning("MoveCamera: invalid direction " + direction);
                return;
        }
        transform.Translate(moveDir * cameraSpeed * Time.deltaTime, Space.World);
    }

    private void Update()
    {
        if (isLocked) return;
        Vector3 mousePos = Input.mousePosition;

        // 좌/우
        if (mousePos.x <= edgeThreshold)        MoveCamera(0);
        else if (mousePos.x >= Screen.width - edgeThreshold) MoveCamera(2);

        // 하/상
        if (mousePos.y <= edgeThreshold)        MoveCamera(3);
        else if (mousePos.y >= Screen.height - edgeThreshold) MoveCamera(1);

        ZoomInOut();

        ClampPosition();
    }

    /// <summary>
    /// 마우스 휠 줌인/줌아웃
    /// </summary>
    private void ZoomInOut() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            if (cam.orthographic)
            {
                cam.orthographicSize = Mathf.Clamp(
                    cam.orthographicSize - scroll * zoomSpeed,
                    minOrthoSize, maxOrthoSize
                );
            }
            else
            {
                cam.fieldOfView = Mathf.Clamp(
                    cam.fieldOfView - scroll * zoomSpeed,
                    minFov, maxFov
                );
            }
        }
    }

    /// <summary>
    /// 이동 후 위치를 min/max 값 안으로 제한
    /// </summary>
    private void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    public void SetCameraLock(bool b) {
        isLocked = b;
        if (b) transform.position = originCamPosition;
    }
}
