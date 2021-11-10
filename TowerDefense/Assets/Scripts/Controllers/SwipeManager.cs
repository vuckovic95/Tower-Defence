using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Assertions;

public class SwipeManager : MonoBehaviour
{
    [BoxGroup("Speed")]
    [SerializeField]
    private float _speed = 0.006f;

    [BoxGroup("Turret")]
    [SerializeField]
    private Transform _turret;

    private Transform _transform;
    private float _deltaX;
    private float _deltaY;
    private Rigidbody _rigidbody;

    private Quaternion _rotationY;
    private bool _canSwipe;

    private bool _touchSupported => UnityEngine.Input.touchSupported;
    private Touch? _fakeTouch => SimulateTouchWithMouse.Instance.FakeTouch;

    private void Awake()
    {
        _transform = this.transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {
        Actions.ToMenuAction += ResetData;
        Actions.StartGameAction += ResetData;
        Actions.BeginDragTurret += () => { _canSwipe = false; };
        Actions.EndDragTurret += () => { _canSwipe = true; };
    }

    private void ResetData()
    {
        _turret.rotation = Quaternion.Euler(Vector3.zero);
        _canSwipe = true;
    }

    public bool GetButton(string buttonName)
    {
        return UnityEngine.Input.GetButton(buttonName);
    }

    public bool GetButtonDown(string buttonName)
    {
        return UnityEngine.Input.GetButtonDown(buttonName);
    }

    public bool GetButtonUp(string buttonName)
    {
        return UnityEngine.Input.GetButtonUp(buttonName);
    }

    public bool GetMouseButton(int button)
    {
        return UnityEngine.Input.GetMouseButton(button);
    }

    public bool GetMouseButtonDown(int button)
    {
        return UnityEngine.Input.GetMouseButtonDown(button);
    }

    public bool GetMouseButtonUp(int button)
    {
        return UnityEngine.Input.GetMouseButtonUp(button);
    }

    public int touchCount
    {
        get
        {
            if (_touchSupported)
            {
                return UnityEngine.Input.touchCount;
            }
            else
            {
                return _fakeTouch.HasValue ? 1 : 0;
            }
        }
    }

    public Touch GetTouch(int index)
    {
        if (_touchSupported)
        {
            return UnityEngine.Input.GetTouch(index);
        }
        else
        {
            Assert.IsTrue(_fakeTouch.HasValue && index == 0);
            return _fakeTouch.Value;
        }
    }

    public Touch[] touches
    {
        get
        {
            if (_touchSupported)
            {
                return UnityEngine.Input.touches;
            }
            else
            {
                return _fakeTouch.HasValue ? new[] { _fakeTouch.Value } : new Touch[0];
            }
        }
    }

    private void FixedUpdate()
    {
         if (States.GameStateReference is States.GameState.Play && _canSwipe)
         {
#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);             
                _rotationY = Quaternion.Euler(0f, touch.deltaPosition.x * _speed * Time.fixedDeltaTime, 0f);
                _turret.rotation = _rotationY * _turret.transform.rotation;
            }
#endif


#if UNITY_EDITOR
        //tr.Translate(Input.GetAxis("Horizontal") * 50 * Time.deltaTime, 0, 0);
            if (touchCount > 0) 
            { 
                _rotationY = Quaternion.Euler(0f, touches[0].deltaPosition.x * _speed * Time.fixedDeltaTime, 0f);
                _turret.rotation = _rotationY * _turret.transform.rotation;
            }
#endif
         }
    }
}

internal class SimulateTouchWithMouse
{
    static SimulateTouchWithMouse instance;
    float lastUpdateTime;
    Vector3 prevMousePos;
    Touch? fakeTouch;


    public static SimulateTouchWithMouse Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SimulateTouchWithMouse();
            }

            return instance;
        }
    }

    public Touch? FakeTouch
    {
        get
        {
            update();
            return fakeTouch;
        }
    }

    private void update()
    {
        if (Time.time != lastUpdateTime)
        {
            lastUpdateTime = Time.time;

            var curMousePos = UnityEngine.Input.mousePosition;
            var delta = curMousePos - prevMousePos;
            prevMousePos = curMousePos;

            fakeTouch = createTouch(getPhase(delta), delta);
        }
    }

    private static TouchPhase? getPhase(Vector3 delta)
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            return TouchPhase.Began;
        }
        else if (UnityEngine.Input.GetMouseButton(0))
        {
            return delta.sqrMagnitude < 0.01f ? TouchPhase.Stationary : TouchPhase.Moved;
        }
        else if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            return TouchPhase.Ended;
        }
        else
        {
            return null;
        }
    }

    private static Touch? createTouch(TouchPhase? phase, Vector3 delta)
    {
        if (!phase.HasValue)
        {
            return null;
        }

        var curMousePos = UnityEngine.Input.mousePosition;
        return new Touch
        {
            phase = phase.Value,
            type = TouchType.Indirect,
            position = curMousePos,
            rawPosition = curMousePos,
            fingerId = 0,
            tapCount = 1,
            deltaTime = Time.deltaTime,
            deltaPosition = delta
        };
    }
}
