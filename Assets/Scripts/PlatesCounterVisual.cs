using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlateCounter plateCounter;
    [SerializeField] private GameObject plateVisual;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private float platesDistance = 0.08f;

    private List<GameObject> platesVisual= new List<GameObject>();
   
    void Start()
    {
        plateCounter.OnPlatesSpawned += PlateCounterVisual_SpawnPlateVisual;
        plateCounter.OnPlatesRemoved += PlateCounterVisual_RemovePlateVisual;

    }

    private void PlateCounterVisual_RemovePlateVisual(object sender, EventArgs e)
    {
        GameObject lastPlate= platesVisual[platesVisual.Count-1];
        platesVisual.Remove(lastPlate);
        Destroy(lastPlate);
    }

    private void PlateCounterVisual_SpawnPlateVisual(object sender, EventArgs e)
    {
        GameObject plate= Instantiate(plateVisual, counterTopPoint);
        platesVisual.Add(plate);
        
        plate.transform.position = new Vector3(0,
            counterTopPoint.localPosition.y + platesDistance*(platesVisual.Count-1), 0);

    }
}
