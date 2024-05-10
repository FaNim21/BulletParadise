using BulletParadise.Entities.Bosses;
using UnityEngine;

namespace BulletParadise
{
    [CreateAssetMenu(fileName = "new NoMoveResetPhase", menuName = "Boss/Phase/NoMoveReset")]
    public class NoMoveResetPhase : Phase
    {
        private float _timer;

        private readonly float _timeToRestart = 2.25f;
        private readonly float _timeToVulnerability = 1f;


        public override void OnEnter()
        {
            _timer = 0f;

            boss.SetSpeedAnim(0);
        }

        public override void LogicUpdate(Vector2 targetDirection)
        {
            if (_timer >= _timeToVulnerability && boss.entity.healthManager.IsInvulnerable())
                boss.entity.healthManager.SetInvulnerability(false);

            if (_timer >= _timeToRestart)
                boss.GoToNextPhase();

            _timer += Time.deltaTime;
        }

        public override void PhysicsUpdate(Rigidbody2D rb)
        {

        }

        public override void OnExit()
        {

        }

        public override bool UpdatePhaseOnHit() => false;
        public override int CountAsRealPhase() => 0;
    }
}
