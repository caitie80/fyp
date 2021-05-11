using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderSpawner : MonoBehaviour
{
    public GameObject spiderPrefab;
    public Slider amount;
    public Slider scale;

    int spiderLayerID = 9;
    int numOfSpiders;
    List<GameObject> spidersMade = new List<GameObject>();

    Vector3 spiderPosition;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateSpiders()
    {
        numOfSpiders = (int)amount.value;

        if (spidersMade.Count > numOfSpiders)
        {
            int difference = spidersMade.Count - numOfSpiders;
            destroySpiders(difference);
        }
            
        
        GameObject newSpider;
        float newScale = scale.value;
        Vector3 currentRotation;


        int maxSpawnAttemptsPerObstacle = 10;
        for (int i = spidersMade.Count; i < numOfSpiders; i++)
        {
            bool validPosition = false;
            int spawnAttempts = 0;

            while (!validPosition && (spawnAttempts < maxSpawnAttemptsPerObstacle))
            {
                spawnAttempts++;

                spiderPosition = new Vector3(Random.Range(0.55f, 1.08f), 0.5f, Random.Range(0.25f, -0.92f));

                validPosition = true;

                Collider[] colliders = Physics.OverlapSphere(spiderPosition, 0.2f);

                foreach(Collider c in colliders)
                {
                    Debug.Log("Tag: " + c.tag);

                    if (c.tag == "Spider")
                    {
                        validPosition = false;
                    }
                }                
            }
            
            if (validPosition)
            {
                newSpider = Instantiate(spiderPrefab, spiderPosition, Quaternion.identity);
                newSpider.transform.localScale = new Vector3(newScale, newScale, newScale);

                currentRotation = newSpider.transform.rotation.eulerAngles;
                newSpider.transform.rotation = Quaternion.Euler(currentRotation.x, Random.Range(0f, 360f), currentRotation.z);

                spidersMade.Add(newSpider);
            }



        }
    }

    void destroySpiders(int amount)
     {
        //foreach (var spider in spidersMade)
        //{
        //    Destroy(spider);
        //}

        //spidersMade.Clear();

        for(int i = 0; i < amount; i++)
        {
            Destroy(spidersMade[0]);
            spidersMade.RemoveAt(0);
        }
    }
}
