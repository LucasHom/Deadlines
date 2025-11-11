using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class WorldDraggable : MonoBehaviour
{
    //types
    [SerializeField] public string type = default;
    public static List<string> acceptableTypes = new List<string>();

    [Header("Drag Limits")]
    [SerializeField] private float minBorderX = -6.5f;
    [SerializeField] private float minBorderY = -3.1f;
    [SerializeField] private float maxBorderX = 6.5f;
    [SerializeField] private float maxBorderY = 3.1f;

    private Camera cam;
    private bool isDragging;
    private Vector3 offset;

    // We'll store the mask here
    private int sortGroupMask;

    //tracking files
    public static int ActiveFiles = 0;
    public SortGroup sortGroup;
    private GameManager gameManager;    

    //Sprites
    [Header("Sprite Settings")]

    public static List<Sprite> typeSprites = new List<Sprite>();

    private SpriteRenderer spriteRenderer;
    public static Dictionary<string, Sprite> typeSpriteMap = new Dictionary<string, Sprite>();

    

    void Start()
    {
        ActiveFiles++;
        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;

        //types
        type = acceptableTypes[Random.Range(0, acceptableTypes.Count)];

        //rb2d
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;

        // gives u a bitmask that includes ONLY that layer
        sortGroupMask = LayerMask.GetMask("SortGroup");

        //Set sprites
        UpdateSpriteByType();
    }


    public static void BuildTypeSpriteDictionary()
    {
        typeSpriteMap.Clear();


        // 2. Copy and shuffle sprites
        List<Sprite> shuffledSprites = new List<Sprite>(typeSprites);
        for (int i = 0; i < shuffledSprites.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledSprites.Count);
            (shuffledSprites[i], shuffledSprites[randomIndex]) = (shuffledSprites[randomIndex], shuffledSprites[i]);
        }

        for (int i = 0; i < acceptableTypes.Count; i++)
        {
            string key = acceptableTypes[i];
            Sprite value = shuffledSprites[i];

            typeSpriteMap.Add(key, value);
        }
    }


    // Sets the sprite based on the object's current type string.
    public void UpdateSpriteByType()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (typeSpriteMap.TryGetValue(type, out Sprite newSprite))
        {
            spriteRenderer.sprite = newSprite;
            if (type == "trash")
            {
                gameManager.isTrash.sprite = newSprite;
                gameManager.isTrash.SetNativeSize();
            }
            if (type == "good")
            {
                gameManager.isFolder.sprite = newSprite;
                gameManager.isFolder.SetNativeSize();
            }   
            //add more for future folders
        }
        else
        {
            Debug.LogWarning($"No sprite found for type '{type}'");
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Started dragging " + type);
        isDragging = true;

        // Calculate offset from click point to object center
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (sortGroup != null)
        {
            if (sortGroup.objTouching != null)
            {
                WorldDraggable draggable = sortGroup.objTouching.GetComponent<WorldDraggable>();
                if (draggable != null)
                {
                    sortGroup.TrySort(draggable);
                }
            }
        }
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;

            // Restrict movement within borders
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minBorderX, maxBorderX),
                Mathf.Clamp(transform.position.y, minBorderY, maxBorderY),
                transform.position.z
            );
        }
    }

    public bool IsDragging => isDragging;
}
