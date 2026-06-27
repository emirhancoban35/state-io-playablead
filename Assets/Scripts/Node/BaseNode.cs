using UnityEngine;
using Enums;

[RequireComponent(typeof(NodeVisuals))]
[DisallowMultipleComponent]
public class BaseNode : MonoBehaviour
{
    [Header("Node Settings")]
    [SerializeField, Tooltip("Kulenin başlangıçtaki sahibi.")] 
    private TeamType _currentTeam;
    
    [SerializeField, Tooltip("Başlangıç asker sayısı.")] 
    private int _unitCount = 10;
    
    [SerializeField, Tooltip("Maksimum birikebilecek asker sayısı.")] 
    private int _maxUnitCount = 50;
    
    [SerializeField, Tooltip("Asker üretim hızı (saniye cinsinden).")] 
    private float _spawnInterval = 1f;

    private float _timer = 0f;
    private NodeVisuals _visuals;
    
    private NodeManager _nodeManager; 
    public bool IsPlayerOwned => _currentTeam == TeamType.Player;
    public TeamType CurrentTeam => _currentTeam;
    public int UnitCount => _unitCount;

    private void Awake()
    {
        _visuals = GetComponent<NodeVisuals>();
    }

    private void Start()
    {
        _nodeManager = NodeManager.Instance; 
        
        if (_nodeManager != null)
        {
            _nodeManager.RegisterNode(this);
            
            _visuals.Initialize(_nodeManager);
        }

        UpdateVisualData();
    }

    private void OnEnable()
    {
        if (NodeManager.Instance != null)
            NodeManager.Instance.RegisterNode(this);
    }

    private void OnDisable()
    {
        if (NodeManager.Instance != null)
            NodeManager.Instance.UnregisterNode(this);
    }

    public void Tick(float deltaTime)
    {
        if (_currentTeam != TeamType.Neutral && _unitCount < _maxUnitCount)
        {
            _timer += deltaTime;
            if (_timer >= _spawnInterval)
            {
                _unitCount++;
                
                _timer -= _spawnInterval; 
                
                UpdateVisualData(); 
            }
        }
        _visuals.TickVisuals(deltaTime);
    }
    public void ChangeTeam(TeamType newTeam, int newCount)
    {
        _currentTeam = newTeam;
        _unitCount = newCount;
        UpdateVisualData();
    }
    public void TakeDamage(int newCount)
    {
        _unitCount = newCount;
        _visuals.PlayDamageAnimation();
        UpdateVisualData();
    }
    private void UpdateVisualData()
    {
        if (_nodeManager != null)
        {
            _visuals.UpdateVisuals(_currentTeam, _unitCount, _maxUnitCount);
        }
    }
}