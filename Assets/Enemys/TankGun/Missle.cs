using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public Transform feyLocation;
    private Rigidbody2D rigid;
    [SerializeField] public float speed;
    [SerializeField] public float rotateSpeed;
    void Start()
    {
        feyLocation = GameObject.FindWithTag("Fey").GetComponent<Transform>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 direction = (Vector2)feyLocation.position - rigid.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        rigid.angularVelocity = -rotateAmount * rotateSpeed;
        rigid.velocity = transform.right * speed;
    }
}
