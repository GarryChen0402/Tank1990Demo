using UnityEngine;

public class Boss : MonoBehaviour
{
    public void GetShot()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.PlayFx("Blast");
        GameManager.Instance.SwitchToLoose();
    }
}
