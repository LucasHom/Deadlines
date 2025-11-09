using System.Collections;
using System.Collections.Generic;
using TMPro;
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


    [SerializeField] private Sprite squareSprite;
    [SerializeField] private Sprite circleSprite;


    [SerializeField] private AmbitionManager ambitionManager;

    //UI
    [SerializeField] private GameObject typeToShapeIndicator;


    // Start is called before the first frame update
    void Start()
    {
        typeToShapeIndicator.SetActive(false);
        numIncorrect = 0;
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GameLoop()
    {
        //define acceptable types and their sprites
        WorldDraggable.acceptableTypes.Add("good");
        WorldDraggable.acceptableTypes.Add("trash");
        WorldDraggable.typeSprites.Add(squareSprite);
        WorldDraggable.typeSprites.Add(circleSprite);

        //one deadline for each hour
        for (int hour = 0; hour < 8; hour++)
        {
            WorldDraggable.BuildTypeSpriteDictionary();

            //for debugging: print the type-sprite map
            foreach (var kvp in WorldDraggable.typeSpriteMap)
            {
                Debug.Log($"Type: {kvp.Key} -> Sprite: {(kvp.Value != null ? kvp.Value.name : "null")}");
            }

            if (hour == 0) //change to be 2nd shift later
            {
                ambitionManager.ShowRecordIcon();
            }


            yield return StartCoroutine(WorkShift(10));
            
        }

    }

    private IEnumerator WorkShift(int numToSort)
    {
        typeToShapeIndicator.SetActive(true);

        //Spawn files
        for (int i = 0; i < numToSort; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(fileSpawnMinX, fileSpawnMaxX), Random.Range(fileSpawnMinY, fileSpawnMaxY), 0);
            GameObject file = Instantiate(filePrefab, spawnPosition, Quaternion.identity);
            WorldDraggable fileScript = file.GetComponent<WorldDraggable>();
            fileScript.type = WorldDraggable.acceptableTypes[Random.Range(0, WorldDraggable.acceptableTypes.Count)];

            yield return new WaitForSeconds(0.08f);
        }

        //setup timer
        yield return new WaitForSeconds(TimeScript.fiveMinuteLength + 1f);

        typeToShapeIndicator.SetActive(false);

        //wait until timer reaches certain point
        yield return new WaitUntil(() => TimeScript.atDeadline); 


        int percentCorrect = ((numToSort - numIncorrect) * 100) / numToSort;

        if (WorldDraggable.ActiveFiles > 0)
        {
            Debug.Log("There are still " + WorldDraggable.ActiveFiles + " files left unsorted! YOU LOSE!");
            Time.timeScale = 0f;
        }
        if (percentCorrect < winPercentThreshold) // or timer reached end before sorting all files    
        {   
            Debug.Log("Shift over! You sorted only " + percentCorrect + "% of files correctly. YOU LOSE!");
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log(percentCorrect + "%");
        }
        
    }
}
