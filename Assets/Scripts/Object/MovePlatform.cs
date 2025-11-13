using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveTime = 2f;
    public float waitTime = 1f;

    private void Start()
    {
        StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        while (true)
        {
            yield return MoveTo(pointB.position);
            yield return new WaitForSeconds(waitTime);
            yield return MoveTo(pointA.position);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = transform.position;
        float time = 0f;

        while (time < moveTime)
        {
            transform.position = Vector3.Lerp(start, target, time / moveTime);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }
}
