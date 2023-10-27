using UnityEngine;
using UnityEngine.Events;

public class UIJengaManager : MonoBehaviour
{
    [SerializeField] UIDatanfoPanel _uiDataInfoPanel;
    public UnityAction OnHidedAll;

    private void Start()
    {
        HideAll();
    }

    public void HideAll()
    {
        _uiDataInfoPanel.gameObject.SetActive(false);

        if (OnHidedAll != null) { 
            OnHidedAll.Invoke();
        }
    }

    public void ShowDataInfo(string gradeLevel, string domain, string cluster, string standardId, string StandardDescription)
    {
        HideAll();      // just in case we put more panels

        _uiDataInfoPanel.gameObject.SetActive(true);
        _uiDataInfoPanel.SetInfoText(gradeLevel, domain, cluster, standardId, StandardDescription);
    }
}
