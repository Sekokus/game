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

        private Coroutine _routine;
        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = transform.localPosition;
        }

        public void Shake(float time, float speed, float magnitude)
        {
            StopShake();
            _startPosition = transform.localPosition;
            var settingsFactor = GameSettings.Instance.cameraShakeStrength;

            _routine = Do.EveryFrameFor(state =>
                {
                    var strength = state.PassedTime * speed;
                    var shakeRight = (Mathf.PerlinNoise(strength, strength * 2) - 0.5f) * transform.right;
                    var shakeUp = (Mathf.PerlinNoise(strength * 2, strength) - 0.5f) * transform.up;

                    var curveAffect = shakeCurve.Evaluate(state.Fraction);

                    var shake = (shakeRight + shakeUp) * (curveAffect * magnitude * settingsFactor);
                    transform.localPosition = new Vector3(shake.x, shake.y, _startPosition.z);
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
            motionBlur.intensity.value = GameSettings.Instance.motionBlurStrength;

            Do.After(() => { motionBlur.active = false; }, shakeTimeOnHitTaken)
                .Start(this);
        }

        public void PlayHitInflictedEffect()
        {
            // TODO: пока ничего
        }

        public void PlayDashEffect(float dashTime)
        {
            var profile = volume.profile;
            if (!profile.TryGet(out ChromaticAberration chromaticAberration))
            {
                return;
            }

            chromaticAberration.active = true;
            var settingsFactor = GameSettings.Instance.chromaticAberrationStrength;

            Do.EveryFrameFor(
                    state =>
                    {
                        var intensityValue = aberrationCurve.Evaluate(state.Fraction);
                        chromaticAberration.intensity.value = intensityValue * settingsFactor;
                    },
                    dashTime * dashTimeMultiplier)
                .Action(() => { chromaticAberration.active = false; })
                .Start(this);
        }
    }
}