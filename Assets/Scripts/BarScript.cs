using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarScript : MonoBehaviour
{
    public static int gamesWon = 0;

    public GameObject NoteSpawner;
    private bool inTrigger;
    private Collider2D currentCollider;
    public int notesHit = 0;

    //[SerializeField] private AmbitionManager ambitionManager;


    // Start is called before the first frame update
    void Start()
    {
        inTrigger = false;
        currentCollider = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger)
        {
            string keyToPress = currentCollider.gameObject.GetComponent<NoteScript>().noteLetter;
            if (Input.GetKeyDown(keyToPress))
            {
                //Debug.Log("BAM!");
                NoteScript.ActiveNotes--;
                Destroy(currentCollider.gameObject);
                notesHit++;
            }
        }

        if (notesHit == NoteSpawner.gameObject.GetComponent<NoteSpawnScript>().totalNotes)
        {
            gamesWon++;
            Debug.Log("You hit all the notes!");
            notesHit = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("touching");
        inTrigger = true;
        if (collider.CompareTag("Note"))
        {
            currentCollider = collider; 
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        
        Debug.Log("left collision");
        inTrigger = false;
        currentCollider = null;
    }
}
