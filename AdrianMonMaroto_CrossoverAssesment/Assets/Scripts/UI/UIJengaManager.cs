using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIJengaManager : MonoBehaviour
{
    [SerializeField] UIDatanfoPanel _uiDataInfoPanel;
    public UnityAction OnHidedAllEvent;

    [SerializeField] Button _testMyStackBtn;
    public UnityAction OnTestMyStackEvent;

    private void Start()
    {
        _testMyStackBtn.onClick.AddListener(OnTestMyStackEvent);
        HideAll();
    }

    public void HideAll()
    {
        _uiDataInfoPanel.gameObject.SetActive(false);

        if (OnHidedAllEvent != null) { 
            OnHidedAllEvent.Invoke();
        }
    }

    public void ShowDataInfo(string gradeLevel, string domain, string cluster, string standardId, string StandardDescription)
    {
        HideAll();      // just in case we put more panels

        _uiDataInfoPanel.gameObject.SetActive(true);
        _uiDataInfoPanel.SetInfoText(gradeLevel, domain, cluster, standardId, StandardDescription);
    }
}
