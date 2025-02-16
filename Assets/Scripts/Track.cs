using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{

    public GameObject[] obstacles;
    public List<GameObject> newObstacles;
    public List<GameObject> newCoins;
    public Vector2 numberOfObstacles;
    public GameObject coin;
    public Vector2 numberOfCoin;

    // Start is called before the first frame update
    void Start()
    {
        int newNumberOfObstacles =(int) Random.Range(numberOfObstacles.x, numberOfObstacles.y);
        int newNumberOfCoins = (int)Random.Range(numberOfCoin.x, numberOfCoin.y);


        for (int i = 0; i < newNumberOfObstacles; i++)
        {
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform ));
            newObstacles[i].SetActive(false);
        }

        for (int i = 0;i < newNumberOfCoins; i++)
        {
            newCoins.Add(Instantiate(coin, transform));
            newCoins[i].SetActive(false);

        }

        PositionateObstacles();
        PositionateCoins();
    }

    void PositionateObstacles()
    {
        float trackLength = 32f; // Chiều dài track
        for (int i = 0; i < newObstacles.Count; i++)
        {
            float posZ = (i + 1) * (trackLength / newObstacles.Count);
            newObstacles[i].transform.localPosition = new Vector3(0, 0, posZ);
            newObstacles[i].SetActive(true);

            ChangeLane laneScript = newObstacles[i].GetComponent<ChangeLane>();
            if (laneScript != null)
            {
                laneScript.PositionLane();
            }
        }
    }



    void PositionateCoins()
    {
        float minZPos = 10f;
        float coinSpacing = 1f; // Khoảng cách giữa các coin
        for (int i = 0; i < newCoins.Count; i++)
        {
            float randomZPos = minZPos + (i * coinSpacing);
            float laneX = Random.Range(-1, 2) * 2f; // Spawn theo lane

            newCoins[i].transform.localPosition = new Vector3(laneX, transform.position.y, randomZPos);
            newCoins[i].SetActive(true);

            ChangeLane laneScript = newCoins[i].GetComponent<ChangeLane>();
            if (laneScript != null)
            {
                laneScript.PositionLane();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            {
                transform.position = new Vector3(0, 0, transform.position.z + (32 * 2));
            PositionateObstacles();
            PositionateCoins();
            }
    }
}
