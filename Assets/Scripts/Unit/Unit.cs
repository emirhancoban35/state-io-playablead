using System;
using UnityEngine;
using Enums;

public class Unit : MonoBehaviour
{
    public static event Action<BaseNode, TeamType, Unit> OnTargetReached;

    private float _moveSpeed; 

    private SpriteRenderer _spriteRenderer;
    private BaseNode _targetNode;
    private TeamType _myTeam;
    private bool _isMoving = false;
    
    public TeamType Team => _myTeam;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Initialize(TeamType team, BaseNode target, Vector3 spawnPosition, Color teamColor, float moveSpeed)
    {
        _myTeam = team;
        _targetNode = target;
        transform.position = spawnPosition;
        _spriteRenderer.color = teamColor;

        _moveSpeed = moveSpeed;

        gameObject.SetActive(true); 
        _isMoving = true;
    }

    private void Update()
    {
        if (!_isMoving || _targetNode == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position, 
            _targetNode.transform.position, 
            _moveSpeed * Time.deltaTime
        );

        if ((_targetNode.transform.position - transform.position).sqrMagnitude < 0.01f)
        {
            ReachTarget();
        }
    }

    private void ReachTarget()
    {
        _isMoving = false;
        
        OnTargetReached?.Invoke(_targetNode, _myTeam, this);
    }
}