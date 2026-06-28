using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Enums;

[DisallowMultipleComponent]
public class UIManager : MonoBehaviour
{
    [Header("Progress Bar Settings")]
    [SerializeField] private Image _playerFillImage; 
    [SerializeField] private Image _neutralFillImage; 
    [SerializeField, Range(1f, 10f)] private float _barLerpSpeed = 5f;

    private float _targetPlayerRatio = 0f;
    private float _targetNeutralFillRatio = 0f;

    private void OnEnable()
    {
        BaseNode.OnNodeTeamChanged += CalculateMapDominance;
    }

    private void OnDisable()
    {
        BaseNode.OnNodeTeamChanged -= CalculateMapDominance;
    }

    private IEnumerator Start()
    {
        yield return null; 

        CalculateMapDominance();
        
        if (_playerFillImage != null) _playerFillImage.fillAmount = _targetPlayerRatio;
        if (_neutralFillImage != null) _neutralFillImage.fillAmount = _targetNeutralFillRatio;
    }

    private void Update()
    {
        if (_playerFillImage != null && _playerFillImage.fillAmount != _targetPlayerRatio)
            _playerFillImage.fillAmount = Mathf.Lerp(_playerFillImage.fillAmount, _targetPlayerRatio, Time.deltaTime * _barLerpSpeed);

        if (_neutralFillImage != null && _neutralFillImage.fillAmount != _targetNeutralFillRatio)
            _neutralFillImage.fillAmount = Mathf.Lerp(_neutralFillImage.fillAmount, _targetNeutralFillRatio, Time.deltaTime * _barLerpSpeed);
    }

    private void CalculateMapDominance()
    {
        var nodes = NodeManager.Instance.AllNodes;
        if (nodes == null || nodes.Count == 0) return;

        int playerNodes = 0;
        int neutralNodes = 0;
        int totalNodes = nodes.Count; 

        for (int i = 0; i < totalNodes; i++)
        {
            if (nodes[i].CurrentTeam == TeamType.Player)
                playerNodes++;
            else if (nodes[i].CurrentTeam == TeamType.Neutral)
                neutralNodes++;
        }

        _targetPlayerRatio = (float)playerNodes / totalNodes;
        _targetNeutralFillRatio = _targetPlayerRatio + ((float)neutralNodes / totalNodes);
    }
}