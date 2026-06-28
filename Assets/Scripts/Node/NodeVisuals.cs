using UnityEngine;
using UnityEngine.UI;
using Enums;
using Config;

[DisallowMultipleComponent]
public class NodeVisuals : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text _unitText;
    [SerializeField] private SpriteRenderer _circleSprite;
    [SerializeField] private SpriteRenderer _mapSprite;
    [SerializeField] private GameObject _highlightRing;

    private Vector3 _originalScale;
    private Vector3 _targetScale;
    private Color _targetCircleColor;
    private Color _targetMapColor;
    
    private PlayableConfig _config;
    private NodeManager _nodeManager;

    private void Awake()
    {
        _originalScale = transform.localScale;
        _targetScale = _originalScale;
        if (_highlightRing != null) _highlightRing.SetActive(false);
    }

    public void Initialize(NodeManager nodeManager)
    {
        _nodeManager = nodeManager;
        _config = nodeManager.Config;
        
        _targetCircleColor = _config.NeutralColor;
        _targetMapColor = _config.NeutralColor;
    }

    public void UpdateVisuals(TeamType team, int count, int maxCount)
    {
        if (_config == null || _nodeManager == null) return;

        if (_unitText != null) 
            _unitText.text = _nodeManager.GetCachedNumber(count);

        Color baseColor = _config.GetTeamColor(team);

        float intensityRatio = Mathf.Clamp01((float)count / maxCount);
        _targetCircleColor = Color.Lerp(Color.white, baseColor, 0.4f + (intensityRatio * 0.6f)); 
        _targetMapColor = Color.Lerp(Color.white, baseColor, 0.15f + (intensityRatio * 0.35f)); 
    }
    public void PlayPulseAnimation()
    {
        if (_config != null)
            transform.localScale = _originalScale * _config.HeartbeatPulseAmount;
    }

    public void PlayDamageAnimation()
    {
        if (_config != null)
        {
            float randomPunch = Random.Range(0f, 0.05f); 
            transform.localScale = _originalScale * (_config.DamageShrinkAmount - randomPunch);
        }
    }
    public void SetHighlight(bool isHighlighted)
    {
        if (_highlightRing != null) _highlightRing.SetActive(isHighlighted);
    }

    public void TickVisuals(float deltaTime)
    {
        if (_config == null) return;

        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, deltaTime * _config.ScaleLerpSpeed);

        if (_circleSprite != null)
            _circleSprite.color = Color.Lerp(_circleSprite.color, _targetCircleColor, deltaTime * _config.ColorLerpSpeed);

        if (_mapSprite != null)
            _mapSprite.color = Color.Lerp(_mapSprite.color, _targetMapColor, deltaTime * _config.ColorLerpSpeed);
    }
}