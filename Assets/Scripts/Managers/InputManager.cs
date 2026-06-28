using System;
using UnityEngine;

[DisallowMultipleComponent]
public class InputManager : MonoBehaviour
{
    public static event Action<BaseNode> OnDragStarted;
    public static event Action<Vector2> OnDragUpdated;
    public static event Action OnDragCanceled;
    
    [Header("Settings")]
    [SerializeField] private LayerMask _nodeLayer;
    
    private Camera _mainCamera;
    private BaseNode _startNode;
    private BaseNode _hoveredNode;
    public static bool CanInput { get; set; } = true;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!CanInput) return;

        if (Input.GetMouseButtonDown(0))
            StartDrag();
        else if (Input.GetMouseButton(0) && _startNode != null)
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
        OnDragUpdated?.Invoke(GetMouseWorldPosition());

        BaseNode currentNode = GetNodeUnderPointer();

        if (currentNode != _hoveredNode)
        {
            if (_hoveredNode != null) _hoveredNode.Visuals.SetHighlight(false);
            
            _hoveredNode = currentNode;

            if (_hoveredNode != null && _hoveredNode != _startNode)
            {
                _hoveredNode.Visuals.SetHighlight(true);
            }
        }
    }

    private void EndDrag()
    {
        BaseNode targetNode = GetNodeUnderPointer();

        if (targetNode != null && targetNode != _startNode)
        {
            GameEvents.OnAttackIssued?.Invoke(new AttackData { StartNode = _startNode, TargetNode = targetNode });
        }

        if (_hoveredNode != null)
        {
            _hoveredNode.Visuals.SetHighlight(false);
            _hoveredNode = null;
        }

        OnDragCanceled?.Invoke();
        _startNode = null;
    }

    private BaseNode GetNodeUnderPointer()
    {
        Vector2 mousePos = GetMouseWorldPosition();
        Collider2D col = Physics2D.OverlapPoint(mousePos, _nodeLayer);
        return col != null ? col.GetComponent<BaseNode>() : null;
    }

    private Vector2 GetMouseWorldPosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}