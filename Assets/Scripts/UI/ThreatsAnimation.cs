using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treatsAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _destinationGO;
    [SerializeField] private float _speed;
    private Vector3 _destination;

    private Rigidbody rb;

    void Start()
    {
        _destination = _destinationGO.transform.position;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(treatAnimation());
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, _destination);
        if (distance < 1f)
        {
            Destroy(gameObject);
        }

    
    }


    private IEnumerator treatAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector3(0, 0, 0);
        //yield return new WaitForSeconds(0.7f);
        ////rb.velocity = (_destination - transform.position);
        //rb.velocity = (transform.position - _destination);
        ////Destroy(gameObject, 2f);
    }

}
