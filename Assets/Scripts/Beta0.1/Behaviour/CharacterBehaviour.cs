using UnityEngine;

public abstract class CharacterBehaviour : MonoBehaviour
{

    private void FixedUpdate()
    {
        Move();
    }
    protected abstract void Move();
    protected abstract void Jump();
    protected abstract void Interact();
    protected abstract void Die();

}
