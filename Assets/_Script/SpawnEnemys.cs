using UnityEngine;

public class SpawnEnemys : MonoBehaviour
{

    public GameObject[] enemysForSpawn;

    void Start()
    {
        for(int i = 0; i < enemysForSpawn.Length; i++)
        {
            enemysForSpawn[i].gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        print("TriggerSpawnEnemy");
        if(col.CompareTag("Panda"))
        {
            SpawnAllEnemys();
        }
    }
    
    void SpawnAllEnemys()
    {
        for(int i = 0; i < enemysForSpawn.Length; i++)
        {
            enemysForSpawn[i].gameObject.SetActive(true);
        }
    }
}
