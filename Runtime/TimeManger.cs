using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public enum Timeing
{
    Morning,Afternoon,Night
}
/// <summary>
/// 用于管理游戏时间的系统，将一天时间分为三段
/// 创建者： 黎凡
/// </summary>
//[ExecuteInEditMode]
public class TimeManger : MonoBehaviour
{
    public static TimeManger Instance;

    public List<Timeing> timeSequence;

    [PropertySpace]
    [Tooltip("当前的时间(上午、下午等)")]
    [Header("当前的时间(上午、下午等)")]
    [ShowInInspector]
    [ReadOnly]
    private Timeing now = Timeing.Morning;

    [PropertySpace]
    [ReadOnly]
    [ShowInInspector]
    [Header("总共经过天数（注意经过3天的话现在就是第4天）")]
    [Tooltip("总共经过天数（注意经过3天的话现在就是第4天）")]
    private int totalDay;

    [Header("测试用修改时间按钮")]
    [ValidateInput("CheckIncrement","不能回退过多时间",InfoMessageType.Error)]
    [PropertySpace]
    [InlineButton("UpdateTime")]
    public int amount;

    [Tooltip("当前时间在时间序列中的下标")]
    private int nowIndex = 0;

    [PropertySpace]
    [Header("时间发生变化时调用的函数")]
    [Tooltip("时间发生变化时调用的函数")]
    public UnityEvent onTimeChangedEvent=new UnityEvent();


    public void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Start()
    {
        timeSequence = new List<Timeing>();
        foreach(Timeing current in Enum.GetValues(typeof(Timeing)))
        {
            timeSequence.Add(current);
        }

    }


    public Timeing Now()
    {
        return now;
    }

    public int TotalDay()
    {
        return totalDay;
    }

    /// <summary>
    /// 此处参数为增量，即过了几个时间单位（例如经过了上午和下午就写2）
    /// </summary>
    /// <param name="increment"></param>
    public void UpdateTime(int increment)
    {
        if (nowIndex + increment >= 0)
        {
            nowIndex += increment;
            totalDay = (int)Mathf.Floor((nowIndex / (float)timeSequence.Count));
            now = timeSequence[nowIndex % timeSequence.Count];
            onTimeChangedEvent.Invoke();
        }
        else
        {
            Debug.LogWarning("回退了过多时间！");
        }
    }

    /// <summary>
    /// 用于检测测试输入是否正确
    /// </summary>
    /// <param name="tempInt"></param>
    /// <returns></returns>
    private bool CheckIncrement(int tempInt)
    {
        return nowIndex + tempInt >= 0;
    }

}
