using UnityEngine;

public class UIJengaManager : MonoBehaviour
{
    [SerializeField] UIDatanfoPanel _uiDataInfoPanel;

    private void Start()
    {
        HideAll();
    }

    public void HideAll()
    {
        _uiDataInfoPanel.gameObject.SetActive(false);
    }

    public void ShowDataInfo(string gradeLevel, string domain, string cluster, string standardId, string StandardDescription)
    {
        HideAll();      // just in case we put more panels

        _uiDataInfoPanel.gameObject.SetActive(true);
        _uiDataInfoPanel.SetInfoText(gradeLevel, domain, cluster, standardId, StandardDescription);
    }
}
