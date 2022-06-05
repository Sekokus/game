using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DefaultNamespace
{
    public class CameraEffects : MonoBehaviour
    {
        [SerializeField] private Volume volume;
        [SerializeField] private AnimationCurve shakeCurve;
        [SerializeField] private AnimationCurve aberrationCurve;

        [SerializeField, Range(0, 3)] private float dashTimeMultiplier = 2;
        [SerializeField] private float shakeTimeOnHitTaken = 0.3f;
        [SerializeField] private float shakeSpeedOnHitTaken = 5;
        [SerializeField] private float shakeMagnitudeOnHitTaken = 2;
        [SerializeField] private float shakeTimeOnHitInflicted = 0.15f;
        [SerializeField] private float shakeSpeedOnHitInflicted = 7f;
        [SerializeField] private float shakeMagnitudeOnHitInflicted = 1.2f;

        private Coroutine _routine;
        private Vector3 _startPosition;

        public void Shake(float time, float speed, float magnitude)
        {
            StopShake();
            _startPosition = transform.localPosition;
            _routine = Do.EveryFrameFor(state =>
                {
                    var strength = state.PassedTime * speed;
                    var shakeRight = (Mathf.PerlinNoise(strength, strength * 2) - 0.5f) * transform.right;
                    var shakeUp = (Mathf.PerlinNoise(strength * 2, strength) - 0.5f) * transform.up;

                    var curveAffect = shakeCurve.Evaluate(state.Fraction);

                    var shake = (shakeRight + shakeUp) * (curveAffect * magnitude);
                    transform.localPosition = shake;
                }, time)
                .Action(OnShakeEnded)
                .Start(this);
        }

        private void OnShakeEnded()
        {
            transform.localPosition = _startPosition;
        }

        private void StopShake()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }

            OnShakeEnded();
        }

        public void PlayHitTakenEffect()
        {
            Shake(shakeTimeOnHitTaken, shakeSpeedOnHitTaken, shakeMagnitudeOnHitTaken);

            var profile = volume.profile;
            if (!profile.TryGet(out MotionBlur motionBlur))
            {
                return;
            }

            motionBlur.active = true;

            Do.After(() => { motionBlur.active = false; }, shakeTimeOnHitTaken)
                .Start(this);
        }

        public void PlayHitInflictedEffect()
        {
            Shake(shakeTimeOnHitInflicted, shakeSpeedOnHitInflicted, shakeMagnitudeOnHitInflicted);
        }

        public void PlayDashEffect(float dashTime)
        {
            var profile = volume.profile;
            if (!profile.TryGet(out ChromaticAberration chromaticAberration))
            {
                return;
            }

            chromaticAberration.active = true;

            Do.EveryFrameFor(
                    state => { chromaticAberration.intensity.value = aberrationCurve.Evaluate(state.Fraction); },
                    dashTime * dashTimeMultiplier)
                .Action(() => { chromaticAberration.active = false; })
                .Start(this);
        }
    }
}