using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class BloodSplashFactory : MonoBehaviour
    {
        [SerializeField] private SavedBloodSplashes savedBloodSplashes;
        [SerializeField] private SpriteAtlas atlas;
        [SerializeField] private SpriteRenderer bloodSplashPrefab;
        [SerializeField] private int maxSplashesPerScene = 16;
        [SerializeField] private bool alphaDecay = true;

        private Sprite[] _sprites;
        private readonly Dictionary<int, SpriteRenderer> _createdSplashes = new Dictionary<int, SpriteRenderer>();
        private int _id = 0;

        private void Awake()
        {
            GameSettings.Instance.PropertyValueChanged += OnSettingsUpdated;
            _isBloodEnabled = GameSettings.Instance.BloodEnabled;

            _sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(_sprites);

            var currentSceneName = gameObject.scene.name;
            if (savedBloodSplashes.lastScene != currentSceneName)
            {
                savedBloodSplashes.Clear();
                savedBloodSplashes.lastScene = currentSceneName;
            }
            else
            {
                RecreateBloodSplashesFromLastSession();
            }
        }

        private void OnDestroy()
        {
            GameSettings.Instance.PropertyValueChanged -= OnSettingsUpdated;
        }

        private void OnSettingsUpdated(string updatedProperty)
        {
            var settings = GameSettings.Instance;
            if (updatedProperty != nameof(settings.BloodEnabled))
            {
                return;
            }

            _isBloodEnabled = settings.BloodEnabled;
            if (!_isBloodEnabled)
            {
                DestroyActiveBlood();
            }
        }

        private bool _isBloodEnabled;

        private void DestroyActiveBlood()
        {
            foreach (var spriteRenderer in _createdSplashes.Values.ToArray())
            {
                Destroy(spriteRenderer.gameObject);
            }

            _createdSplashes.Clear();
            savedBloodSplashes.Clear();
            _id = 0;
        }

        private void OnApplicationQuit()
        {
            savedBloodSplashes.Clear();
        }

        private void RecreateBloodSplashesFromLastSession()
        {
            var bloodSplashes = savedBloodSplashes.BloodSplashes.ToArray();
            savedBloodSplashes.Clear();
            foreach (var bloodSplashInfo in bloodSplashes)
            {
                CreateBloodSplashAtInternal(bloodSplashInfo.position, true,
                    bloodSplashInfo.rotation, atlasIndex: bloodSplashInfo.atlasIndex);
            }
        }

        public void CreateBloodSplashAt(Vector2 position)
        {
            if (_isBloodEnabled)
            {
                CreateBloodSplashAtInternal(position, true);
            }
        }

        private void CreateBloodSplashAtInternal(Vector2 position, bool addInfo,
            Quaternion? rotation = null, int? id = null, int? atlasIndex = null)
        {
            if (savedBloodSplashes.Count == maxSplashesPerScene)
            {
                var toRemove = savedBloodSplashes.PopLast();
                var key = toRemove.id;
                var toRemoveObj = _createdSplashes[key];
                _createdSplashes.Remove(key);
                Destroy(toRemoveObj);
            }

            if (alphaDecay)
            {
                DecayAlpha();
            }

            atlasIndex ??= Random.Range(0, atlas.spriteCount);

            var sprite = _sprites[atlasIndex.Value];
            rotation ??= Quaternion.Euler(0, 0, Random.Range(-180f, 180f));
            var obj = Instantiate(bloodSplashPrefab, position, rotation.Value, transform);
            obj.sprite = sprite;

            if (id == null)
            {
                id = _id++;
            }
            else if (id.Value >= _id)
            {
                _id = id.Value + 1;
            }

            _createdSplashes[id.Value] = obj;

            if (!addInfo)
            {
                return;
            }

            savedBloodSplashes.AddInfo(new SavedBloodSplashes.BloodSplashInfo
            {
                atlasIndex = atlasIndex.Value,
                position = position,
                rotation = rotation.Value,
                id = id.Value
            });
        }

        private void DecayAlpha()
        {
            var decay = 1f / maxSplashesPerScene;
            foreach (var bloodSplashInfo in savedBloodSplashes.BloodSplashes)
            {
                if (!_createdSplashes.TryGetValue(bloodSplashInfo.id, out var obj))
                {
                    continue;
                }

                var color = obj.color;
                color.a -= decay;
                obj.color = color;
            }
        }
    }
}