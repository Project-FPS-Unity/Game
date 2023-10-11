using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class TestAgent : Agent
{
    [SerializeField] public Transform target;
    private float moveSpeed = 7.5f;
    private float rotateDirection = 1;

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(-10,10), 1.5f, UnityEngine.Random.Range(-10, 10));
        target.position = new Vector3(UnityEngine.Random.Range(-10, 10), 1.5f, UnityEngine.Random.Range(-10, 10));
        transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0,360), 0);
        int randomNum = UnityEngine.Random.Range(0, 2);
        if (randomNum == 0) rotateDirection = 1;
        else rotateDirection = -1;
    }

    //Observe
    public override void CollectObservations(VectorSensor sensor)
    {
        //Observe it's position
        sensor.AddObservation(transform.position);
        //Observe target position
        sensor.AddObservation(target.position);
    }

    //Receive action
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float moveZ = actions.ContinuousActions[2];
        //Move(moveX, moveZ);
        Turn(Math.Abs(moveY));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contact = actionsOut.ContinuousActions;
        contact[0] = Input.GetAxisRaw("Horizontal");
        contact[1] = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10f, Color.yellow);
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.gameObject.TryGetComponent<FPS>(out FPS fps))
            {
                Debug.Log("Hit Player!");
                AddReward(+50f);
                EndEpisode();
            }
            else
            {
                AddReward(-75);
                //EndEpisode();
            }
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //Hit wall
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-75f);
            EndEpisode();
        }
    }

    private void Move(float x, float z)
    {
        transform.position += new Vector3(x, 0, z) * Time.deltaTime * moveSpeed;
    }

    private void Turn(float y)
    {
        transform.Rotate(0, y * rotateDirection, 0);
    }
}
