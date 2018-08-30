using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QData {
  public interface IStateSaver {
    object OnSave();
    void OnLoad(object data);
  }
}