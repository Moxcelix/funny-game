using UnityEngine;

using Movement = BodyMovement.Movement;
using Coroutine = System.Collections.IEnumerator;
public class BasicEnemy : BodyController
{
    private enum MoodState : int
    {
        AGGRESSIVE,
        PACIFIC,

        // utilitarian
        END
    }

    private enum PacificState : int
    {
        STANDING,
        WALKING,
        RUNNING,
        JUMP_RUNNING,

        // utilitarian
        END
    }

    [SerializeField] private Transform attackTarget;
    private Vector3 pacificTarget;

    private readonly float rotationSpeed = 5f;
    private readonly float rotationAccuracy = 5f;
    private readonly float attackDistanse = 3f;
    private readonly float maxSecondsForState = 5f;

    private PacificState state = PacificState.WALKING;

    private void Start()
    {
        StartCoroutine(StateCycle());
        StartCoroutine(TargetCycle());
    }

    protected override void GetInput()
    {
        var target = GetTarget(MoodState.PACIFIC);

        Move();
        RotateHead(rotationSpeed, target);
    }

    private void Move()
    {
        IsRuning = false;
        IsJumping = false;

        var forward = Movement.NONE;
        var back = Movement.NONE;
        var right = Movement.NONE;
        var left = Movement.NONE;

        switch (state)
        {
            case PacificState.STANDING:
                Stand();
                break;

            case PacificState.WALKING:
                Walk(forward: ref forward, back: ref back,
                     right: ref right, left: ref left);
                break;

            case PacificState.RUNNING:
                Run(ref forward);
                break;

            case PacificState.JUMP_RUNNING:
                JumpRun(ref forward);
                break;

            default:
                break;
        };

        SimpleMove(ref forward);

        HorizontalMovement = (Movement)((int)right + (int)left);
        VerticalMovement = (Movement)((int)forward + (int)back);
    }

    private void SimpleMove(ref Movement movement)
    {
        movement = Movement.NORMAL;
    }

    private void Stand()
    {

    }

    private void Walk(ref Movement forward, ref Movement back,
                      ref Movement right, ref Movement left)
    {
        int variant = Random.Range(0, 5);

        switch (variant)
        {
            case 0:
                SimpleMove(ref forward);
                break;
            case 1:
                SimpleMove(ref back);
                break;
            case 2:
                SimpleMove(ref left);
                break;
            case 3:
                SimpleMove(ref right);
                break;
            case 4:
                Run(ref forward);
                break;
        }
    }

    private void Run(ref Movement movement)
    {
        IsRuning = true;
        SimpleMove(ref movement);
    }

    private void JumpRun(ref Movement movement)
    {
        IsJumping = true;
        Run(ref movement);
    }

    private void RotateHead(float speed, Vector3 target)
    {
        var rotationDelta = RotationDelta;
        var angle = Vector3.SignedAngle(transform.position - target,
                                        transform.forward, Vector3.up);

        var usignedAngle = Mathf.Abs(angle);
        var singOfAngle = Mathf.Sign(angle);
        var middleAngle = 180;
        var zeroAngle = 0;

        angle = (middleAngle - usignedAngle) * singOfAngle;
        angle = (usignedAngle > rotationAccuracy) ? angle : zeroAngle;
        rotationDelta.x = angle * Time.deltaTime * speed;

        RotationDelta = rotationDelta;
    }

    private Vector3 GetTarget(MoodState moodState)
    {
        return moodState == MoodState.AGGRESSIVE ?
                            attackTarget.position : pacificTarget;
    }

    private Coroutine StateCycle()
    {
        while (true)
        {
            state = (PacificState)Random.Range(0, (int)PacificState.END);

            yield return new WaitForSeconds(Random.Range(0, maxSecondsForState));
        }
    }

    private Coroutine TargetCycle()
    {
        while (true)
        {
            var dist = 100;
            var x = Random.Range(-dist, dist);
            var z = Random.Range(-dist, dist);

            pacificTarget = new(x, 0, z);

            yield return new WaitForSeconds(Random.Range(0, maxSecondsForState));
        }
    }
}
