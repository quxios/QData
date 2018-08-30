Made with Unity 2018.x

# About
This is a small collection of components to help save/load data between scenes and to a save file.

# How to use
## DataManager
Create a `GameObject` and add the `DataManager` component to it. This component is a singleton and there should only be 1 per scene. The component will also mark the GameObject as "DontDestroyOnLoad"

**Important** You need to use the `DataManager`s scene changing function in order to save the states or merge your scene changer into it / merge the data managers into your scene changing function.

To change a scene use:
~~~c#
DataManager.current.ChangeScene(string sceneName, bool dontSave);
~~~

## StateSavers
To save data from a GameObject add the `StateSaver` component. Set the UID to a unique ID that will be used for the location this data is stored. Next you need to check which components you want to save. By default only `bool`, `string`, `int`, and `float` values will be stored. If you need to store other types you will need to create a component that uses the interface `IStateSaver`.

## Saving/Loading
To save the data you need to use the function:
~~~c#
DataManager.current.Save(int SAVEINDEX);
~~~
This will save all save states to a save file. Save folder is located in [Application.persistentDataPath](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html) 

To load a save file:
~~~c#
DataManager.current.Load(int SAVEINDEX, bool goToLoadedScene);
~~~
This will load the save file with that index. If the bool is true it will change scenes to the scene that save file was last at.

## Clear data
If you need to clear all the save states (Ex, on a New Game) use the function:
~~~c#
DataManager.current.InitNewGame();
~~~

# Support
[Patreon](https://www.patreon.com/quxios)

[Ko-fi](https://ko-fi.com/quxios)