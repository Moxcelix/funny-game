using UnityEngine;

using Movement = BodyMovement.Movement;
public class PlayerController : BodyController
{
    const string mouseXtag = "Mouse X";
    const string mouseYtag = "Mouse Y";

    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backKey = KeyCode.S;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runKey = KeyCode.LeftControl;
    [SerializeField] private float mouseSensitivity = 10;
    
    protected override void GetInput()
    {
        IsRuning = Input.GetKey(runKey);
        IsJumping = Input.GetKey(jumpKey);

        var forward = Input.GetKey(forwardKey) ? Movement.NORMAL : Movement.NONE;
        var back = Input.GetKey(backKey) ? Movement.NORMAL : Movement.NONE;
        var right = Input.GetKey(rightKey) ? Movement.NORMAL : Movement.NONE;
        var left = Input.GetKey(leftKey) ? Movement.NORMAL : Movement.NONE;

        HorizontalMovement = (Movement)((int)right - (int)left);
        VerticalMovement = (Movement)((int)forward - (int)back);

        Vector3 rotationDelta = RotationDelta;

        rotationDelta.x = Input.GetAxis(mouseXtag) * mouseSensitivity;
        rotationDelta.y = Input.GetAxis(mouseYtag) * mouseSensitivity;

        RotationDelta = rotationDelta;
    }
}
