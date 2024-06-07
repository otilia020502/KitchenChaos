using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManualGenerator : MonoBehaviour
{
    [SerializeField] private GameObject blackCube, whiteCube, greenCube;
    [SerializeField] private GameObject roomOne, roomTwo, roomThree;
    private float distanceBetweenTiles = 1f;
    private float speed = 1f; // Speed at which rows move in rooms 2 and 3

    private List<Vector3> initialPositionsRoom2 = new List<Vector3>();
    private List<Vector3> initialPositionsRoom3 = new List<Vector3>();

    private void Start()
    {
        
        roomOne.transform.position = new Vector3(0, 0, 0);
        roomTwo.transform.position = new Vector3(15, 0, 0); 
        roomThree.transform.position = new Vector3(-15, 0, 0); 
        
        GenerateCubesRoom1();
        GenerateCubesRoom2();
        GenerateCubesRoom3();

        // Store initial positions for moving rows
        StoreInitialPositions(roomTwo.transform, initialPositionsRoom2);
        StoreInitialPositions(roomThree.transform, initialPositionsRoom3);
    }

    private void Update()
    {
        MoveRowsInRoom2();
        MoveRowsInRoom3();
    }

    private void GenerateCubesRoom1()
    {
        // Room 1: 10 by 20 black and white checker pattern
        for (int z = 0; z < 20; z++)
        {
            for (int x = 0; x < 10; x++)
            {
                Vector3 newPosition = roomOne.transform.position + new Vector3(x * distanceBetweenTiles, 0, z * distanceBetweenTiles);
                GameObject cubeToInstantiate = (x + z) % 2 == 0 ? blackCube : whiteCube;
                Instantiate(cubeToInstantiate, newPosition, Quaternion.identity, roomOne.transform);
            }
        }
    }

    private void GenerateCubesRoom2()
    {
        // Room 2: 10 by 20 red and white alternating rows, rows move in update
        for (int z = 0; z < 20; z++)
        {
            for (int x = 0; x < 10; x++)
            {
                Vector3 newPosition = roomTwo.transform.position + new Vector3(x * distanceBetweenTiles, 0, z * distanceBetweenTiles);
                GameObject cubeToInstantiate = z % 2 == 0 ? blackCube : whiteCube;
                Instantiate(cubeToInstantiate, newPosition, Quaternion.identity, roomTwo.transform);
            }
        }
    }

    private void GenerateCubesRoom3()
    {
        // Room 3: 10 by 20 red and black solid color floor, cross pattern on player row and column
        for (int z = 0; z < 20; z++)
        {
            for (int x = 0; x < 10; x++)
            {
                Vector3 newPosition = roomThree.transform.position + new Vector3(x * distanceBetweenTiles, 0, z * distanceBetweenTiles);
                GameObject cubeToInstantiate = blackCube; // Default to black
                Instantiate(cubeToInstantiate, newPosition, Quaternion.identity, roomThree.transform);
            }
        }
    }

    private void StoreInitialPositions(Transform roomTransform, List<Vector3> initialPositions)
    {
        initialPositions.Clear();
        for (int i = 0; i < roomTransform.childCount; i++)
        {
            initialPositions.Add(roomTransform.GetChild(i).localPosition);
        }
    }

    private void MoveRowsInRoom2()
    {
        // Move each row in room 2 back and forth based on initial positions
        for (int z = 0; z < initialPositionsRoom2.Count / 10; z++)
        {
            float offset = Mathf.Sin(Time.time * speed) * distanceBetweenTiles;
            for (int x = 0; x < 10; x++)
            {
                Transform cube = roomTwo.transform.GetChild(z * 10 + x);
                Vector3 initialPosition = initialPositionsRoom2[z * 10 + x];
                cube.localPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + offset);
            }
        }
    }

    private void MoveRowsInRoom3()
    {
        // Move each row in room 3 back and forth based on initial positions
        for (int z = 0; z < initialPositionsRoom3.Count / 10; z++)
        {
            float offset = Mathf.Sin(Time.time * speed) * distanceBetweenTiles;
            for (int x = 0; x < 10; x++)
            {
                Transform cube = roomThree.transform.GetChild(z * 10 + x);
                Vector3 initialPosition = initialPositionsRoom3[z * 10 + x];
                cube.localPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + offset);
            }
        }
    }
}
