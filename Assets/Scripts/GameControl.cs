using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public GameObject gameOverText;

	public GameObject bonusText;

	public GameObject flapFlapText;

	public bool gameOver = false;
    private int score;
	private float bonusTimeCounter=0f;

    public float scrollSpeed = 2f;
	private float bonusTime = 3f;

    public static GameControl instance;

    public Text scoreText;

	public double totalTimePassed=0;

	public AudioClip bonusClip;
	public AudioSource bonusSource;

    void Awake()
    {
		
        if(instance==null)
        {
            instance = this;
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }

		flapFlapText.SetActive (true);
		bonusSource.clip = bonusClip;
	}



    private void Update()
    {
		totalTimePassed = totalTimePassed + (double)Time.deltaTime;

		if (bonusTimeCounter > 0.85) {
			bonusText.SetActive (false);
		}

		if (totalTimePassed <= 2.5 + 2 * (double)Time.deltaTime) {
			
			if (totalTimePassed >= 2.5) {
				flapFlapText.SetActive (false);
			} else if (totalTimePassed >= 2) {
				flapFlapText.SetActive (true);
			} else if (totalTimePassed >= 1.5) {
				flapFlapText.SetActive (false);
			} else if (totalTimePassed >= 1) {
				flapFlapText.SetActive (true);
			} else if (totalTimePassed >= 0.5) {
				flapFlapText.SetActive (false);
			}
		} 
		else if(!gameOver){
			bonusTimeCounter = bonusTimeCounter + (float)Time.deltaTime;	
		}
		if (gameOver == true && Bird.player.IsPlayerFound() && (Input.GetMouseButtonDown(0) || (Bird.player.GetElbowRight() > Bird.player.GetHeadY())))
        {
			totalTimePassed = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
		if (bonusTimeCounter >= bonusTime) {
			bonusSource.Play ();
			bonusText.SetActive (true);
			score++;
			bonusTimeCounter = 0;
			bonusTime = bonusTime + 0.25f;
		}
    }

    public void BirdDied()
    {
		bonusText.SetActive (false);
        gameOverText.SetActive(true);
        gameOver = true;
    }

    public void BirdScored()
    {
        if (gameOver)
        {
            return;
        }
        score++;
        scoreText.text = "Score :" + score.ToString();
    }
}
