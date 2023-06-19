using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class AbstractedObjectPool<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable {
    private T prefab;
    private ObjectPool<T> pool;
    private ObjectPool<T> Pool {
        get {
            if (pool == null) {
                throw new InvalidOperationException("You need to call InitPool before using the object pool.");
            }

            return pool;
        }
        set => pool = value;
    }

    protected void InitPool(T prefab, int initialSize = 10, int maxSize = 20, bool collectionChecks = false) {
        this.prefab = prefab;
        Pool = new ObjectPool<T>(OnCreate, OnSpawn, OnDespawn, OnDestroy, collectionChecks, initialSize, maxSize);
    }

    #region Overrides
    protected virtual T OnCreate() {
        prefab.OnCreate();
        return Instantiate(prefab);
    }

    protected virtual void OnSpawn(T obj) {
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
    }

    protected virtual void OnDespawn(T obj) {
        obj.gameObject.SetActive(false);
        obj.OnDespawn();
    }

    protected virtual void OnDestroy(T obj) {
        obj.OnDestroy();
        Destroy(obj);
    }
    #endregion

    #region Getters
    public T Spawn() => Pool.Get();
    public void Despawn(T obj) => Pool.Release(obj);
    #endregion
}