using System.Collections.Generic;
using Player;
using UnityEngine;

namespace DefaultNamespace
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private BoxOverlapTester overlapTester;
        private LevelGoalCounter _levelGoalCounter;

        private void Reset()
        {
            overlapTester = GetComponent<BoxOverlapTester>();
        }

        private void Awake()
        {
            _levelGoalCounter = Container.Get<LevelGoalCounter>();

            overlapTester.Overlap += OnOverlap;
        }

        private void OnOverlap(IReadOnlyList<Collider2D> overlaps)
        {
            foreach (var overlap in overlaps)
            {
                if (overlap.GetComponentInParent<PlayerCore>())
                {
                    _levelGoalCounter.IncrementCounter();
                    gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}