using NUnit.Framework.Constraints;
using UnityEngine;

public class CommonEnemy : Enemy
{
    [Header("Sprite Resources")]
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    [Header("Move Settings")]
    [SerializeField] private MoveDirection movingDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isMovingTop;
    [SerializeField] private bool isMovingBottom;
    [SerializeField] private bool isMovingLeft;
    [SerializeField] private bool isMovingRight;
    [SerializeField] private float moveTimer;
    [SerializeField] private float moveTime;

    

    protected override void Awake()
    {
        base.Awake();

        moveTimer = 0;
        moveSpeed = 4;
        leftBulletCount = 1;
        attackTimer = 0;
        attackTime = 1 + Random.Range(-0.2f, 0.2f);
        movingDirection = MoveDirection.Down;
    }

    private void Update()
    {
        if (isFreeze) return;
        EnemyAIUpdate();
        SpriteUpdate();
        MoveUpdate();
        AttackUpdate();
    }

    public override void GetShot()
    {
        base.GetShot();
    }

    private void EnemyAIUpdate()
    {
        moveTimer += Time.deltaTime;
        if(moveTimer >= moveTime)
        {
            //movingDirection = MoveDirection.NoMove;
            moveTimer -= moveTime;
            GetAnotherMovingDirection();
            //ResetRandomMovingDirection();
        }

        attackTimer += Time.deltaTime;
        if(attackTimer >= attackTime)
        {
            attackTimer -= attackTime;
            canAttack = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string collisionTag = collision.gameObject.tag;
        //Debug.Log(collisionTag);
        if (collisionTag.Equals("Item"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            //Debug.Log(collisionTag);
        }
    }

    private void ResetRandomMovingDirection()
    {
        movingDirection = Random.Range(0, 4) switch
        {
            0 => movingDirection = MoveDirection.Up,
            1 => movingDirection = MoveDirection.Down,
            2 => movingDirection = MoveDirection.Left,
            3 => movingDirection = MoveDirection.Right,
            _ => movingDirection = MoveDirection.NoMove
        };
    }
    private void GetAnotherMovingDirection()
    {
        switch (movingDirection)
        {
            case MoveDirection.Up:
                movingDirection = Random.Range(0, 3) switch
                {
                    0 => MoveDirection.Down,
                    1 => MoveDirection.Left,
                    2 => MoveDirection.Right,
                    _ => MoveDirection.NoMove
                };
                break;
            case MoveDirection.Down:
                movingDirection = Random.Range(0, 3) switch
                {
                    0 => MoveDirection.Up,
                    1 => MoveDirection.Left,
                    2 => MoveDirection.Right,
                    _ => MoveDirection.NoMove
                };
                break;
            case MoveDirection.Left:
                movingDirection = Random.Range(0, 3) switch
                {
                    0 => MoveDirection.Up,
                    1 => MoveDirection.Down,
                    2 => MoveDirection.Right,
                    _ => MoveDirection.NoMove
                };
                break;
            case MoveDirection.Right:
                movingDirection = Random.Range(0, 3) switch
                {
                    0 => MoveDirection.Up,
                    1 => MoveDirection.Down,
                    2 => MoveDirection.Left,
                    _ => MoveDirection.NoMove
                };
                break;
            case MoveDirection.NoMove:
                movingDirection = Random.Range(0, 4) switch
                {
                    0 => MoveDirection.Up,
                    1 => MoveDirection.Down,
                    2 => MoveDirection.Left,
                    3 => MoveDirection.Right,
                    _ => MoveDirection.NoMove
                };
                break;
        }
    }
    private void MoveUpdate()
    {
        Vector3 moveDirection = Vector3.zero;

        switch (movingDirection)
        {
            case MoveDirection.Up:
                moveDirection = Vector3.up;
                break;
            case MoveDirection.Down:
                moveDirection = Vector3.down;
                break;
            case MoveDirection.Left:
                moveDirection = Vector3.left;
                break;
            case MoveDirection.Right:
                moveDirection = Vector3.right;
                break;
        }

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
    private void SpriteUpdate()
    {
        switch (movingDirection)
        {
            case MoveDirection.Up:
                GetComponent<SpriteRenderer>().sprite = upSprite;
                break;
            case MoveDirection.Down:
                GetComponent<SpriteRenderer>().sprite = downSprite;
                break;
            case MoveDirection.Left:
                GetComponent<SpriteRenderer>().sprite = leftSprite;
                break;
            case MoveDirection.Right:
                GetComponent<SpriteRenderer>().sprite = rightSprite;
                break;
        }
    }
    private void AttackUpdate()
    {
        if (!canAttack || (canAttack && leftBulletCount <= 0)) return;
        Vector3 spawnPosition = transform.position;
        switch (movingDirection)
        {
            case MoveDirection.Up:
                spawnPosition += new Vector3(-0.025f, 1, 0);
                break;
            case MoveDirection.Down:
                spawnPosition += new Vector3(0.025f, -1, 0);
                break;
            case MoveDirection.Left:
                spawnPosition += new Vector3(-1, -0.025f, 0);
                break;
            case MoveDirection.Right:
                spawnPosition += new Vector3(1, 0.025f, 0);
                break;
        }
        leftBulletCount--;
        canAttack = false;

        GameObject bullet = GameObject.Instantiate(bulletPrefabs, spawnPosition, Quaternion.identity);
        bullet.GetComponent<TankBullet>().MovingDirection = movingDirection;
        bullet.GetComponent<TankBullet>().owner = gameObject;
        bullet.GetComponent<TankBullet>().TargetTag = "Player";
        //AudioManager.Instance?.PlayFx("Fire");
    }

    

}
