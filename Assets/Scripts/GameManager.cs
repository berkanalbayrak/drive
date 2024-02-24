using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectDrive.EventBus;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public void Start()
    {
        GameStartTestAsync().Forget();
    }
    
    private async UniTaskVoid GameStartTestAsync()
    {
        await UniTask.Delay(2000);
        EventBus<StartCountdownEvent>.Raise(new StartCountdownEvent());
    }
}
