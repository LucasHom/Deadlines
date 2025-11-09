using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawnScript : MonoBehaviour
{
    public GameObject note;
    public GameObject bar;
    public int totalNotes = 8;
    public int BPM = 95;
    private float noteTime = 0;
    private int notesSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        notesSpawned = totalNotes;
        // SpawnWave should only be called when the GameLogic object calls it.
        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        float bpmFloat = BPM;
        float spawnTime = 1.0F / (bpmFloat / 60.0F);
        if (spawnTime <= noteTime)
        {
            SpawnNote();
            noteTime = 0;
        }
        else
        {
            noteTime += Time.deltaTime;
        }
    }

    void SpawnNote()
    {
        if (notesSpawned < totalNotes)
        {
            notesSpawned++;
            Instantiate(note, transform.position, transform.rotation);
            Debug.Log(notesSpawned);
        }
    }

    void SpawnWave()
    {
        notesSpawned = 0;
        bar.gameObject.GetComponent<BarScript>().notesHit = 0;
    }
}
