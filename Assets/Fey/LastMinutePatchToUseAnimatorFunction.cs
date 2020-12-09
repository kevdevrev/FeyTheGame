using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastMinutePatchToUseAnimatorFunction : MonoBehaviour
{
    public Fey parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.GetComponentInParent<Fey>();

    }

    public void restartElStage()
    {
        parent.restartStage();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
