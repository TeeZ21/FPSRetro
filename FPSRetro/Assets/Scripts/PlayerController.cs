using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using EZCameraShake;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    public static PlayerController _playerInstance;
    [SerializeField] private Rigidbody2D _rb = null;
    [SerializeField] private float _moveSpeed;
    private Vector2 _moveInput;
    private Vector2 _mouseInput;
    [SerializeField] private float _mouseSensitivity = 1f;
    [SerializeField] private Camera _viewCamera = null;
    [SerializeField] private GameObject _bulletImpact = null;
    public int _currentAmmo;
    [SerializeField] private Animator _gunAnim;
    public int _currentHealth;
    public int _maxHealth = 100;
    [SerializeField] GameObject _deadScreen = null;
    private bool _hasDied;
    [SerializeField] private Text _healthText, _ammoText;
    [SerializeField] private Animator _movingAnim;

    [SerializeField] private Animator _damageCrosshair;

    [SerializeField] private GameObject _pauseMenu;

    public bool _hasShotgunInHand;
    [SerializeField] private GameObject _shotgun;
    public Animator _shotgunSelection;
    [SerializeField] private GameObject _weaponsSelection;
    #endregion Fields

    void Awake()
    {
        _playerInstance = this;
    }

    void Start()
    {
        _currentHealth = _maxHealth;
        _healthText.text = _currentHealth.ToString() + "%";
        _ammoText.text = _currentAmmo.ToString();
        _pauseMenu.SetActive(false);
        _hasShotgunInHand = true;
        _shotgun.SetActive(true);
        _weaponsSelection.SetActive(false);
    }

    void Update()
    {
        #region Methods
        if(!_hasDied)
        {
            #region Player Movement
            _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 moveHorizontal = transform.up * -_moveInput.x;
            Vector3 moveVertical = transform.right * _moveInput.y;
            _rb.velocity = (moveHorizontal + moveVertical) * _moveSpeed;
            #endregion Player Movement
            #region Player View Control
            _mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * _mouseSensitivity;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - _mouseInput.x);
            _viewCamera.transform.localRotation = Quaternion.Euler(_viewCamera.transform.localRotation.eulerAngles + new Vector3(0f, _mouseInput.y, 0f));
            #endregion Player View Control
            #region Shooting{
            if (Input.GetMouseButtonDown(0) && _hasShotgunInHand)
            {
                if (_currentAmmo > 0)
                {
                    Ray ray = _viewCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                    RaycastHit hit;
                    CameraShaker.Instance.ShakeOnce(2f,2f,.1f,.1f);
                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log("I'm looking at " + hit.transform.name);
                        Instantiate(_bulletImpact, hit.point, transform.rotation);
                        if (hit.transform.tag == "Enemy")
                        {
                            hit.transform.parent.GetComponent<EnemyController>().TakeDamage();
                            _damageCrosshair.SetTrigger("Hit");
                        }
                    }
                    else
                    {
                        //Debug.Log("I'm looking at nothing!");
                    }
                    _currentAmmo--;
                    _gunAnim.SetTrigger("Shoot");
                    UpdateAmmoUI();
                }
            }
            
            if(_moveInput != Vector2.zero)
            {
                _movingAnim.SetBool("isMoving", true);
            }
            else
            {
                _movingAnim.SetBool("isMoving", false);
            }

            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                //_weaponsSelection.SetActive(true);
                _hasShotgunInHand = true;
                _shotgunSelection.SetTrigger("GetShotgun");
            }
            #endregion Shooting

        }
        #region PauseMenu
        if(Input.GetKeyDown(KeyCode.P))
        {
            _pauseMenu.SetActive(true);
        }
        #endregion PauseMenu
        
    }
    #region Damage
    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        if (_currentHealth <= 0)
        {
            _deadScreen.SetActive(true);
            _hasDied = true;
            _currentHealth = 0;
        }

        _healthText.text = _currentHealth.ToString() + "%";

    }
    #endregion Damage
    #region Health
    public void AddHealth(int healAmount)
    {
        _currentHealth += healAmount;
        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        _healthText.text = _currentHealth.ToString() + "%";

    }
    #endregion Health
    #region Ammo
    public void UpdateAmmoUI()
    {
        _ammoText.text = _currentAmmo.ToString();
    }
    #endregion Ammo
    #region MovementSystem
    /*public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

    }*/
    #endregion MovementSystem
    #region Shotgun
    public void HasShotgun()
    {
        if(_hasShotgunInHand)
        {
            _shotgun.SetActive(true);
        }
        else
        {
            _shotgun.SetActive(false);
        }
    }
    #endregion Shotgun
    #endregion Methods
}