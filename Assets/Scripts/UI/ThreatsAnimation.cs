using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currencie : MonoBehaviour
{
    [SerializeField] private GameObject _threat;

    void Start()
    {
        Entity.OnDeath += SpawnThreats;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnThreats(Entity mouse, bool isItAMouse)
    {
        if (isItAMouse)
        {

            Vector3 spawnPos = mouse.transform.position;
            for (int i = 0; i < mouse.Level; i++)
            {
                spawnPos = new Vector3(spawnPos.x + Random.Range(-0.1f, 0.1f), spawnPos.y + Random.Range(-0.1f, 0.1f), spawnPos.z);
                GameObject threat = _threat;
                Instantiate(threat, spawnPos, Quaternion.identity);
                StartCoroutine(ThreatAnimation(threat));
            }
        }
    }

    private IEnumerator ThreatAnimation(GameObject threat)
    {
        Rigidbody rb = threat.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(0.7f);
        rb.velocity = transform.position - threat.transform.position;

    }

}
