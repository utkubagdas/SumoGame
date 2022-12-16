using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private CollectableBoost BoostPrefab;
    [SerializeField] private MeshRenderer GroundMesh;
    [SerializeField] private float TimeForNewBoost = 3f;

    public List<CollectableBoost> CollectableBoosts = new List<CollectableBoost>();

    private float _minX;
    private float _maxX;
    private float _minZ;
    private float _maxZ;

    private void OnEnable()
    {
        EventManager.OnCollectBoost.AddListener(RemoveFromList);
    }

    private void OnDisable()
    {
        EventManager.OnCollectBoost.RemoveListener(RemoveFromList);
    }

    private void Start()
    {
        AssignSpawnPointFields();

        SpawnBoost();
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
    
    
    private void SpawnBoost()
    {
        StartCoroutine(SpawnBoostCo());
    }

    private IEnumerator SpawnBoostCo()
    {
        while (true)
        {
            var boost = Instantiate(BoostPrefab);
            boost.transform.position = new Vector3(GetSpawnX(), 10, GetSpawnZ());
            boost.GetComponent<Rigidbody>().isKinematic = false;
            CollectableBoosts.Add(boost);
            yield return new WaitForSeconds(TimeForNewBoost);
        }
    }

    private void RemoveFromList(CollectableBoost collectableBoost)
    {
        CollectableBoosts.Remove(collectableBoost);
    }
}
