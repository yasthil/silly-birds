using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for the general controlling of the game
/// </summary>
public class GameController : MonoBehaviour {

    private bool _gameRunning;

	// Use this for initialization
	void Start () {

        _gameRunning = true;
        StartCoroutine(SpawnBirds());
	}

    /// <summary>
    /// Spawns Birdies using the ObjectPooler
    /// </summary>
    private IEnumerator SpawnBirds()
    {
        GameObject sillyBird = null;

        // game loop - this should be more performant than using Update() on every frame
        while(_gameRunning)
        {
            sillyBird = ObjectPooler.Instance.GetPooledObject();
            if (sillyBird != null)
            {
                // set random position for silly bird

                // set it to active
                sillyBird.SetActive(true);
            }

            // hard coding spawn for every 2 seconds
            // TODO: Have a min, max spawn range and randomly choose from there
            yield return new WaitForSeconds(2);
        }
    }
	
}
