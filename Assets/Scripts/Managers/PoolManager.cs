using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private int _initialPoolSize = 100; 

    private Queue<Unit> _unitPool = new Queue<Unit>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreateNewUnitToPool();
        }
    }

    private Unit CreateNewUnitToPool()
    {
        Unit newUnit = Instantiate(_unitPrefab, transform); 
        newUnit.gameObject.SetActive(false);
        _unitPool.Enqueue(newUnit);
        return newUnit;
    }
    
    public Unit GetUnit()
    {
        if (_unitPool.Count == 0)
        {
            return CreateNewUnitToPool(); 
        }

        Unit unit = _unitPool.Dequeue();
        return unit;
    }

    public void ReturnUnit(Unit unit)
    {
        unit.gameObject.SetActive(false); 
        _unitPool.Enqueue(unit);
    }
}