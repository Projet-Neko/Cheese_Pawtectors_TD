using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBrain : MouseBrain
{
    private float repulsionForce = 2f;
    protected override void Start()
    {
        base.Start();
        StartCoroutine(AttackEveryFiveSeconds());
    }

    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator AttackEveryFiveSeconds()
    {
        // Attack that repels all the cats around him.
        while (true)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, FollowRange);
            foreach (Collider2D target in targets)
            {
                Debug.Log("foreach cat");
                if (target.gameObject.layer == LayerMask.NameToLayer("Cat"))
                {
                    // Calculate the direction of repulsion
                    Vector2 repulsionDirection = target.transform.position - transform.position;

                    // Get the target Rigidbody2D component
                    Rigidbody2D targetRigidbody = target.GetComponent<Rigidbody2D>();
                    Debug.Log("if layer");
                    if (targetRigidbody != null)
                    {
                        Debug.Log("if rb");
                        // Apply a repulsive force to the target
                        targetRigidbody.AddForce(repulsionDirection.normalized * repulsionForce, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(1f);
                        targetRigidbody.AddForce(-repulsionDirection.normalized * repulsionForce, ForceMode2D.Impulse);
                        Debug.Log(-repulsionDirection.normalized * repulsionForce);
                    }
                }
            }
            Debug.Log("Avant la fin de la coroutine");
            yield return new WaitForSeconds(5f);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
