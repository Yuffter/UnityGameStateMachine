using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yuffter.GameStateMachine
{
    public class GameStateMachine : MonoBehaviour
    {
        #region シングルトン初期設定
        private static GameStateMachine _instance;
        public static GameStateMachine Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<GameStateMachine>();
                }

                if (_instance == null)
                {
                    Debug.LogError("[Game StateMachine] ステートマシンが見つかりません");
                    return null;
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
        }
        #endregion
        private IState _currentState;
        /// <summary>
        /// 現在のステート
        /// </summary>
        public IState CurrentState => _currentState;
        private Dictionary<Type, IState> _stateCache = new Dictionary<Type, IState>();

        private void Update()
        {
            _currentState?.Update();
        }

        /// <summary>
        /// 初期ステートを設定します
        /// </summary>
        /// <param name="state">初期ステート</param>
        public void SetInitialState<T>() where T : IState
        {
            ChangeState<T>();
        }

        /// <summary>
        /// ステートを変更します
        /// </summary>
        /// <typeparam name="T">遷移先のステートの型</typeparam>
        public void ChangeState<T>() where T : IState
        {
            if (_currentState?.GetType() == typeof(T)) return;

            Debug.Log($"[Game StateMachine] {_currentState?.GetType().Name} -> {typeof(T).Name}");

            _currentState?.Exit();
            if (!_stateCache.ContainsKey(typeof(T)))
            {
                IState state = (IState)Activator.CreateInstance(typeof(T));
                _currentState = state;
                _stateCache[typeof(T)] = state;
            }
            else 
            {
                _currentState = _stateCache[typeof(T)];
            }

            _currentState?.Enter();
        }
    }
}