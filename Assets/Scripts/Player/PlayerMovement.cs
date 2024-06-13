using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : BaseCharacter
{
    Rigidbody myRigidBody;
    public float moveSpeed = 10.0f;

    public GameObject mousePointer;
    [SerializeField] private Transform parent;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAim();



    }
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = Vector3.zero;
        movement -= transform.forward * Input.GetAxis("Horizontal"); 
        movement += Vector3.right * Input.GetAxis("Vertical"); 

        myRigidBody.velocity= transform.forward + movement * moveSpeed * Time.fixedDeltaTime;
    }

    void UpdateAim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = transform.position.y;
        mousePointer.transform.position = mousePos;
        float deltaY = mousePos.z - transform.position.z;
        float deltaX = mousePos.x - transform.position.x;
        float angleInDegrees = Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;
        transform.eulerAngles = new Vector3(0, -angleInDegrees, 0);
        myRigidBody.MoveRotation(transform.rotation);
    }

  
    public void ResetPosition()
    {
        transform.position = parent.transform.position;

    }
}
