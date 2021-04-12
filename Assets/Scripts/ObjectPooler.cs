using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler instance;

    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
        public bool spawnOnStart;
        public GameObject parent;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake() {
        if(!instance) {
            instance = this;
        }   
    }

    void Start() {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools) {
            if(pool.spawnOnStart) {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for(int i = 0; i < pool.size; i++) {
                    GameObject obj = Instantiate(pool.prefab) as GameObject;
                    obj.transform.parent = pool.parent.transform;
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.tag, objectPool);
            }     
        }
    }

    public virtual GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {
        if(!poolDictionary.ContainsKey(tag)) {
            Debug.Log("Pool with tag " + tag + " doesn't  exist");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.position = position;

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    public void DelayedDeactivateObject(GameObject obj, float time) {
        StartCoroutine(DelayedSetActive(obj, false, time));
    }

    public void DelayedActivateObject(GameObject obj, float time) {
        StartCoroutine(DelayedSetActive(obj, false, time));
    }

    private IEnumerator DelayedSetActive(GameObject obj, bool state, float time) {
        yield return new WaitForSecondsRealtime(time);
        obj.SetActive(state);
        yield return null;
    }
}
