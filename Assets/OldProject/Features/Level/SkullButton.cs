using UnityEngine;

public class SkullButton : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Transform enviroment;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float lerp;
    [SerializeField] private Color defaultCommand;
    [SerializeField] private Color activeCommand;
    private GameObject _playerContainer;
    private bool _pressed;

    private void Start()
    {
        GameObject.Find("Directional Light").transform.SetParent(enviroment);
        var player = GameObject.Find("OVRCameraRig").transform;
        _playerContainer = Instantiate(new GameObject());
        _playerContainer.name = "playerContainer";
        _playerContainer.transform.position = enviroment.position;
        player.transform.SetParent(_playerContainer.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _pressed = true;
            mesh.material.color = activeCommand;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _pressed = false;
            mesh.material.color = defaultCommand;
        }
    }
    
    void Update()
    {
        if (_pressed)
        {
            var rot2 = enviroment.rotation;
            rot2.y += rotationSpeed * Time.deltaTime;
            enviroment.localRotation = Quaternion.Lerp(enviroment.localRotation, rot2, lerp);
            _playerContainer.transform.rotation = Quaternion.Lerp(_playerContainer.transform.rotation, rot2, lerp);
        }
    }
}
