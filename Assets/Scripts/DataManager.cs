using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QData {
  public class DataManager : MonoBehaviour {
    private static DataManager _current;
    public static DataManager current {
      get {
        if (_current == null) {
          GameObject obj = new GameObject();
          obj.name = "DataManager";
          _current = obj.AddComponent<DataManager>();
        }
        return _current;
      }
    }

    public GameData data {
      get; private set;
    }

    private void Awake() {
      if (_current == null) {
        _current = this;
      } else if (_current != this) {
        Destroy(this);
        return;
      }
      DontDestroyOnLoad(this);
      Init();
    }

    private void Init() {
      InitNewGame();
      SetUpSaveFolder();
      SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ChangeScene(string sceneName, bool dontSave = false) {
      if (!dontSave) {
        SaveSaveStates();
      }
      SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      data.currentScene = scene.name;
      LoadSaveStates();
    }

    public void InitNewGame() {
      data = new GameData();
    }

    private void SetUpSaveFolder() {
      if (!Directory.Exists(SaveFolderPath())) {
        Directory.CreateDirectory(SaveFolderPath());
      }
    }

    public string SaveFolderPath() {
      return Application.persistentDataPath + "/Saves/";
    }

    public string SavePath(int saveIndex) {
      return SaveFolderPath() + "save" + saveIndex + ".sav";
    }

    public void Save(int saveIndex) {
      if (data.currentScene == null) {
        data.currentScene = data.currentScene = SceneManager.GetActiveScene().name;
      }
      SaveSaveStates();
      FileStream file = File.Create(SavePath(saveIndex));
      BinaryFormatter bf = new BinaryFormatter();
      bf.Serialize(file, data);
      file.Close();
    }

    public void Load(int saveIndex, bool goToLoadedScene = true) {
      if (File.Exists(SavePath(saveIndex))) {
        FileStream file = File.Open(SavePath(saveIndex), FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        data = (GameData)bf.Deserialize(file);
        file.Close();
        if (goToLoadedScene) {
          ChangeScene(data.currentScene, true);
        }
      }
    }

    public bool HasAnySaveFile() {
      var files = Directory.GetFiles(SaveFolderPath(), "*.sav", System.IO.SearchOption.TopDirectoryOnly);
      return files.Length > 0;
    }

    public void SaveSaveStates() {
      foreach (StateSaver saver in UnityEngine.Object.FindObjectsOfType<StateSaver>()) {
        data.saveStates[saver.UID()] = saver.Save();
      }
    }

    public void LoadSaveStates() {
      foreach (StateSaver saver in UnityEngine.Object.FindObjectsOfType<StateSaver>()) {
        if (data.saveStates.ContainsKey(saver.UID())) {
          saver.Load(data.saveStates[saver.UID()]);
        }
      }
    }
  }
}