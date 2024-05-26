using UnityEngine;

[System.Serializable]
public class ControllerEditorPreview {
    public bool preview = false;

    [Range(0, 1)]
    public float previewResult;
    [HideInInspector]
    public bool IHaveInitialValue = false;
    [HideInInspector]
    public float InitialFloat;
    [HideInInspector]
    public Vector3 InitialVector3;
    public string initialString;

    public void PreviewMyValue(Controller myController){
        if(preview){
            if(!IHaveInitialValue){
                myController.GetInitialValue(this);
                IHaveInitialValue = true;
            }
            myController.SetValues(previewResult);
        }else{
            if(IHaveInitialValue){
                myController.SetInitialValue(this);
                IHaveInitialValue = false;
            }
        }
    }
}