    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BeaconBehaviour : MonoBehaviour
    {
        private List<bool[]> memorizedInput=new List<bool[]>();
        private bool hasEcho=false;
        // Start is called before the first frame update
        void Start()
        {        
        }

        public void SwitchShadow(Vector3 nearBeaconPosition)
        {
            // 尝试加载 Shadow prefab
            GameObject shadowPrefab = Resources.Load<GameObject>("Prefab/Shadow");
            if (shadowPrefab == null)
            {
                Debug.LogError("Failed to load Shadow prefab from Resources/Prefab/Shadow");
                return;
            }

            // 实例化 Shadow
            GameObject shadow = Instantiate(shadowPrefab, nearBeaconPosition, Quaternion.identity);
            shadow.GetComponent<ShadowBehaviour>().setBeaconBehaviour(this);
            if (shadow == null)
            {
                Debug.LogError("Failed to instantiate Shadow prefab");
                return;
            }

            Debug.Log("Shadow instantiated successfully at: " + shadow.transform.position);

            // 获取主摄像机并设置目标
            GameObject mainCamera = GameObject.Find("Main Camera");
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not found in scene!");
                return;
            }

            CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
            if (cameraFollow == null)
            {
                Debug.LogError("CameraFollow component not found on Main Camera!");
                return;
            }

            mainCamera.transform.position = new Vector3(shadow.transform.position.x, shadow.transform.position.y, -10);
            cameraFollow.target = shadow.transform;

            Debug.Log("Camera target set to shadow");
        }

        public void SwitchPlayer(Vector3 nearBeaconPosition){
            //generate echo
            // 尝试加载 Echo prefab
            GameObject echoPrefab = Resources.Load<GameObject>("Prefab/Echo");
            if (echoPrefab == null)
            {
                Debug.LogError("Failed to load Echo prefab from Resources/Prefab/Echo");
                return;
            }

            // 实例化 Echo
            GameObject echo = Instantiate(echoPrefab, nearBeaconPosition, Quaternion.identity);
            echo.GetComponent<EchoBehaviour>().simulatedInputs=memorizedInput;
            echo.GetComponent<EchoBehaviour>().beaconBehaviour=this;
            hasEcho=true;
            if (echo == null)
            {
                Debug.LogError("Failed to instantiate Shadow prefab");
                return;
            }

            Debug.Log("Echo instantiated successfully at: " + echo.transform.position);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public List<bool[]> getInput(){
            return memorizedInput;
        }

        public bool HasEcho(){
            return hasEcho;
        }
        public void SetHasEcho(bool hasEcho){
            this.hasEcho=hasEcho;
        }
    }
