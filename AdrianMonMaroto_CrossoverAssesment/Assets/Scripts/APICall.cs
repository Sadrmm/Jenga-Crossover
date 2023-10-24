using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APICall : MonoBehaviour
{
    private const string API_URL = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

    public class StackData
    {
        public int id { get; set; }
        public string subject { get; set; }
        public string grade { get; set; }
        public int mastery { get; set; }
        public string domainid { get; set; }
        public string domain { get; set; }
        public string cluster { get; set; }
        public string standardid { get; set; }
        public string standarddescription { get; set; }

        public override string ToString()
        {
            return $"Id: {id}, Subject: {subject}, Grade: {grade}, Mastery: {mastery}, DomainId: {domainid}," +
                $"Domain: {domain}, Cluster: {cluster}, StandardId: {standardid}, StandardDescription: {standarddescription}";
        }
    }

    private List<StackData> _stackDataList;

    void Start()
    {
        StartCoroutine(GetRequest(API_URL));
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(($"Something went wrong: {0}", webRequest.error));
                    break;

                case UnityWebRequest.Result.Success:
                    string json = webRequest.downloadHandler.text;
                    //Debug.Log(json);

                    _stackDataList = JsonConvert.DeserializeObject<List<StackData>>(json);
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
