using UnityEngine;
using UnityEngine.Events;

public enum LoopType{
    repeat,
    pingPong
}

[System.Serializable]
public class DriverSettings{
    public LoopSettings Loop;
    public WaitSettings Wait;
    public EventSettings Events;
}

[System.Serializable]
public class LoopSettings{
    public LoopType loopType;
    public int loopCount = 1;
    public bool autoRepeat = false;
}

[System.Serializable]
public class WaitSettings{
    public float wait;
    public float maxWait;

    public float CalcutateRandomValueTime(float startWaitTime, float maxWaitTime){
        if(maxWaitTime > startWaitTime) return Random.Range(startWaitTime, maxWaitTime);
        else return startWaitTime;
    }
}

[System.Serializable]
public class EventSettings{
    public UnityEvent startEvent;
    public UnityEvent endEvent;
}