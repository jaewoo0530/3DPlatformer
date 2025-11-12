using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] LineRenderer lineRenderer;
    private Vector3 start;
    private Vector3 end;
    private Vector3 dir;
    private float dist;
    private bool isCoolTime;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, point1.position);
        lineRenderer.SetPosition(1, point2.position);
    }

    private void Update()
    {
        LaserCheck();
    }

    private void LaserCheck()
    {
        start = lineRenderer.GetPosition(0);
        end = lineRenderer.GetPosition(1);
        dir = end - start;
        dist = dir.magnitude;

        Ray ray = new Ray(start, dir.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, dist))
        {
            if (hit.collider.TryGetComponent<Player>(out Player player))
            {
                if (!isCoolTime)
                {
                    StartCoroutine(DamageCoolTime(player));
                }
            }
        }
    }

    private IEnumerator DamageCoolTime(Player player)
    {
        isCoolTime = true;
        player.status.ReduceHp(damage);
        yield return new WaitForSeconds(0.5f);
        isCoolTime = false;
    }
}
