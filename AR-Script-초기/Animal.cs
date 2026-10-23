using UnityEngine;
using UnityEngine.InputSystem; // 📌⭐ Unity에 있는 클래스 | 거기에 담긴 클래스 중 Touchscreen클래스를 불러오는 기능

public class Animal : MonoBehaviour{
    public LayerMask animalLayer;
    public AudioSource audioSource;
    public AudioClip crySound;

    void Update(){
        if (Touchscreen.current != null){

            // 첫 번째 터치 정보를 가져옴
            var touch = Touchscreen.current.primaryTouch;
            if(touch.press.wasPressedThisFrame){
                Vector2 touchPos = touch.position.ReadValue();

                Ray ray = Camera.main.ScreenPointToRay(touchPos);

                //터치해서 눌러졌을때 각각 나열되어서 Raycast에 포함되어서
                if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, animalLayer)){

                    // 터치한 오브젝트가 현재 동물 또는 동물의 자식인지 확인
                    if(hit.transform.IsChildOf(transform)){
                        if(crySound!=null && !audioSource.isPlaying){
                            audioSource.PlayOneShot(crySound);
                        }
                    }
                        
                }
            }
        }
    }
}

/*
📌⭐primaryTouch : 함수 x, property로 적용하여 사용되고 있는 상황

< RayCast >
보이지 않는 가상 광선을 쏴서 오브젝트를 감지하는 기능

AR Raycast:
    - 눌럿을때만 카메라에서 빛을 쏴서 해당 오브젝트를 감지
VR Raycast:
    - 컨트롤러에 있는 막대기 의미


ReadValue:
    - 현재 입력값을 읽어오는 기능
    ex. 터치한 화면의 좌표를 가져옴


PlayOneShot : 한 번만 실행하겠다
PlayShot :

*/

