using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for the general controlling of the game
/// </summary>
public class GameController : MonoBehaviour
{

    // public vars
    public Text _scoreText;

    private int _score;
    private bool _gameRunning;
    private const float BIRD_SPAWN_MIN_Y_POS = -10f;
    private const float BIRD_SPAWN_MAX_Y_POS = 10f;
    private const float BIRD_SPAWN_MIN_X_POS = -20f;
    private const float BIRD_SPAWN_MAX_X_POS = 20f;

    // some defaults spawn positions
    private float _spawnPositionX = 0f;
    private float _spawnPositionY = 0f;
    private float _spawnPositionZ = -40f;       // set the z-order so we know exactly how far from the camera they should be

    // Use this for initialization
    void Start()
    {
        _gameRunning = true;
        StartCoroutine(SpawnBirds());
        StartCoroutine(DisplayScore(0.5f));

    }

    private IEnumerator DisplayScore(float delay)
    {
        yield return new WaitForSeconds(delay);
        _score = 0;
        _scoreText.gameObject.SetActive(true);
        UpdateScore();
    }

    /// <summary>
    /// Updates the displayed score
    /// </summary>
    private void UpdateScore()
    {
        if (_gameRunning)
        {
            _scoreText.text = "Score: " + _score;
        }
    }

    /// <summary>
    /// Increases the score
    /// </summary>
    private void AddScore()
    {
        if (_gameRunning)
        {
            _score++;

            // update displayed text
            UpdateScore();
        }
    }

    /// <summary>
    /// Spawns Birdies using the ObjectPooler
    /// </summary>
    private IEnumerator SpawnBirds()
    {
        GameObject sillyBird = null;

        // game loop - this should be more performant than using Update() on every frame
        while (_gameRunning)
        {
            sillyBird = ObjectPooler.Instance.GetPooledObject();
            if (sillyBird != null)
            {
                // set the bird's position
                sillyBird.transform.position = CalcSpawnPositions();
                sillyBird.transform.eulerAngles = CalcSpawnRotations(sillyBird);

                // set it to active
                sillyBird.SetActive(true);
            }

            // hard coding spawn for every 2 seconds
            // TODO: Have a min, max spawn range and randomly choose from there
            yield return new WaitForSeconds(2);
        }
    }

    /// <summary>
    /// Based on the location of the bird, determines how to rotate the bird
    /// </summary>
    /// <returns></returns>
    private Vector3 CalcSpawnRotations(GameObject sillyBird)
    {
        Vector3 angles;
        if (sillyBird.transform.position.x < 0)
        {
            // rotate to face right side
            angles = new Vector3(0, 90, 0);
        }
        else
        {
            // rotate to face left side
            angles = new Vector3(0, 270, 0);
        }
        return angles;
    }

    /// <summary>
    /// Calculates the spawn positions for the birds
    /// </summary>
    /// <returns></returns>
    private Vector3 CalcSpawnPositions()
    {
        float randomBirdYPos;
        float randomBirdXPos;
        // get a random X and Y positions that's within some bounds
        randomBirdYPos = UnityEngine.Random.Range(BIRD_SPAWN_MIN_Y_POS, BIRD_SPAWN_MAX_Y_POS);

        // we need to position them off screen to then can fly in
        if (UnityEngine.Random.value > 0.5f)
        {
            // move right to left
            randomBirdXPos = BIRD_SPAWN_MIN_X_POS;
        }
        else
        {
            // move left to right
            randomBirdXPos = BIRD_SPAWN_MAX_X_POS;
        }
        return new Vector3(randomBirdXPos, randomBirdYPos, _spawnPositionZ);
    }


    private void Update()
    {
        // Android specific
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.rigidbody != null)
                    {
                        Console.Write("");
                        Destroy(hit.rigidbody.gameObject);
                        AddScore();
                    }
                }
            }
        }
        else
        {
            // Windows/Unity Editor specific
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.rigidbody != null)
                    {
                        Console.Write("");
                        hit.rigidbody.gameObject.SetActive(false);
                        AddScore();
                    }
                }
            }
        }

    }

}
