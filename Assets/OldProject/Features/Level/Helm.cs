using UnityEngine;

public class Helm : MonoBehaviour
{
    [SerializeField] private Transform enviroment;
    [SerializeField] private HingeJoint joint;
    [SerializeField] private Transform jointPivot;
    private GameObject _playerContainer;

    private void Start()
    {
        joint.connectedAnchor = transform.position;
        GameObject.Find("Directional Light").transform.SetParent(enviroment);
        var player = GameObject.Find("OVRCameraRig").transform;
        _playerContainer = Instantiate(new GameObject());
        _playerContainer.transform.position = enviroment.position;
        player.transform.SetParent(_playerContainer.transform);
    }
    void Update()
    {
        joint.connectedAnchor = jointPivot.position;
        var rot = _playerContainer.transform.rotation;
        rot.y = transform.localRotation.z;
        rot.w = transform.localRotation.w;
        _playerContainer.transform.rotation = rot;
        var rot2 = enviroment.rotation;
        rot2.y = transform.localRotation.z;
        rot2.w = transform.localRotation.w;
        enviroment.localRotation = rot2;
    }
}
