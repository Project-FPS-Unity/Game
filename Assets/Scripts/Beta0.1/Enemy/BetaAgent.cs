using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.EventSystems;

public class BetaAgent : Agent
{
    public Transform target;

    [SerializeField] public RayPerceptionSensorComponent3D frontRay;
    [SerializeField] public RayPerceptionSensorComponent3D sideRay;

    private Vector3 moveToDirection;
    private float moveX;
    private float moveY;
    private float moveZ;
    private float moveSpeed = 8f;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-15f, 15f), 1.5f, Random.Range(-15f, 15f));
        target.localPosition = new Vector3(Random.Range(-15f, 15f), 1.5f, Random.Range(-15f, 15f));
        transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(target.transform.localPosition - transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        moveX = actions.ContinuousActions[0];
        moveY = actions.ContinuousActions[1];
        moveZ = actions.ContinuousActions[2];
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contact = actionsOut.ContinuousActions;
        contact[0] = Input.GetAxisRaw("Horizontal");
        contact[1] = Input.GetAxisRaw("Mouse X");
        contact[2] = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        CheckRay();
        Move(moveX, moveZ);
        Turn(moveY);
    }

    //---------------------------------------------------------------------//

    private void GetReward(int number)
    {
        switch (number)
        {
            case 0: //Hitwall
                AddReward(-200f);
                break;
            case 1: //Found Player
                AddReward(+150f);
                break;
            default:
                // code block
                break;
        }
    }

    private void Move(float x, float z)
    {
        moveToDirection = transform.forward * z + transform.right * x;
        transform.localPosition += moveToDirection * Time.deltaTime * moveSpeed;
    }

    private void Turn(float y)
    {
        transform.Rotate(0, y * moveSpeed, 0);
    }

    private void CheckRay()
    {
        //Front vision
        RayPerceptionOutput frontOut = RayPerceptionSensor.Perceive(frontRay.GetRayPerceptionInput());
        int rayFrontLength = frontOut.RayOutputs.Length;
        for (int i = 0; i < rayFrontLength; i++)
        {
            GameObject goHit = frontOut.RayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var rayDirection = frontOut.RayOutputs[i].EndPositionWorld - frontOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = rayDirection.magnitude;
                float rayHitDistance = frontOut.RayOutputs[i].HitFraction * scaledRayLength;

                if (goHit.TryGetComponent<FPS>(out FPS fps))
                {
                    GetReward(1);
                    EndEpisode();
                }
            }
        }

        //Side hit box
        RayPerceptionOutput sideOut = RayPerceptionSensor.Perceive(sideRay.GetRayPerceptionInput());
        int raySideLength = sideOut.RayOutputs.Length;
        for (int i = 0; i < raySideLength; i++)
        {
            GameObject goHit = sideOut.RayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var rayDirection = sideOut.RayOutputs[i].EndPositionWorld - sideOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = rayDirection.magnitude;
                float rayHitDistance = sideOut.RayOutputs[i].HitFraction * scaledRayLength;

                if (goHit.TryGetComponent<Wall>(out Wall wall))
                {
                    GetReward(0);
                    EndEpisode();
                }
            }
        }
    }
}
