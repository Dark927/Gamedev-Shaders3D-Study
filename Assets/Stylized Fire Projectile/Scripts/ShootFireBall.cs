using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFireBall : MonoBehaviour
{
    public GameObject greenfireball;
    public GameObject Orangefireball;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
           Instantiate(greenfireball, transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Instantiate(Orangefireball, transform.position, Quaternion.identity);
        }
    }
}
