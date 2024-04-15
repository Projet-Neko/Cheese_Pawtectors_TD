using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBrain : MouseBrain
{
    private float repulsionForce = 2f;
    protected override void Start()
    {
        base.Start();
        Debug.Log("Start du rat");
        StartCoroutine(AttackEveryFiveSeconds());
    }

    protected override void Update()
    {
        base.Update();

        if ((Entity as Mouse).IsBoss)
        {

        }
    }

    private IEnumerator AttackEveryFiveSeconds()
    {
        Debug.Log("Entrer dans la coroutine");
        while (true)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, FollowRange);
            foreach (Collider2D target in targets)
            {
                if (target.gameObject.layer == LayerMask.NameToLayer("Cat"))
                {
                    // attaque qui repousse tous les chats autour de lui.

                    // Calculer la direction du repoussement
                    Vector2 repulsionDirection = target.transform.position - transform.position;

                    // Obtenir le composant Rigidbody2D de la cible
                    Rigidbody2D targetRigidbody = target.GetComponent<Rigidbody2D>();
                    if (targetRigidbody != null)
                    {
                        // Appliquer une force de repoussement sur la cible
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
