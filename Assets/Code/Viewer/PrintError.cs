using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PrintError : MonoBehaviour
{

    public Text errMsg;
    private string logs;


    internal void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    internal void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="logString">错误信息</param>
    /// <param name="stackTrace">跟踪堆栈bai</param>
    /// <param name="type">错误类型</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
                break;
            case LogType.Assert:
                break;
            case LogType.Warning:
                return;
            case LogType.Log:
                return;
            case LogType.Exception:
                break;
            default:
                break;
        }

        logs += logString + "\n";
        errMsg.text = logs;
    }


}

