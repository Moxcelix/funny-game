using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private PlayerController userController;
    [SerializeField] private BodyMovement playerMovement;

    [SerializeField] private BasicEnemy enemyController;
    [SerializeField] private BodyMovement enemyMovement;

    private void Awake()
    {
        playerMovement.SetController(userController);
        enemyMovement.SetController(enemyController);
    }
}
