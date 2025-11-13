using System.Collections;
using UnityEngine;

public class ShootingPlatform : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private float forcePower = 10f;
    [SerializeField] private float launchAngle = 45f;

    private Coroutine coroutine;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(ShootTime(player));
            }
            else
            {
                return;
            }
        }
    }
    private IEnumerator ShootTime(Player player)
    {
        Rigidbody rb = player.controller.rigidbody;

        rb.velocity = Vector3.zero;
        float timer = 0f;

        while (timer < duration)
        {
            Debug.Log("ÄÚ·çÆ¾");
            rb.velocity = Vector3.zero;
            rb.AddForce((transform.forward + Vector3.up) * forcePower * Time.fixedDeltaTime, ForceMode.Impulse);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        coroutine = null;
    }
}
