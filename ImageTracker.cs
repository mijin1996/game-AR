using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ImageTracker : MonoBehaviour{
    ARTrackedImageManager manager;
    [SerializeField] List<string> name;
    [SerializeField] List<GameObject> animal;

    Dictionary<string, GameObject> dictPrefab = new();
    Dictionary<string, GameObject> dictSpawn = new();

  
    void Awake(){
        manager = FindFirstObjectByType<ARTrackedImageManager>();

        for(int i=0; i<name.Count; i++){ 
            dictPrefab[name[i]] = animal[i];
        }
    }

    //스캔한 이미지의 0, 0, 0위치 에서 스폰해라
    //ㄴ> Instantiate때문에 부모 위치를 기준으로 캐릭터 생성하라는 의미
    void SpawnCharacter(ARTrackedImage img){
        string name = img.referenceImage.name;
        if (!dictPrefab.ContainsKey(name)) return;

        var go = Instantiate(dictPrefab[name], img.transform);
        go.transform.localPosition = Vector3.zero;

        dictSpawn[name] = go;
    }

    // 설정한 이름을 찾아서 불러오도록 
    void UpdateCharacter(ARTrackedImage img){
        string name = img.referenceImage.name;

        // 아직 생성된 동물이 없으면
        if (!dictSpawn.ContainsKey(name)) return;

        bool active = img.trackingState == TrackingState.Tracking;
        dictSpawn[name].SetActive(active);
    }

    void HideCharacter(ARTrackedImage img){
        string name = img.referenceImage.name;
        if (dictSpawn.ContainsKey(name))
        {
            dictSpawn[name].SetActive(false);
        }
    }

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args){
        foreach (var img in args.added)
            SpawnCharacter(img);

        foreach (var img in args.updated)
            UpdateCharacter(img);

        foreach (var img in args.removed)
            HideCharacter(img.Value);

    }

    // 이 스크립트가 켜질 때 (자동 실행)
    // "이미지가 바뀌면 OnChanged 불러 줘" 신청
    void OnEnable(){
        manager.trackablesChanged.AddListener(OnChanged);
    }
    // 이 스크립트가 꺼질 때 (자동 실행)
    // 그 신청 취소
    void OnDisable(){
        manager.trackablesChanged.RemoveListener(OnChanged);
    }
}



/*
foreach : 리스트 안에 있는걸 모두 다 넣어서, 하나씩 실행, 다시 전부 꺼내라
AddListener : 
RemoveListener :

Dictionary: 이름표(키)로 값을 찾는 목록 = 이름으로 찾음
List: 번호로 찾음 
ContainsKey: 그 이름표가 사전에 있는지 확인 -> 있으면 true, 없으면 false를 돌려줘

*/
