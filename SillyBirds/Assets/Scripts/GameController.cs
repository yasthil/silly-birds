using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for the general controlling of the game
/// </summary>
public class GameController : MonoBehaviour {

    private bool _gameRunning;
    private const float BIRD_SPAWN_MIN_Y_POS = -3.9f;
    private const float BIRD_SPAWN_MAX_Y_POS = 3.7f;
    private const float BIRD_SPAWN_MIN_X_POS = -14f;
    private const float BIRD_SPAWN_MAX_X_POS = 14f;

    // some defaults spawn positions
    private float _spawnPositionX = 0f;        
    private float _spawnPositionY = 0f;        
    private float _spawnPositionZ = -60f;       // set the z-order so we know exactly how far from the camera they should be

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
        float randomBirdYPos;
        float randomBirdXPos;

        // game loop - this should be more performant than using Update() on every frame
        while (_gameRunning)
        {
            sillyBird = ObjectPooler.Instance.GetPooledObject();
            if (sillyBird != null)
            {
                // get a random X and Y positions that's within some bounds
                randomBirdYPos = UnityEngine.Random.Range(BIRD_SPAWN_MIN_Y_POS, BIRD_SPAWN_MAX_Y_POS);
                randomBirdXPos = UnityEngine.Random.Range(BIRD_SPAWN_MIN_X_POS, BIRD_SPAWN_MAX_X_POS);

                // set the bird's position
                sillyBird.transform.position = new Vector3(randomBirdXPos, randomBirdYPos, _spawnPositionZ);

                // set it to active
                sillyBird.SetActive(true);
            }

            // hard coding spawn for every 2 seconds
            // TODO: Have a min, max spawn range and randomly choose from there
            yield return new WaitForSeconds(2);
        }
    }
	
}
