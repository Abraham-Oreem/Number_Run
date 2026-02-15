using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] ChainController chainController;
    [SerializeField] NumberGenerator numberGenerator;
    [SerializeField] Number playerNumber;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float Xspeed = 0.02f;
    [SerializeField] float XMin = -5f;
    [SerializeField] float XMax = 5f;
    [SerializeField] float tiltAmount = 25f;
    [SerializeField] float tiltSpeed = 8f;
    [SerializeField] GameObject endScreen;

    private float currentTilt;
    private Vector3 currentInput;

    private void Start()
    {
        SubEvents();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Number>(out var pickUp))
        {
            int newValue = playerNumber.value + pickUp.value;
            playerNumber.value = newValue;
            UpdateNumberVisual(newValue);
            pickUp.gameObject.SetActive(false); 
            chainController.AddtoChain(playerNumber.value);
        }
        else if (other.TryGetComponent<NumberReducer>(out var damage))
        {
            damage.DamageEffect(this);
            int newVal= damage.damage;
            ReduceNumberSmooth(newVal, damage.tweenDuration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<NumberReducer>(out var damage))
        {
           damage.StopTween(this);
            StopNumberTween();
        }
    }

    private Tween numberTween;

    private void ReduceNumberSmooth(int amount, float duration)
    {
        numberTween?.Kill();
        int startValue = playerNumber.value;
        playerNumber.value -= amount;
        int targetValue = Mathf.Max(0, startValue - amount);
        playerNumber.value = targetValue;
        float animatedValue = startValue; 

        numberTween = DOTween.To(
            () => animatedValue,        
            x =>                        
            {
                animatedValue = x;
                if (animatedValue == 0) GameEnd();
                int currentInt = Mathf.RoundToInt(animatedValue);
                UpdateNumberVisual(currentInt);
                chainController.AddtoChain(currentInt);
            },
            targetValue,
            duration
        )
        .SetEase(Ease.Linear);
        
    }


    private void StopNumberTween()
    {
        numberTween?.Kill();
        if (playerNumber.value == 0) GameEnd();
        UpdateNumberVisual(playerNumber.value);
        chainController.AddtoChain(playerNumber.value);
    }



    private void SubEvents()
    {
        InputManager.OnDrag += HandleDrag;
        InputManager.OnTouchEnd += StopMovement;
    }

    private void UnSubEvents()
    {
        InputManager.OnDrag -= HandleDrag;
        InputManager.OnTouchEnd -= StopMovement;
    }


    void HandleDrag(Vector2 delta)
    {
        currentInput = new Vector3((delta.x/(Screen.width * 0.5f)) * Xspeed, 0f, moveSpeed);
    }

    void StopMovement(Vector2 pos)
    {
        currentInput = new Vector3(0,0,moveSpeed);
    }

    private void Update()
    {
        Move();
        chainController.Follow(this.transform);
    }

    void Move()
    {
        transform.Translate(currentInput * Time.deltaTime, Space.World);

        float clampedX = Mathf.Clamp(transform.position.x, XMin, XMax);
        transform.position = new Vector3(clampedX,0.5f, transform.position.z);

    }

    

    void UpdateNumberVisual(int num)
    {
        Mesh mesh = numberGenerator.GenerateNumberMesh(num);
        playerNumber.SetMesh(mesh);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GameEnd()
    {
        UnSubEvents();
        currentInput = new Vector3(0, 0, 0);
        endScreen.SetActive(true);
    }


}
