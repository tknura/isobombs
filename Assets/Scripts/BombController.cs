using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    [SerializeField] private float explosionTime;
    [SerializeField] private float explosionDeleteTime = 1.75f;
    [SerializeField] private string explosionTag = "Explosion_1";

    public float explosionRange;

    private PlayerStatistics playerStats;

    private void Awake() {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatistics>();
    }
   
    private void Start() {
        Invoke("BombExplosion", explosionTime);
    }

    private void BombExplosion() {
        Explode(transform.position);
        Destroy(this.gameObject, 0.5f);
        ExplodeIn4Directions();     
        playerStats.AddBomb();
    }

    private void Explode(Vector3 position) {
        var tempExplosion = ObjectPooler.instance.SpawnFromPool(explosionTag, position, Quaternion.identity);
        ObjectPooler.instance.DelayedDeactivateObject(tempExplosion, explosionDeleteTime);
    }
    
    private void ExplodeIn4Directions() {
        StartCoroutine(ExplosionInLine(Vector3.forward, explosionRange, LayerMask.GetMask("Env")));
        StartCoroutine(ExplosionInLine(Vector3.back, explosionRange, LayerMask.GetMask("Env")));
        StartCoroutine(ExplosionInLine(Vector3.left, explosionRange, LayerMask.GetMask("Env")));
        StartCoroutine(ExplosionInLine(Vector3.right, explosionRange, LayerMask.GetMask("Env")));
    }

    private IEnumerator ExplosionInLine(Vector3 direction, float range, LayerMask layerMask) {
        for(int i = 1; i <= range; i++) {
            RaycastHit hit;
            Physics.Raycast(transform.position, direction, out hit, i, layerMask);
            if(!hit.collider) {
                Explode(transform.position + (i * direction));
                if(Physics.Raycast(transform.position, direction, out hit, i, LayerMask.GetMask("Destroyable"))) {
                    PowerupSpawner.instance.SpawnPowerup(hit.collider.gameObject.transform.position);
                    Destroy(hit.collider.gameObject);
                }
            } else {
                break;
            }

        }
        yield return new WaitForSeconds(.05f);
    }
    
}
