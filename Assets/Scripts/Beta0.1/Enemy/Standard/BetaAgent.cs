using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Integrations.Match3;
using System.Collections;

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

    private PlayerHealth player;

    private void Awake()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    public override void OnEpisodeBegin()
    {
        //PlayerHealth.health.SetHealth(PlayerHealth.health.GetMaxHealth());
        isCombat = false;
        //transform.localPosition = new Vector3(Random.Range(-20f, 20f), 1.5f, Random.Range(-20f, 20f));
        //if (target != null) target.localPosition = new Vector3(Random.Range(-20f, 20f), 1.5f, Random.Range(-20f, 20f));
        //transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation.eulerAngles / 180.0f - Vector3.one);
        //sensor.AddObservation(target.localPosition);
        //sensor.AddObservation(Vector3.Distance(transform.localPosition, target.localPosition));
        if (target != null)
        {
            sensor.AddObservation(target.localPosition);
            sensor.AddObservation(Vector3.Distance(transform.localPosition, target.localPosition));
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        moveX = actions.ContinuousActions[0];
        moveY = actions.ContinuousActions[1];
        moveZ = actions.ContinuousActions[2];

        if (target == null)
        {
            target = transform;
        }

        CheckRay();

        if (!isCombat)
        {
            Turn(moveY, moveSpeed, false);//turn
            Move(moveZ);
        }
        else if (isCombat)
        {
            Turn(moveY, aimSpeed, true);//aim
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
                AddReward(-20.0f);
                break;
            case 2: //Found Player
                AddReward(+4.0f);
                break;
            case 3: //Shoot
                AddReward(+2.5f);
                break;
            case 4: //Player Die
                AddReward(+15.0f);
                break;
            default:
                // code block
                break;
        }
    }

    private void Move(float z)
    {
        animationState.AnimationManager("Walk");
        moveToDirection = transform.forward * z;
        if (z < 0) GetReward(0);
        transform.localPosition += moveToDirection * Time.deltaTime * moveSpeed;
    }

    private void Turn(float y, float speed, bool isAim)
    {
        if (isAim) animationState.AnimationManager("Aim");
        transform.Rotate(0, y * speed, 0);
    }

    private void Shoot()
    {
        animationState.AnimationManager("Shoot");
        heavy.ShootTrigger();
    }

    private void InCombat()
    {
        Debug.Log("Player Found");
        RaycastHit hit;
        Debug.DrawRay(faceDirection.position, faceDirection.forward * 36f, Color.green);
        if (Physics.Raycast(faceDirection.position, faceDirection.forward, out hit, 36f))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                if (PlayerHealth.health.GetCurrentHealth() <= 0)
                {
                    //Debug.Log("Player Defeated");
                    GetReward(4);
                    //ground.material.SetColor("_Color", Color.green);
                    //EndEpisode();
                }
                Shoot();
            }
        }
    }

    private void CheckRay()
    {
        //Front vision
        RayPerceptionOutput frontOut = RayPerceptionSensor.Perceive(frontRay.GetRayPerceptionInput());
        int rayFrontLength = frontOut.RayOutputs.Length;
        bool playerFound_F = false;
        bool foundObstacle_F = false;
        for (int i = 0; i < rayFrontLength; i++)
        {
            GameObject goHit = frontOut.RayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var EnemyDirection = frontOut.RayOutputs[i].EndPositionWorld - frontOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = EnemyDirection.magnitude;
                float rayHitDistance = frontOut.RayOutputs[i].HitFraction * scaledRayLength;
                if (goHit.gameObject.tag == "Untagged" && rayHitDistance <= 10.0f)
                {
                    foundObstacle_F = true;
                }
                if (goHit.gameObject.tag == "Player")
                {
                    playerFound_F = true;
                }
            }
        }

        //Side hit box
        RayPerceptionOutput sideOut = RayPerceptionSensor.Perceive(sideRay.GetRayPerceptionInput());
        int raySideLength = sideOut.RayOutputs.Length;
        bool foundObstacle_S = false;
        for (int i = 0; i < raySideLength; i++)
        {
            GameObject goHit = sideOut.RayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var rayDirection = sideOut.RayOutputs[i].EndPositionWorld - sideOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = rayDirection.magnitude;
                float rayHitDistance = sideOut.RayOutputs[i].HitFraction * scaledRayLength;

                if (goHit.gameObject.tag == "Untagged")
                {
                    foundObstacle_S = true;
                }
            }
        }
        if (foundObstacle_F && foundObstacle_S)
        {
            GetReward(1);
            //ground.material.SetColor("_Color", Color.red);
            //EndEpisode();
        }
        if (playerFound_F && !isCombat)
        {
            isCombat = true;
        }
        if (!playerFound_F && isCombat)
        {
            isCombat = false;
        }
    }
}
