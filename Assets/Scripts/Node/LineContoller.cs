using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[DisallowMultipleComponent]
public class LineController : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private BaseNode _startNode;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        InputManager.OnDragStarted += HandleDragStarted;
        InputManager.OnDragUpdated += HandleDragUpdated;
        InputManager.OnDragCanceled += HandleDragCanceled;
    }

    private void OnDisable()
    {
        InputManager.OnDragStarted -= HandleDragStarted;
        InputManager.OnDragUpdated -= HandleDragUpdated;
        InputManager.OnDragCanceled -= HandleDragCanceled;
    }

    private void HandleDragStarted(BaseNode startNode)
    {
        _startNode = startNode;
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = 2;
        
        Vector3 startPos = _startNode.transform.position;
        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.SetPosition(1, startPos);
    }

    private void HandleDragUpdated(Vector2 pointerPos)
    {
        if (_lineRenderer.enabled && _startNode != null)
        {
            _lineRenderer.SetPosition(0, _startNode.transform.position); 
            _lineRenderer.SetPosition(1, pointerPos);
        }
    }

    private void HandleDragCanceled()
    {
        _lineRenderer.enabled = false; 
        _lineRenderer.positionCount = 0;
        _startNode = null;
    }
}