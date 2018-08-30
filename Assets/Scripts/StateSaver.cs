using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Permissions;
using UnityEngine;

namespace QData {
  public class StateSaver : MonoBehaviour {

    [SerializeField] private string[] rememberComponents;
    [SerializeField] private string _UID;

    private BindingFlags bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    public string UID() {
      return _UID;
    }

    public Dictionary<string, object> Save() {
      var test = new Dictionary<string, object>();
      foreach (string componentName in rememberComponents) {
        Component component = GetComponent(componentName);
        if (component != null) {
          IStateSaver saver = component as IStateSaver;
          if (saver != null) {
            test[componentName] = saver.OnSave();
          } else {
            test[componentName] = AutoData(component);
          }
        }
      }
      return test;
    }

    public void Load(Dictionary<string, object> data) {
      foreach (string componentName in rememberComponents) {
        Component component = GetComponent(componentName);
        if (component != null) {
          IStateSaver saver = component as IStateSaver;
          if (saver != null) {
            saver.OnLoad(data[componentName]);
          } else {
            AutoLoad(component, data[componentName]);
          }
        }
      }
    }

    private Dictionary<string, object> AutoData(Component component) {
      var output = new Dictionary<string, object>();
      Debug.Log(component.GetType());

      var fields = component.GetType().GetFields(bindings);
      foreach (var field in fields) {
        if (ValidType(field.FieldType)) {
          output[field.Name] = field.GetValue(component);
          Debug.Log("storing field " + field.Name);
        }
      }

      var props = component.GetType().GetProperties(bindings);
      foreach (var prop in props) {
        if (prop.Name == "tag" || prop.Name == "name") {
          continue;
        }
        if (prop.CanRead && prop.CanWrite && ValidType(prop.PropertyType)) {
          output[prop.Name] = prop.GetValue(component, null);
          Debug.Log("storing field " + prop.Name);
        }
      }
      return output;
    }

    private void AutoLoad(Component component, object data) {
      var dict = (Dictionary<string, object>)data;
      System.Type type = component.GetType();
      foreach (var item in dict) {
        var field = type.GetField(item.Key, bindings);
        if (field != null) {
          field.SetValue(component, item.Value);
        } else {
          var prop = type.GetProperty(item.Key, bindings);
          if (prop != null && prop.CanWrite) {
            prop.SetValue(component, item.Value, null);
          }
        }
      }
    }

    private bool ValidType(System.Type type) {
      return type == typeof(bool) || type == typeof(string) ||
        type == typeof(int) || type == typeof(float);
    }
  }
}
