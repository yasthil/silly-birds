using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Singleton is responsible for pooling objects. Using this we can avoid 
/// constant spawning and destroying of objects in our scene.
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    public int _amountToPool;
    public static ObjectPooler Instance;
    public List<GameObject> _objectsToPool;
    private List<GameObject> _pooledObjects;
    private List<GameObject> _inActiveObjects;

    void Awake()
    {
        // Singleton implementation
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
            _inActiveObjects = new List<GameObject>();
            return;
        }
        Destroy(this.gameObject);
    }

    void Start()
    {
        // Instantiate the objectsToPool
        _pooledObjects = new List<GameObject>();
        for (int i = 0; i < _amountToPool; i++)
        {
            // randomize their order
            GameObject obj = (GameObject)Instantiate(_objectsToPool[Random.Range(0, _objectsToPool.Count)]);

            // set all as inactive
            obj.SetActive(false);
            _pooledObjects.Add(obj);
        }
    }
    
    public void RemovePooledObjects()
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            _pooledObjects[i].SetActive(false);
        }
    }

    /// <summary>
    /// Returns a random pooled GameObject that was inactive 
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                // keep a bag of inactive objects so we can randomly choose from them
                // this ensures that all items have a chance to be returned
                _inActiveObjects.Add(_pooledObjects[i]);
            }
        }

        GameObject randomGameOject = GetRandomInActiveObject();

        // clear list
        _inActiveObjects.Clear();
        return randomGameOject;
    }
    /// <summary>
    //  Select a random object from the inactive bag and return it
    /// </summary>
    /// <returns></returns>
    private GameObject GetRandomInActiveObject()
    {
        int randomIndex = Random.Range(0, _inActiveObjects.Count);
        return _inActiveObjects[randomIndex];
    }
}
