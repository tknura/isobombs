using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public static PowerupSpawner instance;

    [SerializeField] private GameObject powerupsParent;
    [SerializeField] private GameObject[] powerups;

    [Tooltip("Ranging beetwen 0 and 100")][Range(0,100)]
    [SerializeField] private float chanceOfSpawning;

    private void Awake() {
        if(!instance) {
            instance = this;
        }
    }

    public void SpawnPowerup(Vector3 position) {
        int rand = Random.Range(0, 100);
        if(chanceOfSpawning >= rand) {
            int powerupIndex = Random.Range(0, powerups.Length - 1);
            //Instantiate(powerups[powerupIndex], position, powerups[powerupIndex].transform.rotation);
            ObjectPooler.instance.SpawnFromPool("Powerup", position, Quaternion.identity);
        }
    }
 }
