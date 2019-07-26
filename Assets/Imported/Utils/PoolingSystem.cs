using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSystem : MonoBehaviour {
	public GameObject spawnPrefab;
	public int size = 10;

	public static PoolingSystem instance;

	private List<GameObject> freePool, usedPool;

	void Awake()
	{
		if (instance == null) 
			instance = this;		
		else 
			Destroy(gameObject);

		//DontDestroyOnLoad(gameObject);

		InitPool();
	}

	private void InitPool() {
		freePool = new List<GameObject>();
		FillFreePool();
		usedPool = new List<GameObject>();
	}

	public GameObject GetFreeObject() {
		GameObject free;

		if (freePool.Count == 0) FillFreePool();
	
		free = freePool[0];

		SwapFreePoolObject(free);

		if (free != null) free.SetActive(true);	
		
		return free;
	}

	public void ResetPooling() {
		foreach (GameObject g in usedPool) 
			ReturnUsedObjectForReset(g);
		usedPool.Clear();
	}

	public void ResetPooling(List<GameObject> toRemove, GameObject spawn)
	{
		foreach (GameObject g in toRemove) 
			ReturnUsedObject(g, spawn);
	}

	public void ReturnUsedObjectForReset(GameObject used) {
		used.SetActive(false);
		freePool.Add(used);
	}

    public void RemovePooling()
    {
        foreach(GameObject g in freePool)
        {
            Destroy(g);
        }
        foreach (GameObject g in usedPool)
        {
            Destroy(g);
        }
        InitPool();
    }

	public void ReturnUsedObject(GameObject used, GameObject spawn) {
		ReturnUsedObject(used);
		used.transform.position = spawn.transform.position;
	}

	public void ReturnUsedObject(GameObject used) {
		used.SetActive(false);
		SwapUsedPoolObject(used);
	}

	private void SwapFreePoolObject(GameObject swap) {
		freePool.Remove(swap);
		usedPool.Add(swap);
	}

	private void SwapUsedPoolObject(GameObject swap) {
		freePool.Add(swap);
		usedPool.Remove(swap);
	}

	private void FillFreePool() {
		GameObject tmp;
		for (int i=0; i<size; i++) {
			tmp = Instantiate(spawnPrefab);
			tmp.SetActive(false);
			freePool.Add(tmp);
		}
	}
}
