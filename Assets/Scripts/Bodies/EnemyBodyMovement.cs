using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyMovement : BodyMovement
{
    protected override void UpdateInput()
    {
        if (!controller) return;

        horizontalMovement = controller.HorizontalMovement;
        verticalMovement = controller.VerticalMovement;
        isRuning = controller.IsRuning;
        isJumping = controller.IsJumping;

        rotationDelta = controller.Rotation;
    }
}
