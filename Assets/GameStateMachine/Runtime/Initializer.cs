using UnityEngine;

namespace Yuffter.GameStateMachine
{
    /// <summary>
    /// ステートマシンの初期化を行うクラス
    /// </summary>
    public static class Initializer
    {
        /// <summary>
        /// GameStateMachineの初期設定を行います
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void Initialize()
        {
            GameObject g = new GameObject("Game StateMachine");
            g.AddComponent<GameStateMachine>();
            GameObject.DontDestroyOnLoad(g);
        }
    }
}