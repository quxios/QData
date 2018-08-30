using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QData;

public class TestAutoSaver : MonoBehaviour {
  // public int, is saved
  public int myValue;
  // private vector, not saved, vectors aren't supported in autosave
  [SerializeField] private Vector3 myVector;
  // private string, is saved
  [SerializeField] private string myString;
}
