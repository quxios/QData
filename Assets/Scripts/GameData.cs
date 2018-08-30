using System.Collections;
using System.Collections.Generic;

namespace QData {
  [System.Serializable]
  public class GameData {
    public string currentScene;

    public Dictionary<string, Dictionary<string, object>> saveStates = new Dictionary<string, Dictionary<string, object>>();
  }
}