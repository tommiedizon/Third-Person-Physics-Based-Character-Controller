using UnityEngine;

public class SimpleStairs : MonoBehaviour {

    [SerializeField] private int numStairs;
    [SerializeField] private float stairsHeight = 0.5f;
    [SerializeField] private float stairsWidth = 2f;
    [SerializeField] private float stairsLength = 1f;
    [SerializeField] private GameObject step;
    [SerializeField] private Transform initialStepPos;

    private void Start() {
        step.transform.localScale = new Vector3(stairsLength, stairsHeight, stairsWidth);

        Vector3 stairPos = new Vector3(initialStepPos.position.x, 0.5f*stairsHeight, initialStepPos.position.z);

        // spawn the number of steps needed
        Color stairColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        for (int i = 0; i < numStairs; i++) {
            GameObject newStep = Instantiate(step, stairPos, Quaternion.identity, transform);
            newStep.GetComponent<Renderer>().material.color = stairColor;
            stairPos.z += stairsWidth;
            stairPos.y += stairsHeight;
        }
    }

}
