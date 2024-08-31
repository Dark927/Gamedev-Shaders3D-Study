using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    public GameObject hitParticles;

    public float shootForce;


    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity=Vector3.forward * shootForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitImpact= Instantiate(hitParticles, transform.position, Quaternion.identity);
        hitImpact.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
        Destroy(gameObject);
    }



}
