using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fey_HitBoxManager : MonoBehaviour
{
    public PolygonCollider2D frame0;
    public PolygonCollider2D frame1;
    public PolygonCollider2D frame2;
    public PolygonCollider2D frame3;

    private PolygonCollider2D[] colliders;
    
    //get Fey's collider
    private PolygonCollider2D localCollider;

    public enum hitBoxes
    {
        frame0Box,
        frame1Box,
        frame2Box,
        frame3Box,
        clear //used to clear all boxes
    }

    void Start()
    {
        //set up the array
        colliders = new PolygonCollider2D[]{frame0,frame1,frame2,frame3};
        
        //create a polygon collider
        localCollider = gameObject.AddComponent<PolygonCollider2D>(); //add a polygoncollider2d when we first activate the script
        localCollider.isTrigger = true; //set as a trigger, so it does not collide with environment.
        localCollider.pathCount = 0; //clear our polygon collider.
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //TODO add combat functionality here
    }

    public void setHitBox(hitBoxes val)
    {
        if (val != hitBoxes.clear)
        {
            localCollider.SetPath(0, colliders[(int)val].GetPath(0));

            return;
        } 
        localCollider.pathCount = 0;


    }

}
