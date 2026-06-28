using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Linq; // Listeleri filtrelemek için

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField, Tooltip("Yapay zeka kaç saniyede bir hamle düşünsün?")] 
    private float _thinkInterval = 3f;

    private WaitForSeconds _cachedThinkDelay;
    
    private bool _hasGameStarted = false;

    private void OnEnable()
    {
        // Oyuncu çizgi çekmeye (oynamaya) başladığı an AI'ı uyandır
        InputManager.OnDragStarted += WakeUpAI;
    }

    private void OnDisable()
    {
        InputManager.OnDragStarted -= WakeUpAI;
    }

    private void Start()
    {
        _cachedThinkDelay = new WaitForSeconds(_thinkInterval);
        StartCoroutine(AILoop());
    }

    private IEnumerator AILoop()
    {
        while (true)
        {
            yield return _cachedThinkDelay;
            MakeDecision();
        }
    }
    private void WakeUpAI(BaseNode node)
    {
        _hasGameStarted = true;
        
        InputManager.OnDragStarted -= WakeUpAI;
    }

    private void MakeDecision()
    {
        if (!_hasGameStarted) return;
        
        var allNodes = NodeManager.Instance.AllNodes;
        List<BaseNode> myNodes = allNodes.Where(n => n.CurrentTeam == TeamType.Enemy && n.UnitCount > 10).ToList();

        if (myNodes.Count == 0) return;

        BaseNode attackerNode = myNodes[Random.Range(0, myNodes.Count)];

        List<BaseNode> targetNodes = allNodes.Where(n => n.CurrentTeam != TeamType.Enemy).ToList();

        if (targetNodes.Count == 0) return; 

        BaseNode targetNode = targetNodes[Random.Range(0, targetNodes.Count)];


        AttackData attackCommand = new AttackData
        {
            StartNode = attackerNode,
            TargetNode = targetNode
        };
        GameEvents.OnAttackIssued?.Invoke(new AttackData { StartNode = attackerNode, TargetNode = targetNode });
    }
}