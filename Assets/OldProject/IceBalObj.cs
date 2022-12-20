using UnityEngine;

public class IceBalObj : MonoBehaviour
{
    [SerializeField] private IceFloor floor;
    [SerializeField] private GameObject ball;

    public void MakeFloor()
    {
        floor.gameObject.SetActive(true);
        ball.gameObject.SetActive(false);
    }
}
