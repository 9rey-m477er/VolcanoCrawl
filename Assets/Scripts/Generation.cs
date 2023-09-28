using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public GameObject[] availableRooms;
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
 
    // Start is called before the first frame update
    void Start()
    {
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
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        GameObject room = Instantiate(availableRooms[randomRoomIndex]);
        float roomCenter = farthestRoomEndY + TerrainHeight * .5f+13f;
        room.transform.position = new Vector3(0, roomCenter, 0);
        currentRooms.Add(room);
        
    }

    void GenerateObjectsIfRequired()
    {
        float playerY = transform.position.y;
        float removeObjectsY = playerY - screenHeightInPoints;
        float addObjectY = playerY + screenHeightInPoints;
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

    private void GenerateRoomIfRequired()
    {
        List<GameObject> roomsToRemove = new List<GameObject>();
        bool addRooms = true;
        float playerY = transform.position.y;
        float removeRoomY = playerY - screenHeightInPoints;
        float addRoomY = playerY + screenHeightInPoints;
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

    }
}
