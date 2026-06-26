using UnityEngine;
using UnityEngine.UI;
using Enums;

[DisallowMultipleComponent]
public class NodeVisuals : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text _unitText;
    [SerializeField] private SpriteRenderer _baseSprite;

    [Header("Team Colors")]
    [SerializeField] private Color _neutralColor = Color.gray;
    [SerializeField] private Color _playerColor = Color.blue;
    [SerializeField] private Color _enemyColor = Color.red;

    private void Awake()
    {
        Debug.Assert(_unitText != null, "Unit Text is missing on " + gameObject.name);
        Debug.Assert(_baseSprite != null, "Base Sprite is missing on " + gameObject.name);
    }

    public void UpdateText(string cachedNumber)
    {
        if (_unitText != null)
            _unitText.text = cachedNumber;
    }

    public void UpdateTeamColor(TeamType team)
    {
        if (_baseSprite == null) return;

        switch (team)
        {
            case TeamType.Neutral: _baseSprite.color = _neutralColor; break;
            case TeamType.Player: _baseSprite.color = _playerColor; break;
            case TeamType.Enemy: _baseSprite.color = _enemyColor; break;
        }
    }
}