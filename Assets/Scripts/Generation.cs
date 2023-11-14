using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

public class Generation : MonoBehaviour
{
    public GameObject[] easyRooms;
    public GameObject[] mediumRooms;
    public GameObject[] hardRooms;
    public List<GameObject> currentRooms;
    private float screenHeightInPoints;
    public GameObject[] availableObjects;
    public List<GameObject> objects;
    public float TerrainHeight;
    public float objectsMinDistance = 5.0f;
    public float objectsMaxDistance = 10.0f;

    public float objectsMinX = -1.4f;
    public float objectsMaxX = 1.4f;

    public float objectsMinRotation = -45.0f;
    public float objectsMaxRotation = 45.0f;
    public float lastRoomEndY;
    public float lastObjectY;

    public Text scoreText;
    public Text scoreText2;
    private Text higherText;
    float playerScore = 0;
    float playerScore2 = 0;
    float higherscore = 0;
    private Scene scene;

    public GameObject player1;
    public GameObject player2;
    private GameObject higherPlayer;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        
        float width = 2.0f * Camera.main.orthographicSize * Camera.main.aspect;
        screenHeightInPoints = width;
        StartCoroutine(GeneratorCheck());
        TerrainHeight = GameObject.Find("Terrain").transform.localScale.y;
    }
    void AddObject(float lastObjectY)
    {
        int randomIndex = Random.Range(0, availableObjects.Length);
        GameObject obj = Instantiate(availableObjects[randomIndex]);
        float objectPositionY = lastObjectY + Random.Range(objectsMinDistance, objectsMaxDistance);
        float randomX = Random.Range(objectsMinX, objectsMaxX);
        obj.transform.position = new Vector3(randomX, objectPositionY, 0);
        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
        objects.Add(obj);
    }

    void AddRoom(float farthestRoomEndY)
    {
        GameObject room;
        if (playerScore > playerScore2)
        {
            higherscore = playerScore;
            higherText = scoreText;
            higherscore = int.Parse(scoreText.text.Substring(6));
        }
        else if (playerScore == playerScore2)
        {
            higherscore = playerScore;
            higherText = scoreText;
        }
        else
        {
            higherscore = playerScore2;
            higherText = scoreText2;
            higherscore = int.Parse(scoreText2.text.Substring(6));
        }
        

        Debug.Log(higherscore);
        if(higherscore < 250)
        {
            int randomRoomIndex = Random.Range(0, easyRooms.Length);
            room = Instantiate(easyRooms[randomRoomIndex]);
        }
        else if(higherscore >= 250 && higherscore < 500) 
        {
            if(Random.Range(0,2) == 0)
            {
                int randomRoomIndex = Random.Range(0, easyRooms.Length);
                room = Instantiate(easyRooms[randomRoomIndex]);
            }
            else
            {
                int randomRoomIndex = Random.Range(0, mediumRooms.Length);
                room = Instantiate(mediumRooms[randomRoomIndex]);
            }
        }
        else if(higherscore >= 500 && higherscore < 750)
        {
            int randomRoomIndex = Random.Range(0, mediumRooms.Length);
            room = Instantiate(mediumRooms[randomRoomIndex]);
        }
        else if(higherscore >= 750 && higherscore < 1000)
        {
            if (Random.Range(0, 2) == 0)
            {
                int randomRoomIndex = Random.Range(0, mediumRooms.Length);
                room = Instantiate(mediumRooms[randomRoomIndex]);
            }
            else
            {
                int randomRoomIndex = Random.Range(0, hardRooms.Length);
                room = Instantiate(hardRooms[randomRoomIndex]);
            }
        }
        else
        {
            int randomRoomIndex = Random.Range(0, hardRooms.Length);
            room = Instantiate(hardRooms[randomRoomIndex]);
        }
        //int randomRoomIndex = Random.Range(0, easyRooms.Length);
        //GameObject room = Instantiate(easyRooms[randomRoomIndex]);
        float roomCenter = farthestRoomEndY + TerrainHeight * .5f+16f;
        room.transform.position = new Vector3(0, roomCenter, 0);
        currentRooms.Add(room);
        
    }
    
    void GenerateObjectsIfRequired()
    {
        higherPlayer = player1;
        float higherY = 0;
        if(scene.name == "Two Player Mode")
        {
            if(player1.transform.position.y >= player2.transform.position.y)
            {
                higherPlayer = player1;
            }
            else
            {
                higherPlayer = player2;
            }
        }
        else
        {

        
        higherY = higherPlayer.transform.position.y;
        float removeObjectsY = higherY - screenHeightInPoints;
        float addObjectY = higherY + screenHeightInPoints;
        float farthestObjectY = 0;

        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (var obj in objects)
        {
            float objY = obj.transform.position.y;
            farthestObjectY = Mathf.Max(farthestObjectY, objY);
            if (objY < removeObjectsY)
            {
                objectsToRemove.Add(obj);
            }
        }

        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        if (farthestObjectY < addObjectY)
        {
            AddObject(farthestObjectY);
        }
        }
    }

    private void GenerateRoomIfRequired()
    {
        higherPlayer = player1;
        if (scene.name == "Two Player Mode")
        {
            if (player1.transform.position.y >= player2.transform.position.y)
            {
                higherPlayer = player1;
            }
            else
            {
                higherPlayer = player2;
            }
        }

        List<GameObject> roomsToRemove = new List<GameObject>();
        bool addRooms = true;
        float higherY = higherPlayer.transform.position.y;
        float removeRoomY = higherY - screenHeightInPoints;
        float addRoomY = higherY + screenHeightInPoints;
        float farthestRoomEndY = 0;

        foreach (var room in currentRooms)
        {
            float roomStartY = room.transform.position.y - (TerrainHeight * 0.5f);
            float roomEndY = roomStartY + TerrainHeight;

            if (roomStartY > addRoomY)
            {
                addRooms = false;
            }

            if (roomEndY < removeRoomY)
            {
                roomsToRemove.Add(room);
            }

            farthestRoomEndY = Mathf.Max(farthestRoomEndY, roomEndY);
        }

        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        if (addRooms)
        {
            AddRoom(farthestRoomEndY);
        }
    }

    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GenerateRoomIfRequired();
            //GenerateObjectsIfRequired();
            yield return new WaitForSeconds(3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerScore = int.Parse(scoreText.text.Substring(7));
        playerScore2 = int.Parse(scoreText2.text.Substring(7));



    }
}
