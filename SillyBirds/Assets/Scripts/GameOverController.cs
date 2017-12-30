using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public GameObject _finalScore;

	void Start ()
    {
        _finalScore.GetComponent<TextMeshProUGUI>().text = "Score: " + GameController.SharedInstance.Score;
	}

    public void RestartGame()
    {

        SceneManager.LoadScene(0);
    }
	
}
