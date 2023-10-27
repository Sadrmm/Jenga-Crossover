using TMPro;
using UnityEngine;

public class UIDatanfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _infoText;

    public void SetInfoText(string gradeLevel, string domain, string cluster, string standardId, string StandardDescription)
    {
        _infoText.text = $"{gradeLevel}: {domain}\n" +
            $"{cluster}\n" +
            $"{standardId}: {StandardDescription}";
    }
}
