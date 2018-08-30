using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QData;

public class SaveTransform : MonoBehaviour, IStateSaver {
  [SerializeField] private bool savePosition;
  [SerializeField] private bool saveRotation;
  [SerializeField] private bool saveScale;

  public void OnLoad(object data) {
    // vectors and quaternion were saved as JSON
    // so need to convert them back
    var realData = (Dictionary<string, string>)data;
    if (savePosition && realData.ContainsKey("position")) {
      transform.position = JsonUtility.FromJson<Vector3>(realData["position"]);
    }
    if (saveRotation && realData.ContainsKey("rotation")) {
      transform.rotation = JsonUtility.FromJson<Quaternion>(realData["rotation"]);
    }
    if (saveScale && realData.ContainsKey("scale")) {
      transform.localScale = JsonUtility.FromJson<Vector3>(realData["scale"]);
    }
  }

  public object OnSave() {
    // saving the vectors and quaternions by making them into JSON
    var data = new Dictionary<string, string>();
    if (savePosition) {
      data["position"] = JsonUtility.ToJson(transform.position);
    }
    if (saveRotation) {
      data["rotation"] = JsonUtility.ToJson(transform.rotation);
    }
    if (saveScale) {
      data["scale"] = JsonUtility.ToJson(transform.localScale);
    }
    return data;
  }
}