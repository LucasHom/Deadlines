using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int numIncorrect;

    [SerializeField] private int winPercentThreshold = 80;

    [SerializeField] private GameObject filePrefab;
    //[SerializeField] private float workShiftDuration = 30f;
    [SerializeField] private float fileSpawnMinX = -5f;
    [SerializeField] private float fileSpawnMaxX = -1f;
    [SerializeField] private float fileSpawnMinY = -1.5f;
    [SerializeField] private float fileSpawnMaxY = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        numIncorrect = 0;
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GameLoop()
    {
        //one deadline for each hour
        for (int hour = 0; hour < 8; hour++)
        {
            yield return StartCoroutine(WorkShift(10));
        }

    }

    private IEnumerator WorkShift(int numToSort)
    {
        //Spawn files
        for (int i = 0; i < numToSort; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(fileSpawnMinX, fileSpawnMaxX), Random.Range(fileSpawnMinY, fileSpawnMaxY), 0);
            GameObject file = Instantiate(filePrefab, spawnPosition, Quaternion.identity);
            WorldDraggable fileScript = file.GetComponent<WorldDraggable>();
            fileScript.type = fileScript.acceptableTypes[Random.Range(0, fileScript.acceptableTypes.Count)];

            yield return new WaitForSeconds(0.08f);
        }

        //setup timer


        yield return new WaitUntil(() => WorldDraggable.ActiveFiles == 0); //eventually wait untill timer reaches certain point dont check for 0 files here


        int percentCorrect = ((numToSort - numIncorrect) * 100) / numToSort;

        if (percentCorrect < winPercentThreshold) // or timer reached end before sorting all files    
        {   
            Debug.Log("Shift over! You sorted only " + percentCorrect + "% of files correctly. You lose!");
        }
        else
        {
            Debug.Log("Shift over! You sorted " + percentCorrect + "% of files correctly.");
        }
        
        //reset timer but dont start

    }
}
