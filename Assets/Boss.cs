using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float bobAmplitude = 0.5f;
    [SerializeField] private float bobFrequency = 2f;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float exitX = 12f;
    [SerializeField] private bool facingRight = true;

    private Vector3 startPos;
    private bool isBobbing = false;
    private Coroutine bobRoutine;

    [SerializeField] private GameObject bossArm;
    [SerializeField] private GameObject bossMessage;



    private void OnEnable()
    {
        startPos = new Vector3(-10f, 3, 0);
        transform.position = startPos;
        StartCoroutine(MoveSequence());
    }

    private IEnumerator MoveSequence()
    {
        // Phase 1: Move to target while bobbing
        yield return StartCoroutine(MoveWithBob(targetPosition));

        // Stop bobbing and wait
        yield return new WaitForSeconds(0.2f);
        bossMessage.SetActive(true);
        bossArm.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        bossArm.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        bossMessage.SetActive(false);

        // Phase 2: Move offscreen while bobbing
        Vector3 exitPos = new Vector3(exitX, targetPosition.y, targetPosition.z);
        yield return StartCoroutine(MoveWithBob(exitPos));

        // Disable boss
        gameObject.SetActive(false);
    }

    private IEnumerator MoveWithBob(Vector3 target)
    {
        isBobbing = true;
        bobRoutine = StartCoroutine(BobMotion());

        // Move horizontally toward target
        while (Mathf.Abs(transform.position.x - target.x) > 0.05f)
        {
            float step = moveSpeed * Time.deltaTime * (facingRight ? 1 : -1);
            transform.position = new Vector3(transform.position.x + step, transform.position.y, transform.position.z);
            yield return null;
        }

        // Snap to final position and stop bobbing
        transform.position = new Vector3(target.x, startPos.y, transform.position.z);
        isBobbing = false;

        if (bobRoutine != null)
            StopCoroutine(bobRoutine);
    }

    private IEnumerator BobMotion()
    {
        float timer = 0f;
        while (isBobbing)
        {
            timer += Time.deltaTime * bobFrequency;
            float offset = Mathf.Sin(timer) * bobAmplitude;
            transform.position = new Vector3(transform.position.x, startPos.y + offset, transform.position.z);
            yield return null;
        }
        // Reset to baseline when bobbing stops
        transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
    }
}
