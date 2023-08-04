using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{   
    //针对每一个static event 都需要及时在对应脚本中取消事件订阅或者用这样一个脚本来整理并取消事件订阅
    //以此来避免一个事件存在已经被销毁的物体的不合理的订阅
    private void Awake()
    {
        CuttingCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
    }
}
