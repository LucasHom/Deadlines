using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    private float noteSpeed = 5.0F;
    public string noteLetter;

    public static int ActiveNotes = 0;
    public static bool lost = false;

    // Start is called before the first frame update
    void Start()
    {
        ActiveNotes++;

        int noteColumn = Random.Range(0, 4);
        transform.position = transform.position + new Vector3(noteColumn,0,0);
        List<string> keyArray = new List<string>() { "a", "s", "d", "f" };
        noteLetter = keyArray[noteColumn];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lost)
        {
            ActiveNotes--;
            Destroy(gameObject);
        }
        transform.position = transform.position + new Vector3(0, -noteSpeed * Time.deltaTime, 0);
        if (transform.position.y < -8)
        {
            lost = true;
            AmbitionManager ambitionManager = FindObjectOfType<AmbitionManager>();
            ambitionManager.ShowRecordIconFail();
            
        }
    }
}
