using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    [HideInInspector] public PlayerStatistics playerStats;

    private void OnTriggerEnter(Collider other) {
        playerStats = other.gameObject.GetComponent<PlayerStatistics>();
        this.gameObject.SetActive(false);
    }
}
