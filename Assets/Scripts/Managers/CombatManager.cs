using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Config;

[DisallowMultipleComponent]
public class CombatManager : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float _spawnDelay = 0.05f; 
    private WaitForSeconds _cachedSpawnDelay; 

    private readonly List<Unit> _activeUnits = new List<Unit>();
    
    private PlayableConfig _config;

    private void Awake()
    {
        _cachedSpawnDelay = new WaitForSeconds(_spawnDelay); 
    }
    private void Start()
    {
        _config = NodeManager.Instance.Config;
        _cachedSpawnDelay = new WaitForSeconds(_config.SpawnDelay); // Paneldeki hıza bağlandı
    }
    private void OnEnable()
    {
        GameEvents.OnAttackIssued += HandleAttackIssued;
        Unit.OnTargetReached += HandleTargetReached; 
    }

    private void OnDisable()
    {
        GameEvents.OnAttackIssued -= HandleAttackIssued;
        Unit.OnTargetReached -= HandleTargetReached;
    }

    private void HandleAttackIssued(AttackData attackData)
    {
        int unitsToSend = Mathf.FloorToInt(attackData.StartNode.UnitCount * _config.SendRatio);        
        if (unitsToSend <= 0) return; 

        int remainingUnits = attackData.StartNode.UnitCount - unitsToSend;
        attackData.StartNode.ChangeTeam(attackData.StartNode.CurrentTeam, remainingUnits);

        StartCoroutine(SpawnUnitsRoutine(attackData.StartNode, attackData.TargetNode, unitsToSend));
    }

    private IEnumerator SpawnUnitsRoutine(BaseNode startNode, BaseNode targetNode, int amount)
    {
        TeamType team = startNode.CurrentTeam;
        Color teamColor = NodeManager.Instance.GetTeamColor(team); 

        Vector3 direction = (targetNode.transform.position - startNode.transform.position).normalized;
        
        Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);

        for (int i = 0; i < amount; i++)
        {
            Unit unit = PoolManager.Instance.GetUnit();
            if (unit == null) continue; 

            float offsetMagnitude = 0f;
            int pattern = i % 3;
            if (pattern == 1) offsetMagnitude = 0.25f;
            if (pattern == 2) offsetMagnitude = -0.25f;

            Vector3 spawnPos = startNode.transform.position + (perpendicular * offsetMagnitude);
            
            unit.Initialize(team, targetNode, spawnPos, teamColor);
            _activeUnits.Add(unit);

            yield return _cachedSpawnDelay; 
        }
    }

    private void HandleTargetReached(BaseNode targetNode, TeamType attackingTeam, Unit unit)
    {
        _activeUnits.Remove(unit);
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

        if (newCount > 0)
        {
            targetNode.ChangeTeam(targetNode.CurrentTeam, newCount);
        }
        else if (newCount == 0)
        {
            targetNode.ChangeTeam(TeamType.Neutral, 0);
        }
        else 
        {
            targetNode.ChangeTeam(attackingTeam, 1);
        }
    }
    private void Update()
    {
        HandleMidAirCollisions();
    }

    private void HandleMidAirCollisions()
    {
        for (int i = _activeUnits.Count - 1; i >= 0; i--)
        {
            Unit unitA = _activeUnits[i];
            
            if (!unitA.gameObject.activeInHierarchy) continue;

            for (int j = i - 1; j >= 0; j--)
            {
                Unit unitB = _activeUnits[j];
                
                if (!unitB.gameObject.activeInHierarchy) continue;

                if (unitA.Team != unitB.Team)
                {

                    float sqrDistance = (unitA.transform.position - unitB.transform.position).sqrMagnitude;
                    
                    if (sqrDistance < 0.04f)
                    {
                        PoolManager.Instance.ReturnUnit(unitA);
                        PoolManager.Instance.ReturnUnit(unitB);
                        
                        break; 
                    }
                }
            }
        }
        _activeUnits.RemoveAll(u => !u.gameObject.activeInHierarchy);
    }
}