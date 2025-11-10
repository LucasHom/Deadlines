using System.Collections;
using UnityEngine;

public class NoteSpawnScript : MonoBehaviour
{
    [Header("Note Settings")]
    public GameObject note;
    public GameObject bar;
    public int totalNotes = 8;
    public int BPM = 95;

    [SerializeField] private AmbitionManager ambitionManager;

    private float spawnInterval;
    private int notesSpawned = 0;
    private Coroutine spawnRoutine;

    [SerializeField] private GameObject camera;
    private float greyscale = 1.0f;
    private float decreaseValue = 1.0f / 3.0f;

    private void OnEnable()
    {
        spawnInterval = 60f / BPM;

        SpawnWave();

        spawnRoutine = StartCoroutine(SpawnNotesRoutine());
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
    }

    private void SpawnWave()
    {
        notesSpawned = 0;
        if (bar != null)
        {
            BarScript barScript = bar.GetComponent<BarScript>();
            if (barScript != null)
                barScript.notesHit = 0;
        }
    }

    private IEnumerator SpawnNotesRoutine()
    {
        while (notesSpawned < totalNotes)
        {
            SpawnNote();
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitUntil(() => NoteScript.ActiveNotes <= 0);
        yield return new WaitForSeconds(1f);
        //create confetti effect here if success
        if (greyscale >= decreaseValue-0.1f)
        {
            greyscale -= decreaseValue;
        }
        GreyscaleScript greyscaleScript = camera.GetComponent<GreyscaleScript>();
        greyscaleScript.SetGreyscalePercentage(greyscale);

        ambitionManager.ShowRecordIcon();
    }

    private void SpawnNote()
    {
        Instantiate(note, transform.position, transform.rotation);
        notesSpawned++;
    }
}
