using System.Collections;
using Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _moveVal;
    private Vector2 _rotateVal;
    private Vector3 _playerVelocity;
    private PlayerShooter _shooter;
    private FlashLight _flashLight;
    private Rigidbody _rb;
    private bool _moving;

    public float moveSpeed;
    public GameObject body;
    public GameObject head;
    public GameObject wheelSmokeL;
    public GameObject wheelSmokeR;
    
    private MeshRenderer _meshRenderer;
    private float _wheelRight;
    private float _wheelLeft;
    private static readonly int WheelRight = Shader.PropertyToID("_WheelRight");
    private static readonly int WheelLeft = Shader.PropertyToID("_WheelLeft");

    private void Start()
    {
        _shooter = head.GetComponent<PlayerShooter>();
        _flashLight = head.GetComponent<FlashLight>();
        _rb = GetComponent<Rigidbody>();
        _meshRenderer = transform.GetComponentInChildren<MeshRenderer>();

        StartCoroutine(Interpolation());
    }

    private IEnumerator Interpolation()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void OnMove(InputValue value)
    {
        _moveVal = value.Get<Vector2>();
        
        wheelSmokeR.SetActive(true);
        wheelSmokeL.SetActive(true);
        
    }

    private void OnRotate(InputValue value)
    {
        _rotateVal = value.Get<Vector2>();
    }

    private void OnFire()
    {
        _shooter.Fire();
    }

    private void OnLight()
    {
        if (_shooter.ShootOrder || GetComponent<PlayerHealth>().curHealth == 0) return;
        _flashLight.Light();
    }

    private void OnLightRelease()
    {
        _flashLight.LightRelease();
    }

    private void OnPause()
    {
        GameManager.instance.inGameUiManager.TogglePause();
    }

    private void FixedUpdate()
    {
        DoMove();
    }

    private void Update()
    {
        DoRotate();
    }

    private void DoMove()
    {
        if (_moveVal == Vector2.zero)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            if (_moving)
            {
                StartCoroutine(StopSmoke());
            }

            _moving = false;
            return;
        }
        
        body.transform.rotation = Quaternion.LookRotation(new Vector3(_moveVal.x, 0, _moveVal.y));

        _wheelLeft += moveSpeed;
        _wheelRight += moveSpeed;

        if (!_moving)
        {
            StopCoroutine(StopSmoke());
            _moving = true;
        }

        _meshRenderer.material.SetFloat(WheelLeft, _wheelLeft);
        _meshRenderer.material.SetFloat(WheelRight, _wheelRight);
        _rb.velocity = new Vector3(_moveVal.x * moveSpeed, _rb.velocity.y, _moveVal.y * moveSpeed);
    }

    private void DoRotate()
    {
        if (_rotateVal == Vector2.zero) return;
        head.transform.rotation = Quaternion.LookRotation(new Vector3(_rotateVal.x, 0, _rotateVal.y));
    }

    public void Immobilisation()
    {
        _moveVal = Vector2.zero;
        _rb.velocity = Vector3.zero;
    }

    private IEnumerator StopSmoke()
    {
        yield return new WaitForSeconds(0.3f);
        wheelSmokeR.SetActive(false);
        wheelSmokeL.SetActive(false);
        _moving = false;
    }
}