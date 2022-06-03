using System;
using Player;
using UnityEngine;

namespace DefaultNamespace
{
    public class Finish : MonoBehaviour
    {
        private GameEvents _gameEvents;
        
        [SerializeField] private PlayerTriggerEvents triggerEvents;
        [SerializeField] private GameObject portalBody;
        [SerializeField] private GameObject interactHint;

        private bool _canInteract;
        private bool _canEnter;

        private void Awake()
        {
            _gameEvents = Container.Get<GameEvents>();
            _gameEvents.PlayerMinGoalCompleted += OnMinGoalCompleted;
            _gameEvents.PlayerInteract += OnInteract;
            
            triggerEvents.Entered += OnEntered;
            triggerEvents.Exited += OnExited;
        }

        private void OnDestroy()
        {
            _gameEvents.PlayerMinGoalCompleted -= OnMinGoalCompleted;
            _gameEvents.PlayerInteract -= OnInteract;
        }

        private void OnExited(PlayerCore obj)
        {
            _canInteract = false;
            UpdateHintState();
        }

        private void UpdateHintState()
        {
            interactHint.SetActive(_canEnter && _canInteract);
        }

        private void OnEntered(PlayerCore player)
        {
            _canInteract = true;
            UpdateHintState();
        }

        private void OnInteract()
        {
            if (!_canInteract || !_canEnter)
            {
                return;
            }
            
            _gameEvents.PostFinishReached();
        }

        private void OnMinGoalCompleted()
        {
            _canEnter = true;
            portalBody.SetActive(true);
            UpdateHintState();
        }
    }
}