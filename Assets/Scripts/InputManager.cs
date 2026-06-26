using System;
using UnityEngine;

[DisallowMultipleComponent]
public class InputManager : MonoBehaviour
{
    // Olaylar (Events) tamamen parçalandı. Çizgiyi çizecek adama anlık haber veriyoruz.
    public static event Action<BaseNode> OnDragStarted;
    public static event Action<Vector2> OnDragUpdated;
    public static event Action OnDragCanceled;
    public static event Action<AttackData> OnAttackIssued;

    [Header("Settings")]
    [SerializeField] private LayerMask _nodeLayer;
    
    private Camera _mainCamera;
    private BaseNode _startNode;

    // Oyunu durdurduğumuzda (Kazanma/Kaybetme) input'u kesmek için Global kilit
    public static bool CanInput { get; set; } = true;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!CanInput) return; // Oyun durduysa parmağı okuma

        if (Input.GetMouseButtonDown(0))
            StartDrag();
        else if (Input.GetMouseButton(0) && _startNode != null) // Gereksiz _isDragging silindi
            UpdateDrag();
        else if (Input.GetMouseButtonUp(0) && _startNode != null)
            EndDrag();
    }

    private void StartDrag()
    {
        BaseNode hitNode = GetNodeUnderPointer();
        
        if (hitNode != null && hitNode.IsPlayerOwned)
        {
            _startNode = hitNode;
            OnDragStarted?.Invoke(_startNode);
        }
    }

    private void UpdateDrag()
    {
        Vector2 mousePos = GetMouseWorldPosition();
        OnDragUpdated?.Invoke(mousePos);
    }

    private void EndDrag()
    {
        BaseNode targetNode = GetNodeUnderPointer();

        if (targetNode != null && targetNode != _startNode)
        {
            // Atak emri (DTO) fırlatıldı!
            OnAttackIssued?.Invoke(new AttackData { StartNode = _startNode, TargetNode = targetNode });
        }

        // Çizgiyi sıfırlaması için haber ver
        OnDragCanceled?.Invoke();
        _startNode = null;
    }

    // DRY (Don't Repeat Yourself) - Fiziksel nokta kontrolü tek bir yere toplandı
    private BaseNode GetNodeUnderPointer()
    {
        Vector2 mousePos = GetMouseWorldPosition();
        // Raycast silindi! 2D nokta kontrolü inanılmaz hafiftir.
        Collider2D col = Physics2D.OverlapPoint(mousePos, _nodeLayer);
        return col != null ? col.GetComponent<BaseNode>() : null;
    }

    private Vector2 GetMouseWorldPosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}