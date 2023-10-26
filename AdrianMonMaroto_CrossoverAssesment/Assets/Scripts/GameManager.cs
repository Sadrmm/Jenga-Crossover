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

    [Header("Level")]
    [SerializeField] SpawnPoint[] _spawnPoints;
    [SerializeField] CameraControl _cameraControl;
    
    [Min(0)]
    [SerializeField] private int _spawnIndex;

    IEnumerator Start()
    {
        yield return StartCoroutine(APICall.GetRequest());

        int firstIndex = 0;
        SpawnJengaStack(_spawnPoints[0], ref firstIndex);
        SpawnJengaStack(_spawnPoints[1], ref firstIndex);
        SpawnJengaStack(_spawnPoints[2], ref firstIndex);
        
        // just in case the designer put an incorrect index, we put the latest
        if (_spawnIndex > _spawnPoints.Length) {
            _spawnIndex = _spawnPoints.Length - 1;
        }
        SwitchStackFocus(ORDER.SAME);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            SwitchStackFocus(ORDER.PREVIOUS);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
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

            i++;
            j++;
            if (j >= blocksPerCol) {
                j = 0;
                flipRot = !flipRot;
            }
        }
    }

    private void SwitchStackFocus(ORDER order)
    {
        _spawnIndex += (int)order;

        if (_spawnIndex < 0)
            _spawnIndex = _spawnPoints.Length - 1;
        if (_spawnIndex >= _spawnPoints.Length)
            _spawnIndex = 0;

        _cameraControl.SetTarget(_spawnPoints[_spawnIndex].Center);
    }
}
