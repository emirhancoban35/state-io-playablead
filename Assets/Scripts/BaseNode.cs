using Enums;
using UnityEngine;

public class BaseNode : MonoBehaviour
{
    [Header("Data")]
    public TeamType currentTeam;
    public int unitCount = 10;
    public int maxUnitCount = 50;
    
    public float spawnInterval = 1f;
    private float timer = 0f;

    private NodeVisuals visuals;

    void Awake()
    {
        visuals = GetComponent<NodeVisuals>();
    }

    void Update()
    {
        if (currentTeam != TeamType.Neutral && unitCount < maxUnitCount)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                unitCount++;
                timer = 0f;
                visuals.UpdateVisuals(currentTeam, unitCount);
            }
        }
    }
}