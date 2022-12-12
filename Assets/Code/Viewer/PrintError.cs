using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorDisplay : MonoBehaviour
{

    private Text errMsg;
    private string logs;

    private void Start()
    {
        errMsg = GetComponent<Text>();
    }

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
        if (type != LogType.Error) return;
        logs += logString + "\n";
        errMsg.text = logs;
    }
    

}

