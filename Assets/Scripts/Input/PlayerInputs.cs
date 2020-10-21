using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Inputs.Mirror
{
    public class PlayerInputs : MonoBehaviour
    {
        [SerializeField] CarEngine engine = null;
        [SerializeField] Turbo boost = null;
        [SerializeField] BrakeManager brake = null;
        [SerializeField] Car car = null;

        [SerializeField] Vector2 move = Vector2.zero;
        [SerializeField] bool boosting = false;
        [SerializeField] bool flipping = false;
        public void Start()
        {
            enabled = true;
            engine = GetComponent<CarEngine>();
            brake = GetComponent<BrakeManager>();
            boost = GetComponent<Turbo>();
            car = GetComponent<Car>();

            InputManager.Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>().x);
            InputManager.Controls.Player.Move.canceled += _ => StopMovement();

            InputManager.Controls.Player.MoveKey.performed += ctx => SetMovement(ctx.ReadValue<float>());
            InputManager.Controls.Player.MoveKey.canceled += _ => StopMovement();

            InputManager.Controls.Player.Motor.performed += ctx => SetMotor(ctx.ReadValue<float>());
            InputManager.Controls.Player.Motor.canceled += _ => StopMotor();

            InputManager.Controls.Player.Boost.performed += _ => StartBoost();
            InputManager.Controls.Player.Boost.canceled += _ => StopBoost();

            InputManager.Controls.Player.Flip.performed += _ => Flip();

            InputManager.Controls.Player.Shoot.performed += _ => Shoot();

        }

        private void SetMovement(float input)
        {
            
            move.y = input;
            engine.SetInputX(input);
            brake.SetInputX(input);
        }

        private void StopMovement()
        {
            
            move = Vector2.zero;
            engine.SetInputX(0f);
            brake.SetInputX(0f);
        }

        private void SetMotor(float input)
        {

            move.y = input;
            engine.SetInputY(input);
            brake.SetInputY(input);
        }

        private void StopMotor()
        {

            move = Vector2.zero;
            engine.SetInputY(0f);
            brake.SetInputY(0f);
        }

        private void StartBoost()
        {
            
            boosting = true;
            Debug.Log("PlayerInputs : StartBoost");
            boost.StartBoost();
        }

        private void StopBoost()
        {
            
            boosting = false;
            Debug.Log("PlayerInputs : StopBoost");
            boost.StopBoost();
        }

        private void Flip()
        {
            
            boosting = true;
            car.Flip();
            boosting = false;
        }

        private void Shoot()
        {
            car.Shoot();
        }
    }
}
