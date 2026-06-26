using System;
using UnityEngine;
using Enums;
[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Unit : MonoBehaviour
{

    public static event Action<BaseNode, TeamType, Unit> OnTargetReached;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 3f;

    private SpriteRenderer _spriteRenderer;
    private BaseNode _targetNode;
    private TeamType _myTeam;
    private bool _isMoving = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(TeamType team, BaseNode target, Vector3 spawnPosition, Color teamColor)
    {
        _myTeam = team;
        _targetNode = target;
        transform.position = spawnPosition;
        _spriteRenderer.color = teamColor;

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

        if (Vector3.Distance(transform.position, _targetNode.transform.position) < 0.1f)
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