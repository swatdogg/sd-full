using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class GameScene : MonoBehaviour {
	
	private int wave = 1;
	private PlayerController playerController;
	private EnemySpawner enemySpawner;
	private MusicManager musicManager;
	private ScoreManager scoreManager;
	private SFXManager sfxManager;
	private int difficulty;
	private AudioClip currentMusic;
	private int musicChoiceArrayNumber;
	private float startTime;
	private string gameMode;
	
	public GameObject playerHealthBar;
	public AudioClip[] gameMusicArray;
	public AudioClip bossMusic;
	public GameObject pauseButton;
	public GameObject pauseMenu;
	public GameObject musicSlider;
	public GameObject sfxSlider;
	public Slider musicVolumeSlider;
	public Slider sfxVolumeSlider;
	
	// Use this for initialization
	void Start () {
		HZIncentivizedAd.fetch("default");
		HZBannerAd.hide();
		
		pauseMenu.SetActive(false);
		musicSlider.SetActive(false);
		sfxSlider.SetActive(false);
		difficulty = PlayerPrefsManager.GetDifficulty();
		gameMode = PlayerPrefsManager.GetGameMode();
		scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		playerController = GameObject.FindObjectOfType<PlayerController>();
		enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
		musicManager = GameObject.FindObjectOfType<MusicManager>();
		sfxManager = GameObject.FindObjectOfType<SFXManager>();
		
		playerController.setPlayerShip(PlayerPrefsManager.GetPlayerShip());
		enemySpawner.EnemyNumber(wave);
		StartMusic();
		ReportDifficulty();
	}
	
	// Update is called once per frame
	void Update () {
		if(sfxVolumeSlider.IsActive())
		{
			musicManager.ChangeVolume (musicVolumeSlider.value);
			sfxManager.ChangeVolume (sfxVolumeSlider.value);
		}
		
		if (Time.time >= (startTime + currentMusic.length))
		{
			NextSong();
		}
		
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			OnPause();
		}
	}
	
	void StartMusic()
	{
		musicChoiceArrayNumber = Random.Range(0,5);
		musicManager.PlayGameMusic(gameMusicArray[musicChoiceArrayNumber],false);
		currentMusic = gameMusicArray[musicChoiceArrayNumber];
		startTime = Time.time;
	}
	
	void NextSong()
	{
		if(musicChoiceArrayNumber + 1 == gameMusicArray.Length)
		{
			musicManager.PlayGameMusic(gameMusicArray[0], false);
			musicChoiceArrayNumber = 0;
			currentMusic = gameMusicArray[musicChoiceArrayNumber];
			startTime = Time.time;
		}
		else
		{
			musicChoiceArrayNumber++;
			musicManager.PlayGameMusic(gameMusicArray[musicChoiceArrayNumber], false);
			currentMusic = gameMusicArray[musicChoiceArrayNumber];
			startTime = Time.time;
		}
	}
	
	public int GetWave()
	{
		return wave;
	}
	
	public void NextWave()
	{
		if( (wave) % 5 == 0)
		{
			ReportNonBossWave();
		}
		
		if (gameMode == "Wave")
		{
			if(difficulty == 1)
			{
				if(wave < 10)
				{
					wave ++;
					enemySpawner.EnemyNumber(wave);
				}
				else if (wave >= 10)
				{
					scoreManager.SetCurrentScore();
					Invoke("YouWin", 1.0f);
				}
			}
			else if(difficulty == 2)
			{
				if(wave < 30)
				{
					wave ++;
					enemySpawner.EnemyNumber(wave);
				}
				else if (wave >= 30)
				{
					scoreManager.SetCurrentScore();
					Invoke("YouWin", 1.0f);
				}
			}
			else if (difficulty == 3)
			{
				if(wave < 50)
				{
					wave ++;
					enemySpawner.EnemyNumber(wave);
				}
				else if (wave >= 50)
				{
					scoreManager.SetCurrentScore();
					Invoke("YouWin", 1.0f);
				}
			}
		}
		else if (gameMode == "Endless")
		{
			wave ++;
			enemySpawner.EnemyNumber(wave);
		}
		else if (gameMode == "Boss")
		{
			ReportBossWave();
			
			if(wave < 5)
			{
				wave ++;
				enemySpawner.EnemyNumber(wave);
			}
			else if (wave >= 5)
			{
				scoreManager.SetCurrentScore();
				Invoke("YouWin", 1.0f);
			}
		}
		
		if(wave % 10 == 0)
		{
			musicManager.PlayGameMusic(bossMusic, true);
		}
		else if(wave % 10 == 1)
		{
			NextSong();
		}
	}
	
	void YouWin()
	{
		GoToResults("You WIN!");
	}
	
	public void OnPause()
	{
		playerHealthBar.SetActive(false);
		pauseButton.SetActive(false);
		musicManager.PauseMusic();
		sfxManager.Pause();
		pauseMenu.SetActive(true);
		musicSlider.SetActive(true);
		sfxSlider.SetActive(true);
		musicVolumeSlider.value = PlayerPrefsManager.GetMasterVolume();
		sfxVolumeSlider.value = PlayerPrefsManager.GetSFXVolume();
	}
	
	public void OnUnPause()
	{
		playerHealthBar.SetActive(true);
		musicManager.UnPauseMusic();
		sfxManager.UnPause();
		pauseMenu.SetActive(false);
		musicSlider.SetActive(false);
		sfxSlider.SetActive(false);
		pauseButton.SetActive(true);
		Time.timeScale = 1.0f;
	}
	
	public void SetMusicVolume()
	{
		PlayerPrefsManager.SetMasterVolume(musicVolumeSlider.value);
		PlayerPrefsManager.SetSFXVolume(sfxVolumeSlider.value);
	}
	
	public void GoToResults(string result)
	{
		PlayerPrefsManager.SetResult(result);
		LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
		levelManager.LoadLevel("Results");
	}
	
	void ReportNonBossWave()
	{
		Analytics.CustomEvent("waveCompletion", new Dictionary<string, object>
		{
			{"wavePassed", "Wave: " + wave.ToString()}
		});
	}
	
	void ReportBossWave()
	{
		Analytics.CustomEvent("bossWaveReached", new Dictionary<string, object>
		{
			{"BossWaveReached", "Reached Boss: " + wave.ToString()}
		});
	}
	
	void ReportDifficulty()
	{
		Analytics.CustomEvent("difficultyPlaying", new Dictionary<string, object>
		{
			{"DiffcultyPlayed", "Difficulty: " + difficulty.ToString()}
		});
	}
}
