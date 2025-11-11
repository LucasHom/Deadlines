using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Sprite triangleSprite;


    [SerializeField] private AmbitionManager ambitionManager;
    [SerializeField] private GameObject boss;

    //UI
    [SerializeField] private GameObject bossMessage;

    [SerializeField] public Image isFolder;
    [SerializeField] public Image isTrash;
    [SerializeField] public Image isDropbox;


    [Header("Adding folder")]
    [SerializeField] private TextMeshProUGUI equalsText;
    [SerializeField] private Image dropboxFolderImage;
    [SerializeField] private GameObject dropboxFolder;
    [SerializeField] private Image isDropboxImage;


    // Start is called before the first frame update
    void Start()
    {
        equalsText.text = "=\n=\n";
        dropboxFolderImage.enabled = false;
        dropboxFolder.SetActive(false);
        isDropboxImage.enabled = false;

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

        int files = 0;

        //one deadline for each hour
        for (int hour = 0; hour < 8; hour++)
        {

            switch (hour)
            {
                case 0:
                    
                    files = 10;
                    ambitionManager.ShowRecordIcon();
                    break;

                case 1:
                    files = 12;
                    break;

                case 2:
                    files = 14;
                    break;

                case 3:
                    files = 16;
                    break;

                case 4:
                    files = 8;
                    //add dropbox folder
                    equalsText.text = "=\n=\n=";
                    dropboxFolderImage.enabled = true;
                    dropboxFolder.SetActive(true);
                    isDropboxImage.enabled = true;
                    WorldDraggable.acceptableTypes.Add("dropbox");
                    WorldDraggable.typeSprites.Add(triangleSprite);
                    break;

                case 5:
                    files = 10;
                    break;

                case 6:
                    files = 12;
                    break;

                case 7:
                    files = 12;
                    break;
            }

            WorldDraggable.BuildTypeSpriteDictionary();


            yield return StartCoroutine(WorkShift(files));
            
        }

    }

    private IEnumerator WorkShift(int numToSort)
    {
        boss.SetActive(true);

        int sortOrder = 20;

        //Spawn files
        for (int i = 0; i < numToSort; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(fileSpawnMinX, fileSpawnMaxX), Random.Range(fileSpawnMinY, fileSpawnMaxY), 0);
            GameObject file = Instantiate(filePrefab, spawnPosition, Quaternion.identity);
            WorldDraggable fileScript = file.GetComponent<WorldDraggable>();
            fileScript.type = WorldDraggable.acceptableTypes[Random.Range(0, WorldDraggable.acceptableTypes.Count)];

            file.GetComponent<SpriteRenderer>().sortingOrder = sortOrder;
            sortOrder++;

            yield return new WaitForSeconds(0.08f);
        }

        //setup timer
        yield return new WaitForSeconds(TimeScript.fiveMinuteLength + 1f);

        //wait until timer reaches certain point
        yield return new WaitUntil(() => TimeScript.atDeadline); 


        int percentCorrect = ((numToSort - numIncorrect) * 100) / numToSort;

        if (percentCorrect < winPercentThreshold || WorldDraggable.ActiveFiles > 0) // or timer reached end before sorting all files    
        {
            Debug.Log("Shift over, you lose!");
            // Time.timeScale = 0f;
            ambitionManager.GameOver();
        }
        else
        {
            Debug.Log(percentCorrect + "%");
        }
        
    }
}
