using UnityEngine;

namespace Yuffter.GameStateMachine
{
    public interface IState
    {
        /// <summary>
        /// このステートに入ったときに呼ばれます
        /// </summary>
        void Enter();

        /// <summary>
        /// このステートの間、毎フレーム呼ばれます
        /// </summary>
        void Update();

        /// <summary>
        /// このステートから出ていくときに呼ばれます
        /// </summary>
        void Exit();
    }
}