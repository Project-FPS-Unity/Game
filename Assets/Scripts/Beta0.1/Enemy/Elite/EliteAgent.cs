using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EliteAgent : Agent
{
    [SerializeField] private RayPerceptionSensorComponent3D frontRay;
    [SerializeField] private RayPerceptionSensorComponent3D sideRay;

    private bool isStuck = false;
    private bool isAttack = false;
    private float distanceToTarget;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private EliteArms hitboxLeft;
    [SerializeField] private EliteArms hitboxRight;

    private float moveX;
    private float moveY;
    private float moveZ;
    private float moveSpeed = 7f;
    [SerializeField] private Transform faceDirection;
    private Vector3 moveToDirection;

    [SerializeField] private EliteAnimationController animationState;

    public override void OnEpisodeBegin()
    {
        playerTarget = GameObject.Find("Player").GetComponent<Transform>();
        isStuck = false;
        isAttack = false;
        //PlayerHealth.health.SetHealth(PlayerHealth.health.GetMaxHealth());
        if (playerTarget != null) distanceToTarget = Vector3.Distance(playerTarget.transform.localPosition, transform.position);
        //transform.localPosition = new Vector3(Random.Range(-5f, 5f), 0.5f, 5f);
        //if (playerTarget != null) playerTarget.localPosition = new Vector3(Random.Range(-10f, 10f), 1.8f, -5f);
        //transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(isAttack);
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(transform.rotation.eulerAngles / 180.0f - Vector3.one);
        if (playerTarget != null)
        {
            sensor.AddObservation((playerTarget.transform.localPosition - transform.position).normalized);
            sensor.AddObservation(distanceToTarget);
            sensor.AddObservation(playerTarget.transform.localPosition);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        moveX = actions.ContinuousActions[0];
        moveY = actions.ContinuousActions[1];
        moveZ = actions.ContinuousActions[2];
        distanceToTarget = Vector3.Distance(playerTarget.transform.localPosition, transform.position);
        if (isStuck)
        {
            TurnBack(moveSpeed);
            isStuck = false;
        }
        if (isAttack)
        {
            Attack();
        }
        Move(moveZ);
        Turn(moveY, moveSpeed);
        CheckRay();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contact = actionsOut.ContinuousActions;
        contact[0] = Input.GetAxisRaw("Horizontal");
        contact[1] = Input.GetAxisRaw("Mouse X");
        contact[2] = Input.GetAxisRaw("Vertical");
    }

    private void GetReward(int number)
    {
        switch (number)
        {
            case 0: //Walk forward
                AddReward(+2.5f);
                break;
            case 1: //turn back from wall
                AddReward(+5.0f);
                break;
            case 2: //Found Player
                AddReward(+10.0f);
                break;
            case 3: //Run toward Player
                AddReward(+20.0f);
                break;
            case 4: //Attack Hit
                AddReward(+30.0f);
                break;
            case 5: //Player Die
                AddReward(+50.0f);
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
        if (z > 0) GetReward(0);
        transform.position += moveToDirection * Time.deltaTime * moveSpeed;
    }

    private void Turn(float y, float speed)
    {
        transform.Rotate(0, y * speed, 0);
    }

    private void Attack()
    {
        if (PlayerHealth.health.GetCurrentHealth() <= 0)
        {
            GetReward(5);
            EndEpisode();
        }
        else animationState.AnimationManager("Attack");

        if (hitboxLeft.ishit)
        {
            GetReward(4);
            PlayerHealth.health.TakeDamage(20);
            hitboxLeft.ishit = false;
        }
        if (hitboxRight.ishit)
        {
            GetReward(4);
            PlayerHealth.health.TakeDamage(20);
            hitboxRight.ishit = false;
        }
    }

    private void TurnBack(float speed)
    {
        transform.Rotate(0, 180.0f * speed, 0);
    }

    private void CheckRay()
    {
        //Front vision
        RayPerceptionOutput frontOut = RayPerceptionSensor.Perceive(frontRay.GetRayPerceptionInput());
        int rayFrontLength = frontOut.RayOutputs.Length;
        bool foundObstacle = false;
        bool inAttackRange = false;
        for (int i = 0; i < rayFrontLength; i++)
        {
            GameObject goHit = frontOut.RayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var EnemyDirection = frontOut.RayOutputs[i].EndPositionWorld - frontOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = EnemyDirection.magnitude;
                float rayHitDistance = frontOut.RayOutputs[i].HitFraction * scaledRayLength;
                if (goHit.CompareTag("Player"))
                {
                    inAttackRange = true;
                }
                if (goHit.CompareTag("Untagged"))
                {
                    foundObstacle = true;
                }
            }
        }

        //Side vision
        RayPerceptionOutput sideOut = RayPerceptionSensor.Perceive(sideRay.GetRayPerceptionInput());
        int raySideLength = sideOut.RayOutputs.Length;
        bool seePlayer = false;
        for (int i = 0; i < raySideLength; i++)
        {
            GameObject goHit = sideOut.RayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var EnemyDirection = sideOut.RayOutputs[i].EndPositionWorld - sideOut.RayOutputs[i].StartPositionWorld;
                var scaledRayLength = EnemyDirection.magnitude;
                float rayHitDistance = sideOut.RayOutputs[i].HitFraction * scaledRayLength;
                if (goHit.CompareTag("Player"))
                {
                    if (rayHitDistance < distanceToTarget)
                    {
                        GetReward(3);
                    }
                }
                if (goHit.CompareTag("Player"))
                {
                    seePlayer = true;
                }
                else
                {
                    seePlayer = false;
                }
            }
        }

        if (foundObstacle) isStuck = true;
        if (seePlayer)
        {
            animationState.AnimationManager("PlayerFound");
            GetReward(2);
        }
        if (inAttackRange)
        {
            isAttack = true;
            GetReward(2);
        }
    }
}
