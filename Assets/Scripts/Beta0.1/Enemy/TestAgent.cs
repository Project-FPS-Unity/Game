using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.Mathematics;

public class TestAgent : Agent
{
    public Transform target;

    [SerializeField] public RayPerceptionSensorComponent3D enemyVision;
    [SerializeField] public RayPerceptionSensorComponent3D sideVision;

    [SerializeField] private Rigidbody rb;
    private Vector3 moveToDirection;
    private float moveX;
    private float moveY;
    private float moveZ;
    private float moveSpeed = 8f;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(UnityEngine.Random.Range(-15f, 15f), 1.5f, UnityEngine.Random.Range(-15f, 15f));
        target.localPosition = new Vector3(UnityEngine.Random.Range(-15f, 15f), 1.5f, UnityEngine.Random.Range(-15f, 15f));
        transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0,360), 0);
    }

    //Observe
    public override void CollectObservations(VectorSensor sensor)
    {
        //Observe it's position
        sensor.AddObservation(transform.localPosition);
        //Observe target position
        sensor.AddObservation(target.localPosition);
        //Observe Ray
        RayPerceptionOutput rayOut = RayPerceptionSensor.Perceive(enemyVision.GetRayPerceptionInput());
        for (int i = 0; i < rayOut.RayOutputs.Length; i++)
        {
            sensor.AddObservation(rayOut.RayOutputs[i].HitGameObject);
        }
        RayPerceptionOutput raySideOut = RayPerceptionSensor.Perceive(sideVision.GetRayPerceptionInput());
        for (int i = 0; i < raySideOut.RayOutputs.Length; i++)
        {
            sensor.AddObservation(raySideOut.RayOutputs[i].HitGameObject);
        }
    }

    //Receive action
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

    void FixedUpdate()
    {
        CheckVision();
        Move(moveX, moveZ);
        Turn(moveY);
    }

    private void Move(float x, float z)
    {
        moveToDirection = transform.forward * math.abs(z) + transform.right * 0;
        transform.localPosition += moveToDirection * Time.deltaTime * moveSpeed;
    }

    private void Turn(float y)
    {
        transform.Rotate(0, y * moveSpeed, 0);
    }

    private void CheckVision()
    {
        RayPerceptionOutput rayOut = RayPerceptionSensor.Perceive(enemyVision.GetRayPerceptionInput());
        int rayLength = rayOut.RayOutputs.Length;
        for (int i = 0; i < rayLength; i++)
        {
            GameObject goHit = rayOut.RayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var rayDirection = rayOut.RayOutputs[i].EndPositionWorld - rayOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = rayDirection.magnitude;
                float rayHitDistance = rayOut.RayOutputs[i].HitFraction * scaledRayLength;

                if (goHit.TryGetComponent<FPS>(out FPS fps))
                {
                    //Debug.Log("In shooting range");
                    AddReward(100f);
                    EndEpisode();
                }
            }
        }
        RayPerceptionOutput raySideOut = RayPerceptionSensor.Perceive(sideVision.GetRayPerceptionInput());
        int raySideLength = raySideOut.RayOutputs.Length;
        for (int i = 0; i < raySideLength; i++)
        {
            GameObject goHit = raySideOut.RayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var rayDirection = raySideOut.RayOutputs[i].EndPositionWorld - raySideOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = rayDirection.magnitude;
                float rayHitDistance = raySideOut.RayOutputs[i].HitFraction * scaledRayLength;

                if (goHit.TryGetComponent<FPS>(out FPS fps))
                {
                    AddReward(+1.5f);
                }

                if (goHit.TryGetComponent<Wall>(out Wall wall))
                {
                    if (rayHitDistance <= 5f)
                    {
                        AddReward(-200f);
                        EndEpisode();
                    }
                }
            }
        }
    }
}
