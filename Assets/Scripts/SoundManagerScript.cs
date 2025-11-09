using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public GameObject metronome;
    public GameObject drumLoop;
    private float delaySeconds = 2.0f; // duration for spawned notes to hit the bar
    private float currentTime = 0;
    private bool musicStarted = false;
    private List<GameObject> trackList;
    private int currentTrackIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        trackList = new List<GameObject>() { drumLoop };
        // IDK how to sync this lol
        // StartTrack(metronome);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > delaySeconds && !musicStarted) // can add like a record button to activate as well
        {
            musicStarted = true;
            StartTrack(trackList[currentTrackIndex]);
        }
    }
    
    void StartTrack(GameObject track)
    {
        Instantiate(track, transform.position, transform.rotation); 
    }
}
