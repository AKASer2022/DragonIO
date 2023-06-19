using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private Vector3 _mouseInput = Vector3.zero;
    private Camera _mainCamera;

    private void Initialize()
    {
        _mainCamera = Camera.main;
    }
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }
    private void Update()
    {
        if (!IsOwner || !Application.isFocused) return;
        MovePlayerClient();
        CameraMovement();
    }
    
    private void MovePlayerClient()
    {
        _mouseInput.x = Input.mousePosition.x;
        _mouseInput.y = Input.mousePosition.y;
        _mouseInput.z = _mainCamera.nearClipPlane;
        Vector3 mouseWorldCoordinates = _mainCamera.ScreenToWorldPoint(_mouseInput);
        mouseWorldCoordinates.z = 0f;
        transform.position = Vector3.MoveTowards(transform.position, 
            mouseWorldCoordinates, Time.deltaTime * speed);
        
        //Вращение в сторону мыши
        if (mouseWorldCoordinates != transform.position)
        {
            Vector3 targetDirection = mouseWorldCoordinates - transform.position;
            targetDirection.z = 0f;
            transform.up = targetDirection;
        }
    }

    private void CameraMovement()
    {
        _mainCamera.transform.localPosition = new Vector3(transform.position.x, transform.position.y, -1f);
        transform.position =
            Vector2.MoveTowards(transform.position, 
                _mainCamera.transform.localPosition, Time.deltaTime);
    }    
}
