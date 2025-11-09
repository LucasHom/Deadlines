using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class WorldDraggable : MonoBehaviour
{
    //types
    [SerializeField] public string type = default;
    [SerializeField] public List<string> acceptableTypes = new List<string>();

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

    //Sprites
    [Header("Sprite Settings")]
    [SerializeField] public List<Sprite> typeSprites = new List<Sprite>();

    private SpriteRenderer spriteRenderer;
    private Dictionary<string, Sprite> typeSpriteMap = new Dictionary<string, Sprite>();

    void Start()
    {
        ActiveFiles++;
        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;

        //types
        type = acceptableTypes[Random.Range(0, acceptableTypes.Count)];

        // Make sure this object has a Rigidbody2D set to Kinematic
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;

        // LayerMask.GetMask("SortGroup") gives you a bitmask that includes ONLY that layer
        sortGroupMask = LayerMask.GetMask("SortGroup");
    }


    // Builds a dictionary mapping each acceptable type to its corresponding sprite.
    private void BuildTypeSpriteDictionary()
    {
        typeSpriteMap.Clear();

        // Ensure the lists match in length
        if (acceptableTypes.Count != typeSprites.Count)
        {
            Debug.LogWarning("acceptableTypes and typeSprites must have the same length!");
            return;
        }

        for (int i = 0; i < acceptableTypes.Count; i++) //Make this random
        {
            string key = acceptableTypes[i];
            Sprite value = typeSprites[i];

            if (!typeSpriteMap.ContainsKey(key))
                typeSpriteMap.Add(key, value);
        }
    }


    // Sets the sprite based on the object's current type string.
    public void UpdateSpriteByType()
    {
        if (spriteRenderer == null) return;

        if (typeSpriteMap.TryGetValue(type, out Sprite newSprite))
        {
            spriteRenderer.sprite = newSprite;
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

        // Raycast from camera through the mouse position
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // This ray will ONLY consider colliders on the "SortGroup" layer
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, sortGroupMask);

        if (hit.collider != null)
        {
            // Get the SortGroup component on the hit object (or its parent)
            SortGroup sortGroup = hit.collider.GetComponentInParent<SortGroup>();
            if (sortGroup != null)
            {
                sortGroup.TrySort(this);
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
