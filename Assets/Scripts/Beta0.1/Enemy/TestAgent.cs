using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.EventSystems;

public class TestAgent : Agent
{
    public Transform target;
    public RayPerceptionSensorComponent3D enemyVision;

    [SerializeField] private Material succeed;
    [SerializeField] private Material fail;
    [SerializeField] private MeshRenderer floor;

    [SerializeField] private Rigidbody rb;
    private Vector3 moveToDirection;
    private float moveX;
    private float moveY;
    private float moveZ;
    private float moveSpeed = 8f;

    public override void OnEpisodeBegin()
    {
        enemyVision = transform.GetComponent<RayPerceptionSensorComponent3D>();
        transform.localPosition = new Vector3(Random.Range(-20f, 20f), 1.5f, Random.Range(-20f, 20f));
        target.localPosition = new Vector3(Random.Range(-20f, 20f), 1.5f, Random.Range(-20f, 20f));
        transform.localRotation = Quaternion.Euler(0, Random.Range(0,360), 0);
    }

    //Observe
    public override void CollectObservations(VectorSensor sensor)
    {
        //Observe it's position
        sensor.AddObservation(transform.localPosition);
        //Observe target position
        sensor.AddObservation(target.localPosition);
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
        moveToDirection = transform.forward * z + transform.right * x;
        transform.localPosition += moveToDirection * Time.deltaTime * moveSpeed;
    }

    private void Turn(float y)
    {
        transform.Rotate(0, y, 0);
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
                    Debug.Log("Found Player");
                    if (rayHitDistance <= 30f)
                    {
                        Debug.Log("In shooting range");
                        floor.material = succeed;
                        AddReward(100);
                        EndEpisode();
                    }
                }
                if (goHit.TryGetComponent<Wall>(out Wall wall))
                {
                    floor.material = fail;
                    if(rayHitDistance <= 5f)
                    {
                        AddReward(-50f);
                        EndEpisode();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            floor.material = fail;
            AddReward(-50f);
            EndEpisode();        
        }
    }
}
