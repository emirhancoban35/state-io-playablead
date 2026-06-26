using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }

    private List<BaseNode> _allNodes = new List<BaseNode>();

    private string[] _cachedNumbers;
    private const int MAX_CACHED_NUMBERS = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeNumberCache();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeNumberCache()
    {
        _cachedNumbers = new string[MAX_CACHED_NUMBERS];
        for (int i = 0; i < MAX_CACHED_NUMBERS; i++)
        {
            _cachedNumbers[i] = i.ToString();
        }
    }

    public string GetCachedNumber(int number)
    {
        if (number >= 0 && number < MAX_CACHED_NUMBERS)
        {
            return _cachedNumbers[number];
        }
        return number.ToString(); 
    }

    public void RegisterNode(BaseNode node)
    {
        if (!_allNodes.Contains(node))
            _allNodes.Add(node);
    }

    public void UnregisterNode(BaseNode node)
    {
        if (_allNodes.Contains(node))
           _allNodes.Remove(node);
    }
    private void OnEnable()
    {
        InputManager.OnAttackIssued += HandleAttackIssued;
    }

    private void OnDisable()
    {
        InputManager.OnAttackIssued -= HandleAttackIssued;
    }

    private void HandleAttackIssued(AttackData attackData)
    {
        Debug.Log($"HÜCUM BAŞLADI: {attackData.StartNode.name} kulesinden, {attackData.TargetNode.name} kulesine saldırı emri verildi!");
        
    }
    private void Update()
    {
        float dt = Time.deltaTime;
        
        for (int i = 0; i < _allNodes.Count; i++)
        {
            _allNodes[i].Tick(dt);
        }
    }
}