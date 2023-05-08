using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Unicorn : MonoBehaviour
{
    [SerializeField, Range (0,1)] float moveDuration = 0.1f;
    [SerializeField, Range (0,1)] float jumpHeight = 0.5f;
    [SerializeField] int leftMoveLimit;
    [SerializeField] int rightMoveLimit;
    [SerializeField] int backMoveLimit;
    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioClip carCrashClip;
    [SerializeField] AudioClip gotCoinClip;


    public UnityEvent<Vector3> OnJumpEnd;
    public UnityEvent<int> OnGetCoin;
    public UnityEvent OnDie;

    private bool isMoveable = false;

    void Update()
    {
        if(isMoveable == false)
            return;

        if (DOTween.IsTweening(transform))
            return;

        Vector3 direction = Vector3.zero;
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction += Vector3.forward;
        }
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction += Vector3.back;
        }
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction += Vector3.right;
        }
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction += Vector3.left;
        }
            if(direction == Vector3.zero)
                return;
            Move(direction);
    }
    public void Move(Vector3 direction)
    {
        var targetPosition = transform.position + direction;
        
        // check apakah target posisi valid  
        if(targetPosition.x < leftMoveLimit || 
            targetPosition.x > rightMoveLimit || 
            targetPosition.z < backMoveLimit || 
            Tree.AllPositions.Contains(targetPosition))
        {
            targetPosition = transform.position;
        }

        transform
            .DOJump(
            targetPosition,
            jumpHeight,
            1, 
            moveDuration)
            .onComplete = BroadcastPositionOnJumpEnd;

        transform.forward = direction;
    }

     public void UpdateMoveLimit(int horizontalSize, int backLimit)
     {
        leftMoveLimit = -horizontalSize/2;
        rightMoveLimit = horizontalSize/2;
        backMoveLimit = backLimit;
     }
    public void BroadcastPositionOnJumpEnd()
    {
        OnJumpEnd.Invoke(transform.position);
    }

    public void SetMoveable(bool value)
    {
        isMoveable = value;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Car"))
        {
            if(transform.localScale.y == 0.1f)
                return;
            
            transform.DOScale(new Vector3(2,0.1f,2),0.2f);
            isMoveable = false;
            audioManager.PlaySFX(carCrashClip);
            Invoke("Die",3);
        }
        else if (other.CompareTag("Coin"))
        {
            Debug.Log("Coin Enter");
            var coin = other.GetComponent<Coin>();
            OnGetCoin.Invoke(coin.Value);
            coin.Collected();
            audioManager.PlaySFX(gotCoinClip);
            
        }

        else if (other.CompareTag("Balon"))
        {
            if(this.transform != other.transform)
            {
                this.transform.SetParent(other.transform);
                Invoke("Die",3);
            }
        }
    }

    private void Die()
    {
        OnDie.Invoke();
    }

}
