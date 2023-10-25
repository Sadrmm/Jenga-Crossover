using TMPro;
using UnityEngine;

public class JengaBlock : MonoBehaviour
{
    [SerializeField] private Material[] _blockMaterials;

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private TextMeshPro _gradeText;

    private StackData _data;
    public StackData Data => _data;

    [SerializeField] private float _blockHeight;
    public float BlockHeight => _blockHeight;

    public void Init(StackData Data)
    {
        _data = Data;

        _meshRenderer.sharedMaterial = _blockMaterials[_data.mastery];
        _gradeText.text = _data.grade;
    }
}
