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
    [SerializeField] private PlayerScript player;
    private Vector3 moveToDirection;
    private Vector3 EnemyDirection;
    private float moveX;
    private float moveY;
    private float moveZ;
    private float moveSpeed = 8f;
    private bool isCombat = false;

    public override void OnEpisodeBegin()
    {       
        isCombat = false;
        transform.localPosition = new Vector3(Random.Range(-15f, 15f), 1.5f, Random.Range(-15f, 15f));
        target.localPosition = new Vector3(Random.Range(-15f, 15f), 1.5f, Random.Range(-15f, 15f));
        transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Normalize(transform.localPosition));
        sensor.AddObservation(Quaternion.Normalize(transform.localRotation));

        sensor.AddObservation(Vector3.Normalize(target.transform.localPosition));
        sensor.AddObservation(Vector3.Normalize(transform.forward * 30));
        sensor.AddObservation(Vector3.Normalize(transform.localPosition - target.transform.localPosition));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        moveX = actions.ContinuousActions[0];
        moveY = actions.ContinuousActions[1];
        moveZ = actions.ContinuousActions[2];

        Turn(moveY);
        CheckRay();

        if (!isCombat)
        {
            Move(moveZ);
        }
        else
        {
            InCombat();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contact = actionsOut.ContinuousActions;
        contact[0] = Input.GetAxisRaw("Horizontal");
        contact[1] = Input.GetAxisRaw("Mouse X");
        contact[2] = Input.GetAxisRaw("Vertical");
    }

    //---------------------------------------------------------------------//

    private void GetReward(int number)
    {
        switch (number)
        {
            case 0: //Walk backward or Struggle aim
                AddReward(-0.1f);
                break;
            case 1: //Hitwall
                AddReward(-0.5f);
                break;
            case 2: //Shoot
                AddReward(+0.5f);
                break;
            case 3: //Player Die
                AddReward(+0.75f);
                break;
            default:
                // code block
                break;
        }
    }

    private void Move(float z)
    {
        moveToDirection = transform.forward * z;
        if (z < 0) GetReward(0);
        transform.localPosition += moveToDirection * Time.deltaTime * moveSpeed;
    }

    private void Turn(float y)
    {
        transform.Rotate(0, y * moveSpeed, 0);
    }

    private void InCombat()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 30, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 35f))
        {
            if (hit.transform.gameObject.GetComponent<PlayerScript>())
            {
                var playerHealth = hit.transform.gameObject.GetComponent<PlayerScript>();
                playerHealth.TakeDamage(1);
                Shoot();
                if (playerHealth.CheckDead())
                {                    
                    GetReward(3);
                    Debug.Log("Player Defeated");
                    EndEpisode();
                }
            }
        }
    }

    private void Shoot()
    {                
        GetReward(2);
    }

    private void CheckRay()
    {
        //Front vision
        RayPerceptionOutput frontOut = RayPerceptionSensor.Perceive(frontRay.GetRayPerceptionInput());
        int rayFrontLength = frontOut.RayOutputs.Length;
        bool playerFound = false;
        for (int i = 0; i < rayFrontLength; i++)
        {
            GameObject goHit = frontOut.RayOutputs[i].HitGameObject;
            if (goHit == null && isCombat)
            {
                isCombat = false;
            }
            if (goHit != null)
            {
                var EnemyDirection = frontOut.RayOutputs[i].EndPositionWorld - frontOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = EnemyDirection.magnitude;
                float rayHitDistance = frontOut.RayOutputs[i].HitFraction * scaledRayLength;

                if (goHit.TryGetComponent<PlayerScript>(out PlayerScript playerScript))
                {
                    playerFound = true;
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
                    GetReward(1);
                    EndEpisode();
                }
            }
        }

        if (playerFound && !isCombat)
        {
            isCombat = true;
        }
    }
}
