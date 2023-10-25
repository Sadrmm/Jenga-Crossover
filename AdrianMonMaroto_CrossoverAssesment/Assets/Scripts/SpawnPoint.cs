using TMPro;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] TextMeshPro _gradeText;

    public void SetText(string newText) {
        _gradeText.text = newText;
    }
}
