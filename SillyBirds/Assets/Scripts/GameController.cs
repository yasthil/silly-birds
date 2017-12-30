using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Responsible for the general controlling of the game
/// </summary>
public class GameController : MonoBehaviour
{
    // constants
    private const int GAME_OVER_SCENE_INDEX = 2;

    // public vars
    public GameObject _scoreText;
    public GameObject _timerText;
    public int _timerDuration;
    public static GameController SharedInstance;
    public float _obstableSpawnIntervalMin;
    public float _obstableSpawnIntervalMax;

    private int _score;
    private float _timer;
    private bool _timerStarted;
    private bool _gameRunning;
    private const float BIRD_SPAWN_MIN_Y_POS = -7.1f;
    private const float BIRD_SPAWN_MAX_Y_POS = 6.1f;
    private const float BIRD_SPAWN_MIN_X_POS = -16f;
    private const float BIRD_SPAWN_MAX_X_POS = 16f;

    // some defaults spawn positions
    private float _spawnPositionX = 0f;
    private float _spawnPositionY = 0f;
    private float _spawnPositionZ = -40f;       // set the z-order so we know exactly how far from the camera they should be

    public int Score
    {
        get
        {
            return _score;
        }

        set
        {
            _score = value;
        }
    }


    private void Awake()
    {
        SharedInstance = this;
    }

    // Use this for initialization
    void Start()
    {
        _gameRunning = true;
        StartCoroutine(SpawnBirds());
        StartCoroutine(DisplayScore(1f));

        _timerStarted = false;
        _timer = _timerDuration;
        StartCoroutine(DisplayTimer(1f));

    }

    private IEnumerator DisplayTimer(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        _timerStarted = true;
        _timerText.gameObject.SetActive(true);
        UpdateTimer();

    }
    private IEnumerator DisplayScore(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        _score = 0;
        _scoreText.gameObject.SetActive(true);
        UpdateScore();
    }

    private void UpdateTimer()
    {
        if (_gameRunning)
        {
            _timerText.GetComponent<TextMeshProUGUI>().text = "Timer: " + _timer;
        }
    }

    /// <summary>
    /// Updates the displayed score
    /// </summary>
    private void UpdateScore()
    {
        if (_gameRunning)
        {
            _scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + _score;
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
            
            yield return new WaitForSeconds(UnityEngine.Random.Range(_obstableSpawnIntervalMin, _obstableSpawnIntervalMax));
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

    private void GameOver()
    {
        
    }

    private void Update()
    {
        if (_gameRunning && _timerStarted)
        {
            _timer -= Time.deltaTime;
            int timeLeft = (int)_timer % 60;
            _timerText.GetComponent<TextMeshProUGUI>().text = "Timer: " + timeLeft;

            if (timeLeft <= 0)
            {
                // Game over
                _gameRunning = false;

                // load game over scene
                SceneManager.LoadScene(GAME_OVER_SCENE_INDEX);
            }
        }


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
                        hit.rigidbody.gameObject.GetComponent<BirdController>().Explode();
                        hit.rigidbody.gameObject.SetActive(false);
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
                        hit.rigidbody.gameObject.GetComponent<BirdController>().Explode();
                        hit.rigidbody.gameObject.SetActive(false);
                        AddScore();
                    }
                }
            }
        }

    }

}
