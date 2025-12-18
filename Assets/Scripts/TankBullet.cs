using System;
using UnityEngine;

public class TankBullet : MonoBehaviour
{
    [Header("Sprite Resources")]
    public Sprite topSprite;
    public Sprite bottomSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    [Header("Move Settings")]
    [SerializeField] private MoveDirection moveDirection;
    [SerializeField] private float moveSpeed;


    [Header("Broken Settings")]
    public GameObject owner;
    [SerializeField] private bool isBroken;

    [Header("Attack Settings")]
    [SerializeField] private string targetTag;

    public MoveDirection MovingDirection { get => moveDirection; set => moveDirection = value; }
    public String TargetTag { get => targetTag; set => targetTag = value; }

    private void Start()
    {
        SpriteInitialization();
        isBroken = false;
    }

    private void OnEnable()
    {
        //Debug.Log("Bullet enabled");
    }

    private void Update()
    {
        if (owner == null) Broken();
        MoveUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (owner == null) return;
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject == owner) return;
        if (collision.gameObject.tag.Equals("Untagged")) Broken();
        else if (collision.gameObject.tag.Equals(gameObject.tag))
        {
            collision.gameObject.GetComponent<TankBullet>().Broken();
            Broken();
        }
        else if (collision.gameObject.tag.Equals("Boss"))
        {
            collision.gameObject.GetComponent<Boss>().GetShot();
            Broken();
        }
        if (collision.gameObject.tag.Equals(owner.tag))
        {
            Broken();
        }
        else if (collision.gameObject.tag.Equals("BrickWall")){
            collision.gameObject.GetComponent<BrickWall>().GetShot();
            Broken();
        }else if (collision.gameObject.tag.Equals("SteelWall"))
        {
            collision.gameObject.GetComponent<SteelWall>().GetShot();
            Broken();
        }else if (collision.gameObject.tag.Equals(targetTag))
        {
            if(targetTag.Equals("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().GetShot();
                Broken();
            }else if(targetTag.Equals("Player"))
            {
                PlayerTank.Instance.GetShot();
                Broken();
            }
            
        }else if (collision.gameObject.tag.Equals("Water") || collision.gameObject.tag.Equals("Item"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void SpriteInitialization()
    {
        switch (moveDirection)
        {
            case MoveDirection.Up:
                GetComponent<SpriteRenderer>().sprite = topSprite;
                break;
            case MoveDirection.Down:
                GetComponent<SpriteRenderer>().sprite = bottomSprite;
                break;
            case MoveDirection.Left:
                GetComponent<SpriteRenderer>().sprite = leftSprite;
                break;
            case MoveDirection.Right:
                GetComponent<SpriteRenderer>().sprite = rightSprite;
                break;
        }
    }

    private void MoveUpdate()
    {
        Vector3 moveVector3 = Vector3.zero;
        switch (moveDirection)
        {
            case MoveDirection.Up:
                moveVector3 = Vector3.up;
                break;
            case MoveDirection.Down:
                moveVector3 = Vector3.down;
                break;
            case MoveDirection.Left:
                moveVector3 = Vector3.left;
                break;
            case MoveDirection.Right:
                moveVector3 = Vector3.right;
                break;
        }
        transform.Translate(moveVector3 * moveSpeed * Time.deltaTime);
        if (Math.Abs(transform.position.x) >= 15 || Math.Abs(transform.position.y) >= 15) Broken();
    }

    private void Broken()
    {
        if (isBroken) return;
        isBroken = true;
        if (owner != null && owner.tag.Equals("Player")) PlayerTank.Instance.BulletBroken();
        else if (owner != null && owner.tag.Equals("Enemy")) owner.GetComponent<Enemy>().BulletBroken();
        AudioManager.Instance?.PlayFx("Hit");
        Destroy(gameObject);
    }
}
