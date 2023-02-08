using UnityEngine;

using Movement = BodyMovement.Movement;

public abstract class BodyController : MonoBehaviour
{
    public Movement HorizontalMovement { get; protected set; } = Movement.NONE;
    public Movement VerticalMovement { get; protected set; } = Movement.NONE;
    public bool IsRuning { get; protected set; } = false;
    public bool IsJumping { get; protected set; } = false;
    public Vector3 RotationDelta { get; protected set; } = Vector3.zero;
    public Vector3 Rotation { get; protected set; } = Vector3.zero;

    protected abstract void GetInput();

    private void Update()
    {
        GetInput();
    }

}

