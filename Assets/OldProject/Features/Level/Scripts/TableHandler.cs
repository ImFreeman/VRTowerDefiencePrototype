using UnityEngine;

namespace Features.Level.Scripts
{
    public class TableHandler : MonoBehaviour
    {
        [SerializeField] private GameObject levelPrefab;
        [SerializeField] private Transform bodyTransform;
        [SerializeField] private float tableAppearTime;
        [SerializeField] private float offset;

        private Vector3 _prevPosition;
        private float _currTime;

        private void Start()
        {
            _prevPosition = bodyTransform.position;
        }

        void Update()
        {
            _currTime += Time.deltaTime;
            var currPos = bodyTransform.position;
            if (Vector3.Distance(currPos, _prevPosition) > offset)
            {
                _prevPosition = currPos;
                _currTime = 0;
            }
            else
            {
                if (_currTime > tableAppearTime)
                {
                    var lvl = Instantiate(levelPrefab);
                    var pos = lvl.transform.position;
                    pos = bodyTransform.position;
                    pos.y -= 0.04f;
                    pos.z -= 0.17f;
                    pos.x -= 0.14f;
                    lvl.transform.position = pos;
                    Debug.Log($"body {bodyTransform.position}, lvl {lvl.transform.position}");
                    Destroy(gameObject);
                }
            }
        }
    }
}
