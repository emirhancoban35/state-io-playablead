using System.Collections;
using UnityEngine;
using Enums;

[DisallowMultipleComponent]
public class CombatManager : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField, Range(0.1f, 1f), Tooltip("Kuleye tıklandığında askerlerin yüzde kaçı yola çıkacak?")] 
    private float _sendRatio = 0.5f; 
    
    [SerializeField] private float _spawnDelay = 0.05f; 
    private WaitForSeconds _cachedSpawnDelay; 

    private void Awake()
    {
        _cachedSpawnDelay = new WaitForSeconds(_spawnDelay); 
    }

    private void OnEnable()
    {
        InputManager.OnAttackIssued += HandleAttackIssued;
        Unit.OnTargetReached += HandleTargetReached; 
    }

    private void OnDisable()
    {
        InputManager.OnAttackIssued -= HandleAttackIssued;
        Unit.OnTargetReached -= HandleTargetReached;
    }

    private void HandleAttackIssued(AttackData attackData)
    {
        int unitsToSend = Mathf.FloorToInt(attackData.StartNode.UnitCount * _sendRatio); 
        
        if (unitsToSend <= 0) return; 

        int remainingUnits = attackData.StartNode.UnitCount - unitsToSend;
        attackData.StartNode.ChangeTeam(attackData.StartNode.CurrentTeam, remainingUnits);

        StartCoroutine(SpawnUnitsRoutine(attackData.StartNode, attackData.TargetNode, unitsToSend));
    }

    private IEnumerator SpawnUnitsRoutine(BaseNode startNode, BaseNode targetNode, int amount)
    {
        TeamType team = startNode.CurrentTeam;
        Color teamColor = NodeManager.Instance.GetTeamColor(team); // Rengi NodeManager'dan aldık

        for (int i = 0; i < amount; i++)
        {
            Unit unit = PoolManager.Instance.GetUnit();
            if (unit == null) continue; 

            Vector3 spawnPos = startNode.transform.position + (Vector3)Random.insideUnitCircle * 0.3f;
            unit.Initialize(team, targetNode, spawnPos, teamColor);

            yield return _cachedSpawnDelay; 
        }
    }

    private void HandleTargetReached(BaseNode targetNode, TeamType attackingTeam, Unit unit)
    {
        PoolManager.Instance.ReturnUnit(unit);

        if (targetNode.CurrentTeam == attackingTeam)
        {
            ResolveFriendlyArrival(targetNode);
        }
        else
        {
            ResolveEnemyArrival(targetNode, attackingTeam);
        }
    }

    private void ResolveFriendlyArrival(BaseNode targetNode)
    {
        targetNode.ChangeTeam(targetNode.CurrentTeam, targetNode.UnitCount + 1);
    }

    private void ResolveEnemyArrival(BaseNode targetNode, TeamType attackingTeam)
    {
        int newCount = targetNode.UnitCount - 1;

        if (newCount <= 0)
        {
            targetNode.ChangeTeam(attackingTeam, 1);
        }
        else
        {
            targetNode.ChangeTeam(targetNode.CurrentTeam, newCount);
        }
    }
}