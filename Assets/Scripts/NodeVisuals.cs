using UnityEngine;
using UnityEngine.UI;
using Enums;

public class NodeVisuals : MonoBehaviour
{
    [SerializeField] private Text unitText; 
    [SerializeField] private SpriteRenderer baseSprite;

    public void UpdateVisuals(TeamType team, int count)
    {
        if (unitText != null)
            unitText.text = count.ToString();

        if (baseSprite != null)
        {
            switch (team)
            {
                case TeamType.Neutral: baseSprite.color = Color.gray; break;
                case TeamType.Player: baseSprite.color = Color.blue; break;
                case TeamType.Enemy: baseSprite.color = Color.red; break;
            }
        }
    }
}