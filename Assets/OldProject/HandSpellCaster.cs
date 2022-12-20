using DG.Tweening;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

public class HandSpellCaster : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color castColor;
    [SerializeField] private SkinnedMeshRenderer leftMesh;
    [SerializeField] private SkinnedMeshRenderer rightMesh;
    [SerializeField] private float duration = 15f;
    [SerializeField] private HandPhysicsCapsules rightHandCapsules;
    [SerializeField] private HandPhysicsCapsules leftHandCapsules;
    [SerializeField] private SpellAim spellAim;
    [SerializeField] private SpellShooter spellShooter;
    [SerializeField, Interface(typeof(ISelector))]
    private MonoBehaviour rightSelector;
    [SerializeField, Interface(typeof(ISelector))]
    private MonoBehaviour leftSelector;

    private ISelector _rightSelector;
    private ISelector _leftSelector;

    private bool _leftSelected;
    private bool _rightSelected;
    private bool _isActive;

    private void Awake()
    {
        _rightSelector = rightSelector as ISelector;
        _leftSelector = leftSelector as ISelector;
        _rightSelector.WhenSelected += () =>
        {
            _rightSelected = true;
            if(_leftSelected)
            {
                Cast();
            }
        };
        _rightSelector.WhenUnselected += () =>
        {
            _rightSelected = false;
        };
        _leftSelector.WhenSelected += () =>
        {
            _leftSelected = true;
            if (_rightSelected)
            {
                Cast();
            }
        };
        _leftSelector.WhenUnselected += () =>
        {
            _leftSelected = false;
        };
    }

    private void Cast()
    {
        if(!_isActive)
        {
            _isActive = true;
            rightMesh.material.SetColor("_ColorTop", castColor);
            leftMesh.material.SetColor("_ColorTop", castColor);
            leftHandCapsules.enabled = true;
            rightHandCapsules.enabled = true;
            spellAim.enabled = false;
            spellShooter.enabled = false;
            DOVirtual.DelayedCall(duration, () => 
            {
                spellAim.enabled = true;
                spellShooter.enabled = true;
                leftHandCapsules.enabled = false;
                rightHandCapsules.enabled = false;
                rightMesh.material.SetColor("_ColorTop", defaultColor);
                leftMesh.material.SetColor("_ColorTop", defaultColor);
                _isActive = false;
            });
        }
    }
}
