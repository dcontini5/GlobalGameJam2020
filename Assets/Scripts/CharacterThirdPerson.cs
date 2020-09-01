using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterThirdPerson : MonoBehaviour
{
    #region Variables

    [SerializeField]
    float m_CharacterRotDelay = 0.0f;

    private CharacterController m_CharacterController = null;

    [SerializeField]
    private float m_MovementSpeed = 10.0f;

    private Vector3 m_MovementVec = Vector3.zero;

    //Keep the character to the ground
    [SerializeField]
    private float m_DownWardForce = -0.8f;

    private float m_DownWardVelocity = 0.0f;

    [SerializeField]
    private float m_Gravity = -8.0f;

    [SerializeField]
    private Camera m_Camera = null;

    private Animator m_Animator = null;

    [SerializeField]
    CinemachineVirtualCameraBase m_FreeAimVirtualPlayerCamera = null;

    [SerializeField]
    CinemachineVirtualCameraBase m_ShootVirtualPlayerCamera = null;

    [SerializeField]
    int m_HighestVirtualCameraPriority = 2;
    [SerializeField]
    int m_LowestVirtualCameraPriority = 1;

    PlayerGemMining m_GemMining = null;

    #endregion //Variables

    #region Properties

    

    #endregion //Properties

    #region Methods

    void CharacterMovement(float inputHorzAxis, float inputVertAxis)
    {
        //Character will move along the forward and right axis of the camera
        Vector3 movementForwardVec = m_Camera.transform.forward * inputVertAxis;
        Vector3 movementRightVec = m_Camera.transform.right * inputHorzAxis;
        //Vector3 movementRightVec = transform.right * inputHorzAxis;
        movementForwardVec.y = 0.0f;
        movementRightVec.y = 0.0f;

        m_MovementVec = movementForwardVec + movementRightVec;
        m_MovementVec = m_MovementVec.normalized * m_MovementSpeed * Time.deltaTime;

    }
    void CharacterRotationFixedCameraAim()
    {
        if(m_Camera != null)
        {
            //Find the position of the mouse in World Space
            Vector3 mousePositionScreenSpace = Vector3.zero;//Input.mousePosition;

            mousePositionScreenSpace.x = Input.GetAxis("Mouse X");
            mousePositionScreenSpace.y = Input.GetAxis("Mouse Y");
            mousePositionScreenSpace.z = 0.0f;

            Vector3 mousePositionWorldSpace = m_Camera.ScreenToWorldPoint(mousePositionScreenSpace);

            //Calculate the direction vector from the the mouse position in world space to the player position
            Vector3 fromMouseWorldPosToPlayerPosVec = transform.position - mousePositionWorldSpace;
            fromMouseWorldPosToPlayerPosVec.y = 0.0f;
            fromMouseWorldPosToPlayerPosVec.Normalize();

            //Create a rotation towards the direction vector from the mouse position to the player position
            Quaternion rotation = Quaternion.LookRotation(fromMouseWorldPosToPlayerPosVec);
            rotation = Quaternion.Slerp(transform.rotation, rotation, m_CharacterRotDelay);
            transform.rotation = rotation; //Rotate the character towards this new direction
        }
       
    }

    void CharacterRotationFreeCameraAim(float inputHorzAxis, float inputVertAxis)
    {
        Vector3 inputVec = new Vector3(inputHorzAxis, 0.0f, inputVertAxis);
        Vector3 inputVecWorldSpace = m_Camera.ScreenToWorldPoint(inputVec);

        //Calculate the direction vector from the the mouse position in world space to the player position
        Vector3 fromInputWorldPosToPlayerPosVec = transform.position - inputVecWorldSpace;
        fromInputWorldPosToPlayerPosVec.y = 0.0f;
        fromInputWorldPosToPlayerPosVec.Normalize();

        //Create a rotation towards the direction vector from the mouse position to the player position
        Quaternion rotation = Quaternion.LookRotation(fromInputWorldPosToPlayerPosVec);
        rotation = Quaternion.Slerp(transform.rotation, rotation, m_CharacterRotDelay);

        //Use the Movement vector without the height
        Vector3 tmpMovementVec = m_MovementVec;
        tmpMovementVec.y = 0.0f;
        if (tmpMovementVec.magnitude > 0.0f && inputVertAxis !=  0.0f)
        {
            transform.rotation = rotation; //Rotate the character towards this new direction
        }
    }

    void CharacterGroundChecking()
    {
        if(m_CharacterController.isGrounded)
        {
            m_DownWardVelocity = m_DownWardForce;
        }

        m_DownWardVelocity += m_Gravity * Time.deltaTime;

        m_MovementVec.y = m_DownWardVelocity * Time.deltaTime;
    }

    void CharacterAnimation()
    {
        if(m_Animator != null && m_GemMining != null)
        {
            Vector3 movementDirVector = m_MovementVec.normalized;

            float forwardMotionValue = Vector3.Dot(transform.forward, movementDirVector);
            float rightMotionValue = Vector3.Dot(transform.right, movementDirVector);

            m_Animator.SetFloat("ForwardMotion", forwardMotionValue);
            m_Animator.SetFloat("RightMotion", rightMotionValue);
            m_Animator.SetBool("IsShooting", m_GemMining.IsMiningGem);
        }
    }

    void SwitchCameras()
    {
        //Change Virtual camera priorities to blend between them
        if(m_FreeAimVirtualPlayerCamera 
            && m_ShootVirtualPlayerCamera)
        {
            if(m_GemMining.IsMiningGem)
            {
                //Decrease priority of Free aim camera
                m_FreeAimVirtualPlayerCamera.Priority = m_LowestVirtualCameraPriority;

                //Increase priority of shoot camera
                m_ShootVirtualPlayerCamera.Priority = m_HighestVirtualCameraPriority;
            }
            else
            {
                // Increase priority of Free aim camera
                m_FreeAimVirtualPlayerCamera.Priority = m_HighestVirtualCameraPriority;

                //Descrease priority of shoot camera
                m_ShootVirtualPlayerCamera.Priority = m_LowestVirtualCameraPriority;
            }
        }
    }

    void MineGems()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();

        m_Animator = GetComponent<Animator>();

        m_GemMining = GetComponentInChildren<PlayerGemMining>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorz = Input.GetAxis("Horizontal");
        float inputVert = Input.GetAxis("Vertical");
        m_GemMining.IsMiningGem = false;

        if(m_GemMining != null && m_CharacterController != null)
        {
            if (Input.GetButton("Fire1"))
            {
                m_GemMining.IsMiningGem = true;
            }

            CharacterMovement(inputHorz, inputVert);

            CharacterGroundChecking();

            CharacterRotationFreeCameraAim(inputHorz, inputVert);

            //Move the character
            m_CharacterController.Move(m_MovementVec);

            if (m_GemMining.IsMiningGem)
            {
                CharacterRotationFixedCameraAim();
            }
            else
            {
                CharacterRotationFreeCameraAim(inputHorz, inputVert);
            }

            SwitchCameras();

            //Animate the character
            CharacterAnimation();

            //
            m_GemMining.MineGem();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Ore") collision.gameObject.GetComponent<Rotate>().DestroyMe();

    }

    #endregion Methods
}
