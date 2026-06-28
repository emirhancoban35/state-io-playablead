using UnityEngine;
using Enums;

[DisallowMultipleComponent]
public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Settings")]
    [SerializeField, Tooltip("Ekranda sürüklenecek el/imleç UI objesi")] 
    private RectTransform _handPointer;
    
    [SerializeField, Tooltip("Elin nereden başlayacağı (Mavi Kule)")] 
    private BaseNode _startNode;
    
    [SerializeField, Tooltip("Elin nereye gideceği (Hedef Kule)")] 
    private BaseNode _targetNode;
    
    [SerializeField, Range(0.5f, 5f)] 
    private float _animationSpeed = 1.5f;

    private Camera _mainCamera;
    private bool _isTutorialActive = true;
    private float _animationProgress = 0f;

    private void Start()
    {
        _mainCamera = Camera.main;

        InputManager.OnDragStarted += StopTutorial;

        if (_startNode == null || _targetNode == null || _handPointer == null)
        {
            StopTutorial(null);
        }
    }

    private void OnDestroy()
    {
        InputManager.OnDragStarted -= StopTutorial;
    }

    private void StopTutorial(BaseNode node)
    {
        _isTutorialActive = false;
        if (_handPointer != null) 
            _handPointer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isTutorialActive || _handPointer == null) return;

        Vector3 startScreenPos = _mainCamera.WorldToScreenPoint(_startNode.transform.position) + new Vector3(40, -40, 0);
        Vector3 targetScreenPos = _mainCamera.WorldToScreenPoint(_targetNode.transform.position) + new Vector3(40, -40, 0);

        _animationProgress += Time.deltaTime * _animationSpeed;
        if (_animationProgress > 1.2f) 
        {
            _animationProgress = 0f;
        }

        float t = Mathf.Clamp01(_animationProgress);
        float easeT = 1f - Mathf.Pow(1f - t, 3f);

        _handPointer.position = Vector3.Lerp(startScreenPos, targetScreenPos, easeT);
        
        if (_animationProgress < 0.1f) 
        {
            _handPointer.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one * 0.8f, _animationProgress * 10f);
        }
        else if (_animationProgress > 1f)
        {
            _handPointer.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one * 1.2f, (_animationProgress - 1f) * 5f);
        }
        else
        {
            _handPointer.localScale = Vector3.one * 0.8f;
        }
    }
}