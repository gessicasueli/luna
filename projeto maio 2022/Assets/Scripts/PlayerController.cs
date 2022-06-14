using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   private GameControls _gameControls;
   private PlayerInput _playerInput;
   private Camera _mainCamera;
   private Vector2 _moveInput;
   private Rigidbody _rigidbody;
   
   public float moveMultiplier;

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
   }

   private void Move()
   {
       _rigidbody.AddForce((_mainCamera.transform.forward * _moveInput.y + _mainCamera.transform.right * _moveInput.x)* moveMultiplier * Time.fixedDeltaTime);
   }

   private void FixedUpdate()
   {
      Move();
   }
}
