using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT
{
    public class ObjectPool : Singleton<ObjectPool>
    {
        [SerializeField, Tooltip("Toggling on and off should force it to hookup all the pooled objects")]
        private bool update;

        [SerializeField, Tooltip("All the objects that are pooled.")]
        private PooledObject[] objects = null;

        /// <summary>
        /// Removes the activate gameobject and puts it back into the pool.  
        /// </summary>
        /// <param name="objectToDestroy">the instance to remove.</param>
        public static void DestroyGameObject(GameObject objectToDestroy)
        {
            instance._DestroyGameObject(objectToDestroy);
        }

        /// <summary>
        /// Removes the activate gameobject and puts it back into the pool.
        /// </summary>
        /// <param name="objectToDestroy">the instance to remove.</param>
        private void _DestroyGameObject(GameObject objectToDestroy)
        {
            PooledObject pool = _FindGameObject(objectToDestroy);
            if (pool == null) return;

            objectToDestroy.SetActive(false);
            objectToDestroy.transform.parent = pool.parent;
        }

        /// <summary>
        /// Removes the activate gameobject and puts it back into the pool after a set delay of scaled time. 
        /// </summary>
        /// <param name="objectToDestroy">the instance to remove.</param>
        public static void DestroyGameObject(GameObject objectToDestroy, float time)
        {
            if (time <= 0)
            {
                instance._DestroyGameObject(objectToDestroy);
            }
            else
            {
                //start coroutine to despawn
                instance.DelayedExecute( time, () => DestroyGameObject(objectToDestroy));
            }
        }

        /// <summary>
        /// Spawns a new instance of the game object using the pool. 
        /// </summary>
        /// <param name="reference">The gameobject you want to spawn.</param>
        /// <returns>The new spawned gameobject</returns>
        public static GameObject SpawnGameObject(GameObject reference)
        {
            return instance._SpawnGameObject(reference);
        }

        public static GameObject SpawnGameObject(GameObject reference, Transform transform)
        {
            return instance._SpawnGameObject(reference, transform);
        }

        /// <summary>
        /// Spawns a new instance of the game object using the pool. 
        /// </summary>
        /// <param name="reference">The gameobject you want to spawn.</param>
        /// <param name="position">The location in world space you want to place it.</param>
        /// <param name="rotation">The rotation the new spawned object should have.</param>
        /// <returns>The new spawned gameobject</returns>
        public static GameObject SpawnGameObject(GameObject reference, Vector3 position, Quaternion rotation)
        {
            return instance._SpawnGameObject(reference, position, rotation);
        }

        /// <summary>
        /// Spawns a new instance of the game object using the pool. 
        /// </summary>
        /// <param name="reference">The gameobject you want to spawn.</param>
        /// <param name="position">The location in world space you want to place it.</param>
        /// <param name="rotation">The rotation the new spawned object should have.</param>
        /// <param name="transform">The parent transform it should be nested under.</param>
        /// <returns>The new spawned gameobject</returns>
        public static GameObject SpawnGameObject(GameObject reference, Vector3 position, Quaternion rotation, Transform transform)
        {
            return instance._SpawnGameObject(reference, position, rotation, transform);
        }

        /// <summary>
        /// Spawns a new instance of the game object using the pool. 
        /// </summary>
        /// <param name="reference">The gameobject you want to spawn.</param>
        /// <param name="position">The location in world space you want to place it.</param>
        /// <param name="rotation">The rotation the new spawned object should have.</param>
        /// <param name="transform">The parent transform it should be nested under.</param>
        /// <returns>The new spawned gameobject</returns>
        private GameObject _SpawnGameObject(GameObject reference, Vector3 position, Quaternion rotation, Transform transform = null)
        {
            GameObject result = _SpawnGameObject(reference, transform);
            if (result == null) return null;

            result.transform.position = position;
            result.transform.rotation = rotation;

            return result;
        }

        /// <summary>
        /// Spawns a new instance of the game object using the pool. 
        /// </summary>
        /// <param name="reference">The gameobject you want to spawn.</param>
        /// <param name="transform">The parent transform it should be nested under.</param>
        /// <returns>The new spawned gameobject</returns>
        private GameObject _SpawnGameObject(GameObject reference, Transform transform = null)
        {
            PooledObject pool = _FindGameObject(reference);
            if (pool == null) return null;

            for (int i = 0; i < pool.spawnedObjects.Count; i++)
            {
                GameObject pooledObject = pool.spawnedObjects[i];
                if (!pooledObject.activeSelf)
                {
                    pooledObject.SetActive(true);
                    pooledObject.transform.SetParent(transform);
                    pooledObject.transform.localScale = pool.prefab.transform.localScale;
                    pooledObject.transform.localPosition = pool.prefab.transform.localPosition;
                    pooledObject.transform.localRotation = pool.prefab.transform.localRotation;
                    return pooledObject;
                }
            }

            Debug.LogError($"Not enough inactive instances of {reference.name} in pool {gameObject.name}. Consider adding more.", gameObject);
            return null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (objects == null || objects.Length == 0)
            {
                return;
            }

            foreach (PooledObject pooled in objects)
            {
                if (pooled.prefab != null)
                {
                    //organization of the object pool
                    if (pooled.parent == null)
                    {
                        pooled.parent = new GameObject(pooled.prefab.name).transform;
                        pooled.parent.parent = transform;
                    }

                    pooled.name = pooled.prefab.name;
                    pooled.parent.name = pooled.name;

                    pooled.spawnedObjects.Clear();
                    for (int i = 0; i < pooled.parent.childCount; i++)
                    {
                        GameObject child = pooled.parent.GetChild(i).gameObject;
                        child.SetActive(false);
                        child.name = pooled.name;
                        pooled.spawnedObjects.Add(child);
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Finds which pool the object belongs to.
        /// </summary>
        /// <param name="reference">The gameobject you want to match into the pool.</param>
        /// <returns>The pool it belongs to or null if it doesn't exist.</returns>
        private PooledObject _FindGameObject(GameObject reference)
        {
            foreach (PooledObject pooled in objects)
            {
                if (pooled.prefab == null) continue;

                if (pooled.prefab.name == reference.name)
                {
                    return pooled;
                }
            }

            Debug.LogError($"Gameobject {reference.name} does not exist in {gameObject.name}", gameObject);
            return null;
        }
    }

    [System.Serializable]
    public class PooledObject
    {
        [Tooltip("name of the pooled object.")]
        public string name = "";
        [Tooltip("the prefab of the pooled object.")]
        public GameObject prefab = null;
        [NaughtyAttributes.ReadOnly(), Tooltip("all pooled items.")]
        public List<GameObject> spawnedObjects = new List<GameObject>();
        [NaughtyAttributes.ReadOnly(), Tooltip("the parent object that all the nested objects are under.")]
        public Transform parent;
    }
}