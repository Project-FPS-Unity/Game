using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject theEnemy;
    [SerializeField] private int maxEnemies;

    private int xPos;
    private int zPos;
    private int enemyCount;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    // Update is called once per frame
    private IEnumerator EnemyDrop()
    {
        while(enemyCount < maxEnemies)
        {
            xPos = Random.Range(1, 30);
            zPos = Random.Range(1,30);
            Instantiate(theEnemy, new Vector3(xPos, gameObject.transform.position.y, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount++;
        }
    }
}
