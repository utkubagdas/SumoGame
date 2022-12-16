using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OpponentSpawner : MonoBehaviour
{
    [SerializeField] private OpponentController OpponentPrefab;
    [SerializeField] private int OpponentCount = 5;
    [SerializeField] private MeshRenderer GroundMesh;
    private readonly List<OpponentController> _opponentControllers = new List<OpponentController>();
    private float _minX;
    private float _maxX;
    private float _minZ;
    private float _maxZ;

    private void Start()
    {
        AssignSpawnPointFields();
        
        SpawnOpponent();
    }

    private void AssignSpawnPointFields()
    {
        var bounds = GroundMesh.bounds;
        _minX = bounds.min.x;
        _maxX = bounds.max.x;
        _minZ = bounds.min.z;
        _maxZ = bounds.max.z;
    }

    private float GetSpawnX()
    {
        float tempX = Random.Range(_minX, _maxX);
        return tempX;
    }
    
    private float GetSpawnZ()
    {
        float tempZ = Random.Range(_minZ, _maxZ);
        return tempZ;
    }

    private void SpawnOpponent()
    {
        for (int i = 0; i < OpponentCount; i++)
        {
            var opponent = Instantiate(OpponentPrefab);
            opponent.transform.position = new Vector3(GetSpawnX(), 0, GetSpawnZ());
            _opponentControllers.Add(opponent);
        }

        foreach (var opponent in _opponentControllers)
        {
            opponent.SetAllOpponentList(_opponentControllers);   
        }
    }
}
