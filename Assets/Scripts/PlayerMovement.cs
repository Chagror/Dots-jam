using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    public Camera mainCamera;

    [Header("Movement")]
    public float speed = 4.5f;
    public LayerMask whatIsGround;

    [Header("Life Settings")]
    public float playerHealth = 1f;



    Rigidbody playerRigidbody;
    bool isDead =false;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        //Arrow Key Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(h, 0, v);

        //Camera Direction
        var cameraForward = mainCamera.transform.forward;
        var cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        //Try not to use var for roadshows or learning code
        Vector3 desiredDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

        //Why not just pass the vector instead of breaking it up only to remake it on the other side?
        
        
        MoveThePlayer(desiredDirection);
        TurnThePlayer();
        

    }

    void MoveThePlayer(Vector3 desiredDirection)
    {
        Vector3 movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void TurnThePlayer()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100, whatIsGround))
        {
            
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            Debug.DrawRay(transform.position, playerToMouse, Color.red);

            playerRigidbody.MoveRotation(newRotation);
        }
    }




}
