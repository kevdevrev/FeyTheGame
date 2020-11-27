using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    Animator myAnimator;
    Collider2D myCollider;
    [SerializeField] public float doorSpeed = 2;
    //[SerializeField] private Vector3 doorDesiredLocation;
    [SerializeField] public Transform targetLocation;
    
    void Start()
    {
        //myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    public void Open()
    {
        if (!isOpen)
            SetState(true);
    }

    public void Close()
    {
        if (isOpen)
            SetState(false);
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    void SetState(bool open)
    {
        isOpen = open;
        //myAnimator.SetBool("Open", open);
        
        myCollider.isTrigger = open;
        Debug.Log("longDoor open");
        Vector2 targetL = new Vector2(targetLocation.position.x, targetLocation.position.y);
                    transform.position =
                        Vector2.MoveTowards(transform.position, targetL, doorSpeed * Time.deltaTime);
        //transform.position = new Vector3.MoveTowards(transform.position, targetLocation, doorSpeed);
    }
}
