using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class WorldDraggable : MonoBehaviour
{
    [SerializeField] public string type = default;
    public static List<string> acceptableTypes = new List<string>();

    [Header("Drag Limits")]
    [SerializeField] private float minBorderX = -6.5f;
    [SerializeField] private float minBorderY = -3.1f;
    [SerializeField] private float maxBorderX = 6.5f;
    [SerializeField] private float maxBorderY = 3.1f;

    private static Camera cam;
    private static WorldDraggable currentlyDragging;

    private bool isDragging;
    private Vector3 offset;

    public static int ActiveFiles = 0;
    public SortGroup sortGroup;
    private GameManager gameManager;

    [Header("Sprite Settings")]
    public static List<Sprite> typeSprites = new List<Sprite>();
    private SpriteRenderer spriteRenderer;
    public static Dictionary<string, Sprite> typeSpriteMap = new Dictionary<string, Sprite>();

    void Start()
    {
        ActiveFiles++;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (cam == null) cam = Camera.main;

        // assign random type
        type = acceptableTypes[Random.Range(0, acceptableTypes.Count)];

        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;

        UpdateSpriteByType();
    }

    public static void BuildTypeSpriteDictionary()
    {
        typeSpriteMap.Clear();
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
            if (type == "dropbox")
            {
                gameManager.isDropbox.sprite = newSprite;
                gameManager.isDropbox.SetNativeSize();
            }
        }
        else
        {
            Debug.LogWarning($"No sprite found for type '{type}'");
        }
    }

    void Update()
    {
        // detect click manually so we can pick topmost
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

            if (hits.Length > 0)
            {
                // find topmost by sortingOrder
                RaycastHit2D topHit = hits[0];
                int highestOrder = int.MinValue;

                foreach (var hit in hits)
                {
                    var draggable = hit.collider.GetComponent<WorldDraggable>();
                    if (draggable != null)
                    {
                        var sr = draggable.GetComponent<SpriteRenderer>();
                        if (sr != null && sr.sortingOrder > highestOrder)
                        {
                            highestOrder = sr.sortingOrder;
                            topHit = hit;
                        }
                    }
                }

                WorldDraggable topDraggable = topHit.collider.GetComponent<WorldDraggable>();
                if (topDraggable != null)
                {
                    topDraggable.BeginDrag(mousePos);
                }
            }
        }

        if (Input.GetMouseButton(0) && currentlyDragging != null)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            currentlyDragging.ContinueDrag(mousePos);
        }

        if (Input.GetMouseButtonUp(0) && currentlyDragging != null)
        {
            currentlyDragging.EndDrag();
        }
    }

    private void BeginDrag(Vector2 mouseWorldPos)
    {
        Debug.Log("Started dragging " + type);
        isDragging = true;
        currentlyDragging = this;

        // bring visually to front
        spriteRenderer.sortingOrder = ++currentTopOrder;

        offset = transform.position - new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
    }

    private void ContinueDrag(Vector2 mouseWorldPos)
    {
        if (!isDragging) return;

        transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBorderX, maxBorderX),
            Mathf.Clamp(transform.position.y, minBorderY, maxBorderY),
            transform.position.z
        );
    }

    private void EndDrag()
    {
        isDragging = false;
        currentlyDragging = null;

        if (sortGroup != null && sortGroup.objTouching != null)
        {
            WorldDraggable draggable = sortGroup.objTouching.GetComponent<WorldDraggable>();
            if (draggable != null)
            {
                sortGroup.TrySort(draggable);
            }
        }
    }

    public bool IsDragging => isDragging;

    private static int currentTopOrder = 0;
}
