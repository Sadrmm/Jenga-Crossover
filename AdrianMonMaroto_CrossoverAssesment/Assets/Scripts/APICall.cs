using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class APICall
{
    private const string API_URL = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

    public static List<StackData> StackDataList;

    public static IEnumerator GetRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(API_URL)) {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(($"Something went wrong: {0}", webRequest.error));
                    break;

                case UnityWebRequest.Result.Success:
                    string json = webRequest.downloadHandler.text;
                    //Debug.Log(json);

                    StackDataList = JsonConvert.DeserializeObject<List<StackData>>(json);
                    //foreach (StackData stackData in _stackDataList) {
                    //    Debug.Log(stackData.ToString());
                    //}

                    break;

                default:
                    Debug.LogError("Case not supported");
                    break;
            }
        }
    }
}
