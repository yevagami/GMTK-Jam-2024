using UnityEngine;

public class BasicActivationScript : TriggerScriptInterface {
    public override void Activate() {
        Debug.Log("Activated");
    }
    public override void Deactivate() {
        Debug.Log("Deactivated");
    }
}
