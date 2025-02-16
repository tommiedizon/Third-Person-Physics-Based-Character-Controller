using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using static UnityEditor.AssetDatabase;
using static System.IO.Directory;
using static System.IO.Path;
// using static directive  allows you to import all STATIC members from a class so
// you don't need to specify the class name when calling them

public static class Setup {

    [MenuItem("Tools/Setup/Create Default Folders")]
    // adds a custom command to the unity editor menu

    public static void CreateDefaultFolders() {

        Folders.CreateDefault(
            rootFolder: "_Project",
            folders: new string[] { "_Scripts", "Materials", "Scenes", "Prefabs",
            "Textures", "Animation", "Sounds", "Settings", "ScriptableObjects" }
        );

        Refresh(); // Do this so Unity recognises added directories.
    }

    static class Folders {
        public static void CreateDefault(string rootFolder, params string[] folders) { // params => variable args

            // Application.dataPath returns path to Assets folder
            // Path.Combine constructs a FULL PATH by joining the Assets folder with rootFolder
            var fullpath = Combine(Application.dataPath, rootFolder);

            foreach (var folder in folders) {
                var path = Combine(fullpath, folder);
                if (!Exists(path)) {
                    CreateDirectory(path);
                }
            }
        }

    }
}