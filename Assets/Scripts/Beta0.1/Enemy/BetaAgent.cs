using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BetaAgent : Agent
{
    [SerializeField] private RayPerceptionSensorComponent3D frontRay;
    [SerializeField] private RayPerceptionSensorComponent3D sideRay;
    [SerializeField] private Transform faceDirection;

    [SerializeField] private Transform target;
    [SerializeField] private EnemyHeavy heavy;
    private bool isCombat = false;

    private Vector3 moveToDirection;
    private float moveX;
    private float moveY;
    private float moveZ;
    private float moveSpeed = 6f;
    private float aimSpeed = 4f;

    [SerializeField] private AnimationController animationState;

    private void Start()
    {
        target = GameObject.Find("Model").GetComponent<Transform>();
    }

    public override void OnEpisodeBegin()
    {
        PlayerHealth.health.SetHealth(PlayerHealth.health.GetMaxHealth());
        isCombat = false;
        //transform.localPosition = new Vector3(Random.Range(-20f, 20f), 1.5f, Random.Range(-20f, 20f));
        //target.localPosition = new Vector3(Random.Range(-20f, 20f), 1.5f, Random.Range(-20f, 20f));
        //transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Normalize(transform.localPosition));
        sensor.AddObservation(Quaternion.Normalize(transform.localRotation));

        sensor.AddObservation(Vector3.Normalize(target.localPosition));
        //sensor.AddObservation(Vector3.Normalize(transform.forward * 30));
        //sensor.AddObservation(Quaternion.Normalize(new Quaternion(0, target.localRotation.y - transform.localRotation.y, 0, 0)));
        sensor.AddObservation((Vector3.Distance(transform.localPosition, target.localPosition)));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        moveX = actions.ContinuousActions[0];
        moveY = actions.ContinuousActions[1];
        moveZ = actions.ContinuousActions[2];

        
        CheckRay();

        if (!isCombat)
        {
            Turn(moveY);
            Move(moveZ);
        }
        else
        {
            //Aim(moveY);
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
            case 2: //Found Player
                AddReward(+0.2f);
                break;
            case 3: //Shoot
                AddReward(+0.5f);
                break;
            case 4: //Player Die
                AddReward(+0.8f);
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
        animationState.AnimationManager("Walk");
    }

    private void Turn(float y)
    {
        transform.Rotate(0, y * moveSpeed, 0);
    }

    private void Aim(float y)
    {
        animationState.AnimationManager("Aim");
        transform.Rotate(0, y * aimSpeed, 0);
    }

    private void Shoot()
    {
        animationState.AnimationManager("Shoot");
        heavy.ShootTrigger();
    }


    private void InCombat()
    {
        RaycastHit hit;
        Debug.DrawRay(faceDirection.position, faceDirection.forward * 40f, Color.green);
        if (Physics.Raycast(faceDirection.position, faceDirection.forward, out hit, 40f))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                if (PlayerHealth.health.GetCurrentHealth() <= 0)
                {
                    GetReward(4);
                    EndEpisode();
                }
                //Invoke(nameof(Shoot), 0.25f);
                Shoot();
            }
        }
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
            if (goHit != null)
            {
                var EnemyDirection = frontOut.RayOutputs[i].EndPositionWorld - frontOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = EnemyDirection.magnitude;
                float rayHitDistance = frontOut.RayOutputs[i].HitFraction * scaledRayLength;

                if (goHit.gameObject.tag == "Player")
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

                if (goHit.gameObject.tag == "Wall")
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
        if (!playerFound && isCombat)
        {
            isCombat = false;            
        }
    }
}
