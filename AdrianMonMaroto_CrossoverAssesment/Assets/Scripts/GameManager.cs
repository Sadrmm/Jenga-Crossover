using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] JengaBlock _jengaBlock;
    [SerializeField] GameObject _jengaRow;

    [SerializeField] SpawnPoint[] _spawnPoints;

    IEnumerator Start()
    {
        yield return StartCoroutine(APICall.GetRequest());

        int firstIndex = 0;
        SpawnJengaStack(_spawnPoints[0], ref firstIndex);
        SpawnJengaStack(_spawnPoints[1], ref firstIndex);
        SpawnJengaStack(_spawnPoints[2], ref firstIndex);
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
}
