using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    private Coroutine coroutine;

    void Update()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        yield return null;
        StartCoroutine(StopRoutine());
    }

    private IEnumerator StopRoutine()
    {
        yield return new WaitForSeconds(1f);
    }
}
