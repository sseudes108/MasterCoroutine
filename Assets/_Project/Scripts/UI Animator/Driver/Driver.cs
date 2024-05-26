using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Driver : MonoBehaviour{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _progress;
    [SerializeField] private bool _playForward = true;
    [SerializeField] private bool _autoStart = false;
    [SerializeField] private bool _preStartOnEnable = false;
    [SerializeField] private bool _resetWhenFinish = false;
    [SerializeField] private DriverSettings _settings;
    private IEnumerator _routine;

    [SerializeField] private List<Controller> _controllers = new();

    public void AutoFillControllers(){
        _controllers = GetComponents<Controller>().ToList();
    }

    private void OnEnable() {
        if(_preStartOnEnable) ResetProgress(_playForward);
        if(_autoStart) Run(true, _progress);
    }
    
    private void ResetProgress(bool toStart){
        float result = 0;

        if(toStart){
            result = 0;
            SetProgress(0);
        }else{
            result = 1f;
            SetProgress(_settings.Loop.loopCount * _duration);
        }

        UpdateControllers(result);
    }

    private void SetProgress(float progress){
        _progress = progress;
    }


    public void RunForward(){
        Run(true, _progress);
    }

    public void RunBackward(){
        Run(false, _progress);
    }
    
    public void Run(bool forward, float progress){
        _progress = progress;
        _playForward = forward;
        Stop();
        _routine = DriverRoutine();
        StartCoroutine(_routine);
    }

    public void Stop(){
        if (_routine != null) StopCoroutine(_routine);
    }

    private float CalculateProgress(float inTime, float duration){
        if(inTime < 0) inTime = 0;
        if(inTime > duration) inTime = duration;

        return inTime;
    }

    private IEnumerator DriverRoutine(){
        bool running = true;

        while (running){

            float waitResult = _settings.Wait.wait;
            if(_settings.Wait.wait != 0 || _settings.Wait.maxWait != 0) waitResult = _settings.Wait.CalcutateRandomValueTime(_settings.Wait.wait, _settings.Wait.maxWait);
            if(waitResult != 0) yield return new WaitForSeconds(waitResult);

            float elapsedTime = 0f;
            float wholeDuration = _settings.Loop.loopCount * _duration;
            float result = 0;
            bool eventToTrigger = false;

            if(_playForward == false) elapsedTime = wholeDuration;

            elapsedTime = _progress;

            while(0 <= elapsedTime && elapsedTime <= wholeDuration){

                if(_playForward) elapsedTime += Time.deltaTime;
                else elapsedTime -= Time.deltaTime;

                _progress = CalculateProgress(elapsedTime, wholeDuration);

                if(eventToTrigger == false){
                    if(_progress != 0 && _progress != wholeDuration){
                        eventToTrigger = true;
                        _settings.Events.startEvent?.Invoke();
                    }
                }
                
                if(_settings.Loop.loopType == LoopType.repeat) result = Mathf.Repeat(_progress / _duration, 1.000001f);
                if(_settings.Loop.loopType == LoopType.pingPong) result = Mathf.PingPong(_progress / _duration, 1.000001f);

                UpdateControllers(result);

                yield return null;
            }

            running = _settings.Loop.autoRepeat;
            if(_settings.Loop.autoRepeat || _resetWhenFinish){
                if(_playForward){
                    _progress = 0f;
                   UpdateControllers(0);
                }else{
                    _progress = wholeDuration;
                    UpdateControllers(1f);
                }
            }

            if(eventToTrigger == true){
                _settings.Events.endEvent?.Invoke();
            }
        }
    }

    public void UpdateControllers(float result){
        foreach (var controller in _controllers){
            if(controller == null) break;
            if(controller.gameObject.activeSelf == true){
                controller.SetValues(result);
            }
        }
    }
}
