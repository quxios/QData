using UnityEngine;
using QData;

public class GameUtils : MonoBehaviour {
  public void Save() {
    DataManager.current.Save(0);
  }

  public void Load() {
    DataManager.current.Load(0);
  }

  public void SceneChange(string name) {
    DataManager.current.ChangeScene(name);
  }
}