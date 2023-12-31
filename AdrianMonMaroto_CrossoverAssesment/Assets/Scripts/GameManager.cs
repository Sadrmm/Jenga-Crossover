using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum ORDER
    {
        PREVIOUS = -1,
        SAME = 0,
        NEXT = 1
    }

    [Header("Prefabs")]
    [SerializeField] JengaBlock _jengaBlock;
    [SerializeField] GameObject _jengaRow;
    [SerializeField] LayerMask _jengaLayerMask;

    [Header("UI")]
    [SerializeField] UIJengaManager _uiJengaManager;

    [Header("Level")]
    [SerializeField] SpawnPoint[] _spawnPoints;
    [SerializeField] CameraControl _cameraControl;
    
    [Min(0)]
    [SerializeField] private int _focusIndex;

    private List<JengaBlock[]> _stacksJengaBlocks;
    private List<bool> _stacksTested;

    private void OnEnable()
    {
        _uiJengaManager.OnHidedAllEvent += EnableCameraControl;
        _uiJengaManager.OnTestMyStackEvent += TestMyStack;
    }

    private void OnDisable()
    {
        _uiJengaManager.OnHidedAllEvent -= EnableCameraControl;
        _uiJengaManager.OnTestMyStackEvent -= TestMyStack;
    }

    IEnumerator Start()
    {
        int firstIndex = 0;
        _stacksJengaBlocks = new List<JengaBlock[]>();
        _stacksTested = new List<bool>();

        yield return StartCoroutine(APICall.GetRequest());

        SpawnJengaStack(_spawnPoints[0], ref firstIndex);
        SpawnJengaStack(_spawnPoints[1], ref firstIndex);
        SpawnJengaStack(_spawnPoints[2], ref firstIndex);
        
        // just in case the designer put an incorrect index, we put the latest
        if (_focusIndex > _spawnPoints.Length) {
            _focusIndex = _spawnPoints.Length - 1;
        }
        SwitchStackFocus(ORDER.SAME);
    }

    private void Update()
    {
        ChangeStack();
        HideShowJengaInfo();
    }

    private void TestMyStack()
    {
        // if already tested, do nothing
        if (_stacksTested[_focusIndex]) { 
            return;
        }

        // delete glass blocks and let physics act
        for (int i = 0; i < _stacksJengaBlocks[_focusIndex].Length; i++) {
            JengaBlock currentBlock = _stacksJengaBlocks[_focusIndex][i];

            if (currentBlock == null) {
                Debug.LogError("Check here, currentBlock null");
                break;
            }

            if (currentBlock.Data.mastery != (int)JengaBlock.BlockType.Glass) {
                continue;
            }

            Destroy(currentBlock.gameObject);
        }

        // set as tested
        _stacksTested[_focusIndex] = true;
    }

    private void HideShowJengaInfo()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10.0f);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, _jengaLayerMask))
            {
                Debug.Log(hitInfo.collider.gameObject);

                JengaBlock jengaBlock = hitInfo.collider.GetComponent<JengaBlock>();

                if (jengaBlock == null)
                    return;

                StackData stackData = jengaBlock.Data;

                ShowData(stackData);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideData();
        }
    }

    private void ShowData(StackData data)
    {
        _uiJengaManager.ShowDataInfo(data.grade, data.domain, data.cluster, data.standardid, data.standarddescription);
        DisableCameraControl();
    }

    private void HideData()
    {
        _uiJengaManager.HideAll();
        EnableCameraControl();
    }

    private void EnableCameraControl()
    {
        _cameraControl.enabled = true;
    }

    private void DisableCameraControl()
    {
        _cameraControl.enabled = false;
    }

    private void ChangeStack()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchStackFocus(ORDER.PREVIOUS);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchStackFocus(ORDER.NEXT);
        }
    }


    private void SpawnJengaStack(SpawnPoint spawnPoint, ref int firstIndex)
    {
        Transform spTransform = spawnPoint.transform;

        int i = firstIndex;
        int copyFirstIndex = firstIndex;

        bool sameGrade = true;
        List<StackData> stackDataList = APICall.StackDataList;

        string stackGrade = stackDataList[firstIndex].grade;
        spawnPoint.SetText(stackGrade);
        List<StackData> currentStackDataList = new List<StackData>();

        // get stackdata that we will use
        while (sameGrade) { 
            if (stackDataList[i].grade != stackGrade) {
                sameGrade = false;
                break;
            }

            currentStackDataList.Add(stackDataList[i]);
            i++;
        }

        // save firstIndex for next spawn
        firstIndex = i;

        // sort currentStackData
        StackDataComparer stackDataComparer = new StackDataComparer();
        currentStackDataList.Sort(stackDataComparer);

        // Instantiate stackData as JengaBlocks

        i = 0;
        int j = 0;
        bool flipRot = true;
        int blocksPerCol = 3;
        float spaceBtwBlocks = 1.25f;

        JengaBlock[] jengaBlocksInStack = new JengaBlock[currentStackDataList.Count];

        while (i < currentStackDataList.Count) {

            float currentHeight = i / blocksPerCol;

            float posX = flipRot ? spTransform.position.x : spTransform.position.x + (j * spaceBtwBlocks) - spaceBtwBlocks;
            float posY = spTransform.position.y + (currentHeight * _jengaBlock.BlockHeight);
            float posZ = !flipRot ? spTransform.position.z : spTransform.position.z + (j * spaceBtwBlocks) - spaceBtwBlocks;

            Vector3 pos = new Vector3(posX, posY, posZ);
            Quaternion rot = flipRot ? Quaternion.identity : Quaternion.Euler(0, 90, 0);

            JengaBlock jengaBlock = Instantiate(_jengaBlock, pos, rot);
            jengaBlock.transform.SetParent(spTransform);  // to order the Unity hierarchy

            jengaBlock.Init(stackDataList[i+copyFirstIndex]);

            // add jengablock to the array
            jengaBlocksInStack[i] = jengaBlock;

            i++;
            j++;
            if (j >= blocksPerCol) {
                j = 0;
                flipRot = !flipRot;
            }
        }

        _stacksJengaBlocks.Add(jengaBlocksInStack);
        _stacksTested.Add(false);
    }

    private void SwitchStackFocus(ORDER order)
    {
        _focusIndex += (int)order;

        if (_focusIndex < 0)
            _focusIndex = _spawnPoints.Length - 1;
        if (_focusIndex >= _spawnPoints.Length)
            _focusIndex = 0;

        _cameraControl.SetTarget(_spawnPoints[_focusIndex].Center);
    }
}
