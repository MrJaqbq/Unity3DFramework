﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class SimplePlayerController : BasePlayerController
    {
        [Header("Input")]
        public string MoveX = "Move X";
        public string MoveY = "Move Y";

        private Vector2 CurrentInput = Vector2.zero;

        public Transform CurrentTarget;

        protected override void OnProcessControll()
        {
            if (!Pawn)
                return;

            Vector3 direction = Vector3.zero;
            if (Enabled)
            {
                CurrentInput.x = Input.GetAxis(MoveX);
                CurrentInput.y = Input.GetAxis(MoveY);

                if (CurrentTarget)
                {
                    var rawDistance = transform.position - CurrentTarget.position;
                    Pawn.DesiredForward = -Vector3.Normalize(rawDistance);
                }

                var flatVelocity = new Vector3(CurrentInput.x, 0, CurrentInput.y);
                    direction = Quaternion.LookRotation(Vector3.Normalize(Pawn.DesiredForward)) * flatVelocity;
                
                direction.Normalize();
            }
            
            Pawn.ProcessMovement(direction);
            Pawn.Tick();
        }

        protected override void OnStop()
        {
            Pawn.Stop();
        }

        protected override void OnFixedTick()
        {
            Pawn.FixedTick();
        }

        protected override void OnLateTick()
        {
            //if (IsAttacking)
            //    return;

            if (Enabled)
            {
                if (PawnCamera.transform.forward.magnitude > 0)
                {
                    Pawn.DesiredForward = Vector3.Slerp(Pawn.DesiredForward, new Vector3(
                        PawnCamera.transform.forward.x,
                        0,
                        PawnCamera.transform.forward.z
                    ), Time.deltaTime * 10);
                }
                
                PawnCamera.OnUpdate();
            }

            Pawn.LateTick();
        }
    }
}
