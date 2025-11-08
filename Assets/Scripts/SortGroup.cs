using UnityEngine;

[RequireComponent(typeof(Collider2D))] //basically just to ensure it has a collider for trigger detection
public class SortGroup : MonoBehaviour
{
    [SerializeField] private string type = default;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        transform.localScale = originalScale * 1.1f; // enlarge visually when hovering over with file
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        transform.localScale = originalScale; // reset scale
    }

    public void TrySort(WorldDraggable draggable)
    {
        if (draggable.type == type)
        {
            Debug.Log("Correctly sorted object of type: " + type);
        }
        else
        {
            GameManager.numIncorrect++;
        }

        Destroy(draggable.gameObject);
        WorldDraggable.ActiveFiles--;
    }
}
