using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{ 
    public int coins = 0;

    public TMP_Text coinText;
   private GameControls _gameControls;
   private PlayerInput _playerInput;
   private Camera _mainCamera;
   private Vector2 _moveInput;
   private Rigidbody _rigidbody;

   private bool _isGrounded;
   
   public float moveMultiplier;

   public float maxVelocity;

   public float rayDistance;

   public LayerMask layerMask;

   public float jumpForce;
   private void OnEnable()
   {
       _rigidbody = GetComponent<Rigidbody>();
       
      _gameControls = new GameControls();

      _playerInput = GetComponent<PlayerInput>();
      
      _mainCamera = Camera.main;

      _playerInput.onActionTriggered += OnActionTriggered; 
   }

   private void OnDisable()
   {
       _playerInput.onActionTriggered -= OnActionTriggered; 
   }

   private void OnActionTriggered(InputAction.CallbackContext obj)
   {
       if (obj.action.name.CompareTo(_gameControls.Gameplay.Move.name) == 0)
       {
           _moveInput = obj.ReadValue<Vector2>();
       }

       if (obj.action.name.CompareTo(_gameControls.Gameplay.Jump.name) == 0)
       {
           if(obj.performed) Jump();
       }
   }

   private void Move()
   {
       Vector3 camForward = _mainCamera.transform.forward; 
       Vector3 camRight = _mainCamera.transform.right;
       camForward.y = 0;
       camRight.y = 0;
       
       _rigidbody.AddForce((camForward * _moveInput.y + camRight * _moveInput.x)* moveMultiplier * Time.fixedDeltaTime);
   }

   private void FixedUpdate()
   {
      Move();
      LimitVelocity();
   }

   private void LimitVelocity()
   {
       Vector3 velocity = _rigidbody.velocity;

       if (Mathf.Abs(velocity.x) > maxVelocity) velocity.x = Mathf.Sign(velocity.x) * maxVelocity;

       //velocity.x = Mathf.Clamp(value: velocity.x , min: - maxVelocity, maxVelocity);
       
       if (Mathf.Abs(velocity.z) > maxVelocity) velocity.z = Mathf.Sign(velocity.z) * maxVelocity;
       
       // velocity.z = Mathf.Clamp(value: velocity.z , min: - maxVelocity, maxVelocity);

       _rigidbody.velocity = velocity; 
   }

   private void CheckGround()
   {
       RaycastHit collision;

       if (Physics.Raycast(transform.position, Vector3.down, out collision, rayDistance, layerMask))
       {
           _isGrounded = true;
       }

       else
       {
           _isGrounded = false; 
       }
   }
   private void Jump()
   {
       if (_isGrounded)
       {
           _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
       }
   }

   private void Update()
   {
       CheckGround();
   }

   private void OnDrawGizmos()
   {
       Debug.DrawRay(start: transform.position, dir: Vector3.down * rayDistance, Color.yellow);
   }

   private void OnTriggerEnter(Collider other)
   {
       if (other.CompareTag("coin"))
       {
           coins++;
           coinText.text = coins.ToString();
           Destroy(other.gameObject);
       }
   }
}
