using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject target;
    public string onMessage;
    public string offMessage;
    public bool isOn;
    Animator myAnimator;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void TurnOn()
    {
        //if (!isOn)
            //SetState(true);
        target.GetComponent<Door>().Open();
    }

    public void TurnOff()
    {
        if (!isOn)
            SetState(false);
    }

    public void Toggle()
    {
        if (isOn)
            TurnOff();
        else
            TurnOn();
    }

    void SetState(bool on)
    {
        isOn = on;
        myAnimator.SetBool("On", on);
        if (on)
        {
            if (target != null && !string.IsNullOrEmpty(onMessage))
                target.SendMessage(onMessage);
        }
        else
        {
            if (target != null && !string.IsNullOrEmpty(offMessage))
                target.SendMessage(offMessage);
        }
    }
    /*[SerializeField] GameObject buttonOn;
    [SerializeField] GameObject buttonOff;

    public bool isOn = false;

    void Start()
    {
        // Set the button off sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = buttonOff.GetComponent<SpriteRenderer>().sprite;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Set the button on sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = buttonOn.GetComponent<SpriteRenderer>().sprite;

        isOn = true;
    }*/
}