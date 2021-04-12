using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<PlayerStatistics>().TakeHealth();
        }
    }
}
