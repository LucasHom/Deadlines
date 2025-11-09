using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbitionManager : MonoBehaviour
{

    [SerializeField] private GameObject RythGame;
    [SerializeField] private GameObject RecordIcon;

    private NoteSpawnScript noteSpawnScript;

    

    private bool readyToRecord = false;

    // Start is called before the first frame update
    void Start()
    {
        noteSpawnScript = RythGame.GetComponent<NoteSpawnScript>();
        RythGame.SetActive(false);
        //RecordIcon.SetActive(false);
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
    }

    public void ShowRecordIcon()
    {
        readyToRecord = true;
        RecordIcon.SetActive(true);
        RythGame.SetActive(false);
    }

    public void ShowRecordIconFail()
    {
        Debug.Log("failed rhythm game");
        readyToRecord = true;
        RecordIcon.SetActive(true);
        RythGame.SetActive(false);
    }
}
