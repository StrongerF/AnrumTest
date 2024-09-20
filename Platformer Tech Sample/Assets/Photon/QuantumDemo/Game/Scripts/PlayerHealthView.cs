using Photon.Deterministic;
using Quantum;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(EntityView))]
public class PlayerHealthView : MonoBehaviour
{
    private HealthUI healthUI;
    private DeathUI deathUI;


    private EntityView _view;

    private void Awake()
    {
        QuantumEvent.Subscribe(this, (EventHealthUpdated e) => HandleHealthUpdateEvent(e));
        QuantumEvent.Subscribe(this, (EventPlayerDied e) => HandleDeathEvent(e));
        healthUI = FindAnyObjectByType<HealthUI>();
        deathUI = FindAnyObjectByType<DeathUI>(FindObjectsInactive.Include);

        _view = GetComponent<EntityView>();
    }

    private void Start()
    {
        InitHealth(_view.EntityRef);
    }

    void InitHealth(EntityRef entity)
    {
        if (!IsLocalPlayer(entity))
            return;


        if (healthUI == null)
        {
            Debug.LogWarning("Health UI is null");
        }
        else
        {
            Frame f = QuantumRunner.Default.Game.Frames.Verified;
            if (f.TryGet<CharacterHealth>(entity, out var characterHealth))
            {
                healthUI.UpdateText(characterHealth.CurrentHealth);
            }
        }
    }

    void HandleHealthUpdateEvent(EventHealthUpdated e)
    {
        if (!IsLocalPlayer(e.Entity))
            return;
        if (healthUI == null)
        {
            Debug.LogWarning("Health UI is null");
        }
        else
        {
            healthUI.UpdateText(e.HealthValue);
        }
    }

    void HandleDeathEvent(EventPlayerDied e)
    {
        if (!IsLocalPlayer(e.Entity))
            return;

        if (deathUI == null)
        {
            Debug.LogWarning("Death UI is null");
        }
        else
        {
            deathUI.ShowDeath();
        }
    }

    bool IsLocalPlayer(EntityRef entity)
    {
        Frame f = QuantumRunner.Default.Game.Frames.Verified;
        PlayerLink playerLink = f.Get<PlayerLink>(entity);
        return QuantumRunner.Default.Session.IsLocalPlayer(playerLink.Player);
    }


    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
}
