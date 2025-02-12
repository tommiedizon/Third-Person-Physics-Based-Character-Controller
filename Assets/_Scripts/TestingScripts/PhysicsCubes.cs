using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class PhysicsCubes : MonoBehaviour {
    [SerializeField] List<GameObject> cubes;

    private void Start() {
        foreach(var cube in cubes) {
            float R = UnityEngine.Random.Range(0f, 1f);
            float G = UnityEngine.Random.Range(0f, 1f);
            float B = UnityEngine.Random.Range(0f, 1f);
            cube.GetComponent<Renderer>().material.color = new Color(R,G,B);
            cube.GetComponent<Rigidbody>().mass = 0.1f;
        }
    }
}
