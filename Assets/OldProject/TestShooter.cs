using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class TestShooter : MonoBehaviour
{
    [SerializeField] private Transform goal;
    [FormerlySerializedAs("ball")] [SerializeField] private IceBalObj bal;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            var ice = Instantiate(bal, transform.position, Quaternion.identity);
            var tween = 
                ice.transform
                    .DOMove(
                        goal.position,
                        2f * Vector3.Distance(goal.position, ice.transform.position)).
                    SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        ice.MakeFloor();
                        DOVirtual.DelayedCall(15f, () =>
                        {
                            ice.transform.DOScale(new Vector3(0, 0, 0), 1f).SetEase(Ease.Linear)
                                .OnComplete(() =>
                                {
                                    Destroy(ice.gameObject);
                                }); 
                        });
                    });
            DOVirtual.DelayedCall(15f, () =>
            {
                if (ice != null)
                {
                    tween.Kill();
                    Destroy(ice.gameObject);
                }
            });
        }
        
    }
}
