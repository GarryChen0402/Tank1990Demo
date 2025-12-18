using UnityEngine;


public class BasicItem : MonoBehaviour
{
    private float livingTime;

    private void Awake()
    {
        livingTime = 10f;
        ItemManager.Instance.AddItem(gameObject);
    }

    protected virtual void Update()
    {
        livingTime -= Time.deltaTime;
        if(livingTime <= 0)
        {
            RemoveItem();
        }
    }

    public virtual void OnCollected()
    {
        Debug.Log("This function will be called when the item be collected.");
        AudioManager.Instance?.PlayFx("Item");
        Destroy(gameObject);
    }

    public void RemoveItem()
    {
        ItemManager.Instance.RemoveItem(gameObject);
        Destroy(gameObject);
    }

}