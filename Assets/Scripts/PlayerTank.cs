using Unity.VisualScripting;
using UnityEngine;

//enum MoveDirection
//{
//    Up, Down, Left, Right
//}

public class PlayerTank : SingletonClass<PlayerTank>
{
    [Header("Control Settings")]
    [SerializeField] private bool canControl;

    [Header("Move Settings")]
    [SerializeField] private bool isMovingTop;
    [SerializeField] private bool isMovingBottom;
    [SerializeField] private bool isMovingLeft;
    [SerializeField] private bool isMovingRight;
    [SerializeField] private float moveSpeed;
    [SerializeField] private MoveDirection facingDirection;
    public float MoveSpeed { get => moveSpeed; }

    [Header("Sprite Resources")]
    public Sprite spriteTop;
    public Sprite spriteBottom;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    [Header("Attack Settings")]
    public GameObject bulletPrefabs;
    [SerializeField] private bool canAttack;
    [SerializeField] private int leftBulletCount;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Animator animator;
    [SerializeField] private int leftLife;
    public GameObject explosionPrefab;


    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();

        canAttack = false;
        leftBulletCount = 1;
        spawnPosition = new Vector3(-4, -12, 0);
        canControl = false;
        leftLife = 1;
    }

    private void Update()
    {
        if (!canControl) return;
        InputUpdate();
        MoveUpdate();
        SpriteUpdate();
        AttackUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Item"))
        {
            Debug.Log("Get the item " + collision.gameObject.name);
            collision.gameObject.GetComponent<BasicItem>().OnCollected();
            //Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void InputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W)) isMovingTop = true;
        else if (Input.GetKeyUp(KeyCode.W)) isMovingTop = false;

        if (Input.GetKeyDown(KeyCode.S)) isMovingBottom = true;
        else if (Input.GetKeyUp(KeyCode.S)) isMovingBottom = false;

        if (Input.GetKeyDown(KeyCode.A)) isMovingLeft = true;
        else if (Input.GetKeyUp(KeyCode.A)) isMovingLeft = false;

        if (Input.GetKeyDown(KeyCode.D)) isMovingRight = true;
        else if (Input.GetKeyUp(KeyCode.D)) isMovingRight = false;

        if (Input.GetKeyDown(KeyCode.Space)) canAttack = true;
        else if (Input.GetKeyUp(KeyCode.Space)) canAttack = false;
    }

    private void MoveUpdate()
    {
        Vector3 moveDirection = Vector3.zero;

        if(isMovingTop && !isMovingBottom)
        {
            moveDirection = Vector3.up;
            facingDirection = MoveDirection.Up;
        }
        else if(!isMovingTop && isMovingBottom)
        {
            moveDirection = Vector3.down;
            facingDirection = MoveDirection.Down;
        }
        else if(isMovingLeft && !isMovingRight)
        {
            moveDirection = Vector3.left;
            facingDirection = MoveDirection.Left; 
        }
        else if(!isMovingLeft && isMovingRight)
        {
            moveDirection = Vector3.right;
            facingDirection = MoveDirection.Right;
        }

        transform.Translate(moveDirection * MoveSpeed *  Time.deltaTime);
    }

    private void SpriteUpdate()
    {
        if (isMovingTop && !isMovingBottom) GetComponent<SpriteRenderer>().sprite = spriteTop;
        else if (!isMovingTop && isMovingBottom) GetComponent<SpriteRenderer>().sprite = spriteBottom;
        else if (isMovingLeft && !isMovingRight) GetComponent<SpriteRenderer>().sprite = spriteLeft;
        else if (!isMovingLeft && isMovingRight) GetComponent<SpriteRenderer>().sprite = spriteRight;
    }

    private void AttackUpdate()
    {
        if (!canAttack || (canAttack && leftBulletCount <= 0)) return;
        Vector3 spawnPosition = transform.position;
        switch (facingDirection)
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
        bullet.GetComponent<TankBullet>().MovingDirection = facingDirection;
        bullet.GetComponent<TankBullet>().owner = gameObject;
        bullet.GetComponent<TankBullet>().TargetTag = "Enemy";
        AudioManager.Instance?.PlayFx("Fire");
    }

    public void BulletBroken()
    {
        leftBulletCount++;
    }

    public void GetShot()
    {
        Debug.Log("player get shot " + leftLife.ToString());
        canControl = false;
        GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        //GetComponent<SpriteRenderer>().enabled = false;
        //gameObject.SetActive(false);
        Spawn();
    }

    public void Spawn()
    {
        if (leftLife == 0)
        {
            //gameObject.SetActive(false);
            NoMoreLife();
            return;
        }
        leftLife--;
        ResetState();

        //isMovingBottom
    }

    public void ResetState()
    {
        isMovingTop = false;
        isMovingBottom = false;
        isMovingLeft = false;
        isMovingRight = false;

        transform.position = spawnPosition;
        facingDirection = MoveDirection.Up;
        GetComponent<SpriteRenderer>().sprite = spriteTop;
        animator.SetTrigger("Spawn");
    }


    public void CanControl()
    {
        canControl = true;
    }

    private void NoMoreLife()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SwitchToLoose();
    }

    public void AddLife()
    {
        leftLife++;
    }
}
