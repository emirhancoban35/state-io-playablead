using System.Collections.Generic;
using UnityEngine;
using Enums;
using Config;

[DisallowMultipleComponent]
public class NodeManager : MonoBehaviour
{
    [Header("Master Config")]
    [SerializeField] private PlayableConfig _config;
    public PlayableConfig Config => _config;
    public static NodeManager Instance { get; private set; }

    private readonly List<BaseNode> _nodes = new List<BaseNode>();
    private string[] _cachedNumbers;
    public const int MAX_CACHED_NUMBERS = 500;

    [Header("Global Team Colors")]
    [SerializeField] private Color _neutralColor = Color.gray;
    [SerializeField] private Color _playerColor = Color.blue;
    [SerializeField] private Color _enemyColor = Color.red;

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
            return _cachedNumbers[number];
        return number.ToString(); 
    }

    public void RegisterNode(BaseNode node)
    {
        if (!_nodes.Contains(node)) _nodes.Add(node);
    }

    public void UnregisterNode(BaseNode node)
    {
        if (_nodes.Contains(node)) _nodes.Remove(node);
    }

    public Color GetTeamColor(TeamType team)
    {
        return team switch
        {
            TeamType.Player => _playerColor,
            TeamType.Enemy => _enemyColor,
            _ => _neutralColor,
        };
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        for (int i = 0; i < _nodes.Count; i++)
        {
            _nodes[i].Tick(dt);
        }
    }
}