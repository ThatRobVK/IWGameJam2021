using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepGenerator : MonoBehaviour
{
    [SerializeField] private GameObject stepPrefab;
    [SerializeField] private int stepsPerFlight;
    [SerializeField] private float stairDepth;
    

    private void Start()
    {
        Vector3 pyramidAnchorPosition = transform.position;
        //float stairDepth = 0.052f;
        float stairHeight = 0.1f;

        List<Vector3> staircaseAnchorPositions = new List<Vector3>();
        staircaseAnchorPositions.Add(new Vector3(0, -0.4f, -5.369f));
        staircaseAnchorPositions.Add(new Vector3(0, -0.4f, 5.369f));
        staircaseAnchorPositions.Add(new Vector3(-5.369f, -0.4f, 0));
        staircaseAnchorPositions.Add(new Vector3(5.369f, -0.4f, 0));

        List<Vector3> staircaseStepDistance = new List<Vector3>();
        staircaseStepDistance.Add(new Vector3(0, stairHeight, stairDepth));
        staircaseStepDistance.Add(new Vector3(0, stairHeight, -stairDepth));
        staircaseStepDistance.Add(new Vector3(stairDepth, stairHeight, 0));
        staircaseStepDistance.Add(new Vector3(-stairDepth, stairHeight, 0));

        List<Vector3> staircaseStepRotations = new List<Vector3>();
        staircaseStepRotations.Add(new Vector3(0, 0, 0));
        staircaseStepRotations.Add(new Vector3(0, 00, 0));
        staircaseStepRotations.Add(new Vector3(0, 90, 0));
        staircaseStepRotations.Add(new Vector3(0, 90, 0));

        for (int i = 0; i < 4; i++)
        {
            GenerateStaircase(pyramidAnchorPosition, staircaseAnchorPositions[i], staircaseStepDistance[i], staircaseStepRotations[i]);
        }
    }


    private void GenerateStaircase(Vector3 pyramidAnchorPosition, Vector3 staircaseAnchorPosition, Vector3 staircaseStepDistance, Vector3 staircaseStepRotation)
    {
        Vector3 stepAnchor = pyramidAnchorPosition + staircaseAnchorPosition;
        
        for (int i = 0; i < stepsPerFlight; i++)
        {
            Vector3 stepPosition = stepAnchor + (i * staircaseStepDistance);
            Instantiate(stepPrefab, stepPosition, Quaternion.Euler(staircaseStepRotation));
        }
    }
}
