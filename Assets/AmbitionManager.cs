using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbitionManager : MonoBehaviour
{

    [SerializeField] private GameObject RythGame;
    [SerializeField] private GameObject RecordIcon;
    [SerializeField] private GameObject GameOverScreen;

    private NoteSpawnScript noteSpawnScript;

    

    private bool readyToRecord = false;

    // Start is called before the first frame update
    void Start()
    {
        noteSpawnScript = RythGame.GetComponent<NoteSpawnScript>();
        RythGame.SetActive(false);
        //RecordIcon.SetActive(false);
        GameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToRecord && Input.GetKeyDown(KeyCode.Space))
        {
            StartRhythmGame();
        }
    }

    public void StartRhythmGame()
    {
        NoteScript.lost = false;
        readyToRecord = false;
        RythGame.SetActive(true);
        RecordIcon.SetActive(false);
        GetComponent<SoundManagerScript>().shouldMusicPlay = true;
    }

    public void ShowRecordIcon()
    {
        readyToRecord = true;
        RecordIcon.SetActive(true);
        RythGame.SetActive(false);
        GetComponent<SoundManagerScript>().shouldMusicPlay = false;
        GetComponent<SoundManagerScript>().metronomeRunning = false;

    }

    public void ShowRecordIconFail()
    {
        Debug.Log("failed rhythm game");
        readyToRecord = true;
        RecordIcon.SetActive(true);
        RythGame.SetActive(false);
    }

    public void GameOver()
    {
        GameOverScreen.SetActive(true);
        RythGame.SetActive(false);
        RecordIcon.SetActive(false);
    }

    public void GameReset()
    {
        Debug.Log("game resetting");
        SceneManager.LoadScene("MainScene");
    }
}
