using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public GameObject metronome;
    public GameObject drumLoop;
    public GameObject bass;
    public GameObject melody;
    public GameObject fulltrack;
    private float delaySeconds = 2.0f; // duration for spawned notes to hit the bar
    private float currentTime = 0;
    private bool musicStarted = false;
    public bool shouldMusicPlay = false;
    public bool metronomeRunning = false;
    private List<GameObject> trackList;
    private GameObject currentTrack;
    public int currentTrackIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        trackList = new List<GameObject>() { drumLoop, bass, melody };
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMusicPlay)
        {
            if (!metronomeRunning)
            {
                metronomeRunning = true;
                StartTrack(metronome);
                metronome.GetComponent<AudioSource>().mute = false;
            }
            currentTime += Time.deltaTime;
            if (currentTime > delaySeconds && !musicStarted)
            {
                musicStarted = true;
                StopTrack(); // stop metronome
                StartTrack(trackList[currentTrackIndex]);
                // currentTrackIndex++; // change this to be based on next game stage (based on deadlines)
            }
        }
        else
        {
            currentTime = 0.0f;
            musicStarted = false;
            StopTrack();
        }
    }

    void StartTrack(GameObject track)
    {
        currentTrack = Instantiate(track, transform.position, transform.rotation);
        currentTrack.GetComponent<AudioSource>().mute = false;
    }

    void StopTrack()
    {
        if (currentTrack != null)
        {
            currentTrack.GetComponent<AudioSource>().mute = true;
        }
    }
    
    public void EndingCredits()
    {
        StopTrack();
        StartTrack(fulltrack);
    }
}
