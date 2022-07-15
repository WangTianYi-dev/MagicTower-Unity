/*
 * file: Player.cs
 * author: DeamonHook
 * date: 7/8/2022
 * feature: 控制玩家(勇者)行为的类
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Unit
{
    private Direction _playerDir;

    /// <summary>
    ///  方向变换
    /// </summary>
    public Direction playerDir
    {
        get { return _playerDir; }
        set
        {
            // 当且仅当运动方向改变时才触发动画的变化
            if (_playerDir == value) return;
            _animator.SetTrigger(value.ToString());
            _playerDir = value;
        }
    }

    /// <summary>
    /// 获取玩家面对方向的下一个坐标
    /// </summary>
    public Vector2Int NextPos
    {
        get
        {
            switch (_playerDir)
            {
                case Direction.Left:
                    return _position + GLOBAL.LEFT;
                case Direction.Right:
                    return _position + GLOBAL.RIGHT;
                case Direction.Up:
                    return _position + GLOBAL.UP;
                case Direction.Down:
                    return _position + GLOBAL.DOWN;
                default:
                    return _position;
            }
        }
    }

    public State playerState
    {
        get { return _state; }
        set
        {
            if (_state == value) return;

            // 当且仅当进入或离开移动状态时才设置isMove
            if (value == State.Move)
            {
                _animator.SetFloat("isMove", 1f);
            }
            else
            {
                _animator.SetFloat("isMove", 0f);
            }

            _state = value;
        }
    }

    public float moveSpeed = 5.0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private Direction? GetKeyboardInput()
    {
        Direction? dir = null;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dir = Direction.Up;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.DownArrow))
        {
            dir = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Direction.Down;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Direction.Right;
        }
        return dir;
    }

    /// <summary>
    /// 当前移动的目标位置
    /// </summary>
    private Vector2Int _targetPos;

    /// <summary>
    /// Transform组件的移动(不负责任何其他逻辑)
    /// </summary>
    /// <returns>当组件移动到目标后返回true, 若尚未移动至目标则返回false</returns>
    private bool MoveTransform()
    {
        Vector3 tpos = new Vector3(_targetPos.x, _targetPos.y, 0);
        transform.position = Vector3.MoveTowards(
            transform.position, tpos, moveSpeed * Time.deltaTime
            );
        if (transform.position == tpos) return true;
        else return false;
    }

    private Vector2Int[] _path;
    // TODO SetPath

    private void Update()
    {
        switch (playerState)
        {
            case State.Move:
                MoveTransform();
                break;
            case State.Idle:
                //TODO
                break;
            default:
                break;
        }
    }
}

