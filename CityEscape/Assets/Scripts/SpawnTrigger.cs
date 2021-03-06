using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public GameObject ui;
    public GameObject target;

    void OnTriggerEnter(Collider other)
    {
        ui.GetComponent<UI>().MessageHit();
        target.GetComponent<PlayerMove>().SetSpawn();
    }
}
