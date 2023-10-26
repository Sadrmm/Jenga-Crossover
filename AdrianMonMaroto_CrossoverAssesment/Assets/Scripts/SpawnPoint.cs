using TMPro;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] TextMeshPro _gradeText;
    [SerializeField] Transform _center;
    public Transform Center => _center;

    public void SetText(string newText) {
        _gradeText.text = newText;
    }
}
