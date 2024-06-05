using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private GameObject objectToSpan;
    public void Interact()
    {
        var spannedObject=Instantiate(objectToSpan, counterTopPoint);
        spannedObject.transform.localPosition = Vector3.zero;
    }
   
}
