using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GolfMovementScript : MonoBehaviour
{
    // private PhotonView myView;
    private GameObject myChild;

    private float xInput;
    private float yInput;
    private float movementSpeed = 10.0f;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    bool grounded;
    private InputData inputData;
    private Rigidbody myRB;
    private Transform myXRRig;

    private MeshRenderer myRenderer;
    
    private bool vrFlag = false;


    void Start()
    {
        // myView = GetComponent<PhotonView>();
        myChild = transform.GetChild(0).gameObject;
        myRenderer = myChild.GetComponent<MeshRenderer>(); // hides the body from the player's POV
        myRB = myChild.GetComponent<Rigidbody>();
        

        GameObject myXrOrigin = GameObject.Find("XR Origin");
     
        myXRRig = myXrOrigin.transform;
        inputData = myXrOrigin.GetComponent<InputData>();

        // if (myView.IsMine)
        // {
            myXRRig.position = myChild.transform.position;
            myXrOrigin.transform.SetParent(transform.GetChild(0).transform);
            // myRenderer.enabled = false;
            
        // }
    }

    // Update is called once per frame
    void Update()
    {
        // if (myView.IsMine)
        // {
            
            myXRRig.position = myChild.transform.position;

            if (inputData.rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movement))
            {
                xInput = movement.x;
                yInput = movement.y;
                vrFlag = true;
            }


            ////////////////////////////////////////////////////////////////////////////////////
            else // PC controls
            {
                // Movement controls
                xInput = Input.GetAxis("Horizontal");
                yInput = Input.GetAxis("Vertical");

                // movementSpeed /= 2;

                // Rotation controls
                myChild.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 1);
                myXRRig.Rotate((Vector3.up * Input.GetAxis("Mouse X") * 1));
                

  
            } 
            ////////////////////////////////////////////////////////////////////////////////////
            


            Vector3 moveDir = new Vector3(xInput, 0, yInput).normalized;
            Vector3 targetMoveAmount = moveDir * movementSpeed;
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        // }
    }


    private void FixedUpdate()
    {
        // if (myView.IsMine)
        // {
            Vector3 localMove = myXRRig.GetChild(0).transform.GetChild(0).transform.rotation * transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
            myRB.MovePosition(myRB.position + localMove);



            ////////////////////////////////////////////////////////////////////////////////////
            if (!vrFlag)
            {
                myRB.MoveRotation(myChild.transform.rotation);
            }
            ////////////////////////////////////////////////////////////////////////////////////
        // }
    }



}



