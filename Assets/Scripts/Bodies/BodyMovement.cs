using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BodyMovement : MonoBehaviour
{
    public enum Movement : int
    {
        NORMAL = 1,
        NONE = 0
    }

    [SerializeField] protected Transform headTransform;

    [SerializeField] protected float gravityScale = 0.3f;
    [SerializeField] protected float maxSpeed = 10;
    [SerializeField] protected float jumpForce = 10;
    [SerializeField] protected float runMultiplier = 1.5f;
    [SerializeField] protected float acceleration = 1f;

    protected Movement horizontalMovement = Movement.NONE;
    protected Movement verticalMovement = Movement.NONE;
    protected bool isRuning = false;
    protected bool isJumping = false;

    protected BodyController controller;
    protected CharacterController characterController;

    protected Vector3 rotationDelta = Vector3.zero;
    protected Vector3 velocityXZ = Vector3.zero;
    protected float velocityY = 0f;

    public void SetController(BodyController controller) => this.controller = controller;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateInput();
        UpdateView();
    }
    private void FixedUpdate()
    {
        UpdateMove();
        CalculateVelocity();
    }

    protected virtual void UpdateView()
    {
        rotationDelta.y = Mathf.Clamp(rotationDelta.y, -90, 90);
        headTransform.localEulerAngles = new Vector3(-rotationDelta.y, 0);
        transform.eulerAngles = new Vector3(0, rotationDelta.x);
    }

    protected virtual void UpdateInput()
    {
        if (!controller) return;

        horizontalMovement = controller.HorizontalMovement;
        verticalMovement = controller.VerticalMovement;
        isRuning = controller.IsRuning;
        isJumping = controller.IsJumping;

        rotationDelta += controller.RotationDelta;
    }

    protected virtual void CalculateVelocity()
    {
        var isGrounded = IsGrounded();
        var runMultiplier = isRuning ? this.runMultiplier : 1f;
        var decreaseAcceleration = isGrounded ? Resistance.Ground : Resistance.Air;

        velocityXZ += (int)horizontalMovement * runMultiplier *
            acceleration * Time.fixedDeltaTime * transform.right;

        velocityXZ += (int)verticalMovement * runMultiplier *
            acceleration * Time.fixedDeltaTime * transform.forward;

        PlanarCeil(ref velocityXZ, isRuning ? maxSpeed * runMultiplier : maxSpeed);

        if (verticalMovement == Movement.NONE && horizontalMovement == Movement.NONE)
            velocityXZ *= decreaseAcceleration;

        if (IsGrounded())
        {
            velocityY = isJumping ? jumpForce : 0;
        }
        else
        {
            velocityY += gravityScale * Time.fixedDeltaTime * Physics.gravity.y;
        }
    }

    protected virtual bool IsGrounded()
    {
        float eps = 0.3f;
        return Physics.Raycast(transform.position, -GetNormal(), characterController.height / 2.0f + eps);
    }
    protected virtual void UpdateMove()
    {
        Vector3 normal = GetNormal();
        Vector3 moveDirection = Project(velocityXZ, normal) + Vector3.up * velocityY;

        Debug.DrawRay(transform.position, Project(velocityXZ, normal).normalized, Color.red);

        characterController.Move(moveDirection);
    }
    private Vector3 Project(Vector3 vector, Vector3 normal)
    {
        return vector - Vector3.Dot(vector, normal) * normal;
    }

    private Vector3 GetNormal()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            return hit.normal;
        }

        return Vector3.zero;
    }

    private void PlanarCeil(ref Vector3 vector, float maxMagnitude)
    {
        var planarMovent = new Vector2(vector.x, vector.z);

        if (planarMovent.magnitude > maxMagnitude)
        {
            planarMovent = planarMovent.normalized * maxMagnitude;

            vector.x = planarMovent.x;
            vector.z = planarMovent.y;
        }
    }

}
