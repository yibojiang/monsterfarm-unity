using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class RendererSortingLayer : MonoBehaviour {
     public int SortLayer = 0;
     public int SortingLayerID = SortingLayer.GetLayerValueFromName("Default");
     // Use this for initialization
     void Start () {
         Renderer renderer = this.gameObject.GetComponent<Renderer>();
         if(renderer != null)
         {
             renderer.sortingOrder = SortLayer;
             renderer.sortingLayerID = SortingLayerID;
         }
     }

     void Update() {
     	if(GetComponent<Renderer>() != null)
         {
             GetComponent<Renderer>().sortingOrder = SortLayer;
             GetComponent<Renderer>().sortingLayerID = SortingLayerID;
         }
     }
     
 }