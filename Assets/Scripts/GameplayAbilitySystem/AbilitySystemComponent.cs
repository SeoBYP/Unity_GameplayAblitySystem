using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enemy;
using GameplayAbilitySystem.Attributes;
using GameplayAbilitySystem.Enums;
using GameplayAbilitySystem.GameplayAbilities;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;
using GameplayAbilitySystem.SOs.GroupData;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements.Experimental;
using Task = System.Threading.Tasks.Task;

namespace GameplayAbilitySystem
{
    [Serializable]
    public class AbilitySystemComponent : MonoBehaviour
    {
        public GroupASC initialData;
        [ReadOnly] public Dictionary<string, GameplayAttribute> gameplayAttributesDictionary = new();
        public List<GameplayAttribute> attributes = new List<GameplayAttribute>();
        public Action<AttributeName, float, float, GameplayEffect> OnAttributeChanged;
        public Action<GameplayAttribute, GameplayEffect> OnPreAttributeChanged;

        [SerializeReference]
        public List<GameplayAttributeProcessor> attributeProcessors = new List<GameplayAttributeProcessor>();

        [SerializeReference] public List<GameplayAbility> grantedGameplayAbilities = new List<GameplayAbility>();
        public Action<GameplayAbility, string> OnGameplayAbilityPreActivated;
        public Action<GameplayAbility, string> OnGameplayAbilityActivated;
        public Action<GameplayAbility, string> OnGameplayAbilityTryActivated;
        public Action<GameplayAbility, string> OnGameplayAbilityDeactivated;

        public Action<GameplayAbility, string, EActivationFailure> OnGameplayAbilityFailedActivation;
        public Action<GameplayAbility> OnGameplayAbilityGranted;
        public Action<GameplayAbility> OnGameplayAbilityUngranted;

        public List<GameplayEffect> appliedGameplayEffects;
        public Action<GameplayEffect> OnGameplayEffectApplied;
        public Action<GameplayEffect> OnGameplayEffectRemoved;
        public Action<List<GameplayEffect>> OnGameplayEffectsChanged;

        public List<GameplayTag> tags;
        public Action<List<GameplayTag>, AbilitySystemComponent, AbilitySystemComponent, string> OnTagsChanged;
        public Action<List<GameplayTag>, AbilitySystemComponent, AbilitySystemComponent, string> OnTagsInstant;

        public float level = 1;

        public List<GameplayCue> instancedCues = new List<GameplayCue>();

        public bool logging = false;

        [ReadOnly] public bool invokeEventsGA = true;
        [ReadOnly] public bool invokeEventsGE = true;

        public bool inputBuffering = true;
        private float inputBufferDurationSeconds = .16f;

        public void Awake()
        {
            initialData.AddAttribute(this);
            initialData.AddAttributePorcessors(this);
            initialData.GrantAbilities(this);

            ResetStatsAttributesValues();

            // EVENT HANDLES
            //Trigger GameplayEffects Handles
            OnGameplayEffectApplied += (ge) => OnGameplayEffectsChanged?.Invoke(appliedGameplayEffects);
            OnGameplayEffectRemoved += (ge) => OnGameplayEffectsChanged?.Invoke(appliedGameplayEffects);

            //Trigger Attribute Change Handles
            attributes.ForEach(x => x.OnPostAttributeChanged += (attributeName, oldValue, newValue, ge) =>
            {
                OnAttributeChanged?.Invoke(attributeName, oldValue, newValue, ge);
            });
            attributes.ForEach(x => x.OnPreAttributeChange += (att, ge) => { OnPreAttributeChanged?.Invoke(att, ge); });
            attributes.ForEach(x =>
            {
                x.name = x.attributeName.name;
                gameplayAttributesDictionary.Add(x.attributeName.name, x);
            });

            // TAGS
            //Update gameplayEffectsTags
            OnGameplayEffectApplied += UpdateTagsOnEffectChange;
            OnGameplayEffectRemoved += UpdateTagsOnEffectChange;

            //Process GA tags
            OnGameplayAbilityActivated += UpdateTagsOnGameplayAbilityActivate;
            OnGameplayAbilityDeactivated += UpdateTagsOnGameplayAbilityDeactivate;

            OnGameplayEffectApplied += TriggerOnTagsAdded;

            attributeProcessors.ForEach(x => OnPreAttributeChanged += (att, ge) => { x.PreProcess(att, ge, this); });
            attributeProcessors.ForEach(x => OnAttributeChanged += (attributeName, oldValue, newValue, ge) =>
            {
                x.PostProcessed(attributeName, oldValue, newValue, ge);
            });

            GameplayCueManager.Register(this);
        }

        private void Start()
        {
            InitializeAttributesListeners();

            //Loggers
            if (logging)
            {
                // Debug.Log($"LOGGING {this.name}");
                OnPreAttributeChanged += (attribute, ge) =>
                {
                    Debug.Log($"OnPreAttributeChange: {attribute.attributeName.name} {ge?.name}");
                };
                OnAttributeChanged += (attributeName, oldValue, newValue, ge) =>
                {
                    Debug.Log($"{attributeName.name} {oldValue} -> {newValue} / ge: {ge?.name}");
                };
                // OnTagsChanged += (newTags, src, tgt) => Debug.Log($"[TAGS] OnTagsChanged! newTags: [{string.Join(", ", newTags.Select(x => x.name))}]");
                OnTagsInstant += (newTags, src, tgt, applicationGUID) =>
                    Debug.Log($"[TAGS] OnTagsInstant! tags: [{string.Join(", ", newTags.Select(x => x.name))}]");

                // OnGameplayEffectsChanged += (ges) => {
                //     var geNames = new List<string>();
                //     ges.ForEach(ge => geNames.Add(ge.name));
                //     // Debug.Log($"GameplayEffectsChanged, appliedGEs: {new JsonListWrapper<string>(geNames).ToJson()}");
                //     Debug.Log($"GameplayEffectsChanged, appliedGEs: [{string.Join(", ", geNames)}]");
                // };
                // OnGameplayEffectApplied += (newGE) => Debug.Log($"OnGameplayEffectApplied ge: {newGE.name} ");
                // OnGameplayEffectRemoved += (removedGE) => Debug.Log($"OnGameplayEffectRemoved ge: {removedGE.name}");
                // OnGameplayAbilityFailedActivation += (ga, activationGUID, failureCause) => Debug.Log($"GA Failed Activation: {ga.name} {failureCause}");
            }

            OnGameplayAbilityFailedActivation += (ga, activationGUID, failureCause) =>
                Debug.Log($"GA Failed Activation: {ga.name} {failureCause}");
        }

        private void OnDestroy()
        {
            foreach (var ga in grantedGameplayAbilities)
                if (ga.isActive)
                    ga.DeactivateAbility(); // Cleans up toggle/passive abilities.
        }

        //Tag events
        public void UpdateTagsOnEffectChange(GameplayEffect ge)
        {
            TagProcessor.UpdateTags(ge.source, ge.target,
                ref tags, appliedGameplayEffects,
                grantedGameplayAbilities, OnTagsChanged, ge.applicationGUID);
        }

        public void UpdateTagsOnGameplayAbilityActivate(GameplayAbility ga, string activationGUID)
        {
            //This needs to be a declared function, because we must remove this subscription for non owner client objects on multiplayer.
            TagProcessor.UpdateTags(ga.source, ga.target,
                ref tags, appliedGameplayEffects,
                grantedGameplayAbilities, OnTagsChanged, activationGUID);
        }

        public void UpdateTagsOnGameplayAbilityDeactivate(GameplayAbility ga, string activationGUID)
        {
            //This needs to be a declared function, because we must remove this subscription for non owner client objects on multiplayer.
            TagProcessor.UpdateTags(ga.source, ga.target,
                ref tags, appliedGameplayEffects,
                grantedGameplayAbilities, OnTagsChanged, activationGUID);
        }


        public void TriggerOnTagsAdded(GameplayEffect appliedGE)
        {
            if (appliedGE.gameplayEffectTags.GrantedTags.Count == 0) return;
            if (appliedGE.durationType == EGameplayEffectDurationType.Instant)
                OnTagsInstant?.Invoke(appliedGE.gameplayEffectTags.GrantedTags, appliedGE.source, appliedGE.target,
                    appliedGE.applicationGUID);
        }

        public float GetAttributeValue(AttributeName attName)
        {
            return GetAttributeValue(attName.name);
        }

        public float GetAttributeValue(string attNameString)
        {
            if (gameplayAttributesDictionary.TryGetValue(attNameString, out var attribute))
            {
                return attribute.GetValue();
            }

            Debug.LogWarning($"No Attribute named {attNameString}");
            return 0;
        }

        public void ResetStatsAttributesValues()
        {
            foreach (var att in attributes)
                att.currentValue = att.baseValue;
        }

        public void InitializeAttributesListeners()
        {
            foreach (var att in attributes)
            {
                att.OnPostAttributeChanged?.Invoke(att.attributeName, 0, att.baseValue, null);
            }
        }

        public void ApplyAttributeModifiersValues(GameplayEffect ge)
        {
            attributes.ForEach(att => att.ApplyModifiers(ge));
        }

        public void RefreshAttributesModifers(GameplayEffect ge)
        {
            attributes.ForEach(att => att.modification.Clear());
        }


        public GameplayAbility GrantAbility(GameplayAbility ga)
        {
            GameplayAbility gaCopy = ga.Instantiate(this);
            grantedGameplayAbilities.Add(gaCopy);
            OnGameplayAbilityGranted?.Invoke(gaCopy);

            return gaCopy;
        }

        public void UngrantAbilityByTag(GameplayTag tag)
        {
            var removeIndexes = new List<int>();
            grantedGameplayAbilities.ForEach(ga =>
            {
                if (ga.abiltyTags.DescriptionTags.Contains(tag))
                {
                    removeIndexes.Add(grantedGameplayAbilities.IndexOf(ga));
                }
            });
            removeIndexes.ForEach(i => UngrantAbility(i));
        }

        [ContextMenu("Try Ungrant Ability")]
        public void UngrantAbility(int index)
        {
            UngrantAbility(grantedGameplayAbilities[index]);
        }

        public void UngrantAbility(string guid)
        {
            UngrantAbility(grantedGameplayAbilities.Find(x => x.guid == guid));
        }

        public void UngrantAbility(GameplayAbility ga)
        {
            ga.DeactivateAbility(null);
            grantedGameplayAbilities.Remove(ga);
            OnGameplayAbilityUngranted?.Invoke(ga);
        }

        public List<GameplayTag> GetAllTags()
        {
            return tags;
        }

        public void TryActivateAbility(string abilityName, AbilitySystemComponent target)
        {
            GameplayAbility ga = grantedGameplayAbilities.Find(ga => ga.name == abilityName);
            if (ga == null) ga = grantedGameplayAbilities.Find(ga => ga.name.Contains(abilityName));
            if (ga == null)
            {
                Debug.Log($"No granted Ability named {abilityName}");
                return;
            }

            TryActivateAbility(ga, target, null);
        }

        [ContextMenu("Try Activate Ability")]
        public void TryActivateAbility(int index, AbilitySystemComponent target)
        {
            if (index >= grantedGameplayAbilities.Count)
            {
                Debug.Log($"No granted Ability at given index {grantedGameplayAbilities}");
                return;
            }

            TryActivateAbility(grantedGameplayAbilities[index], target);
        }

        public void TryActivateAbility(string guid, AbilitySystemComponent target, string activationGUID)
        {
            GameplayAbility ga = grantedGameplayAbilities.Find(ga => ga.guid == guid);
            if (ga == null)
            {
                Debug.Log($"No granted Ability with guid {guid}");
                return;
            }

            TryActivateAbility(ga, target, activationGUID);
        }


        public async void TryActivateAbility(GameplayAbility ga, AbilitySystemComponent target,
            string activationGUID = null)
        {
            ga.source = this;
            ga.target = target;
            ga.activationGUID = activationGUID;

            OnGameplayAbilityTryActivated?.Invoke(ga, activationGUID);

            if (ga.isActive)
            {
                ga.DeactivateAbility(ga.activationGUID);
                return;
            }

            await InputBuffering(ga, target, activationGUID);

            if (!ga.CanActivateAbility(this, target, activationGUID, true)) return;

            ga.CommitAbility(this, target, ga.activationGUID);
        }

        public async Task InputBuffering(GameplayAbility ga, AbilitySystemComponent target,
            string activationGUID = null)
        {
            //TODO: Add a check to know if the ga is already on the buffer.
            //If it is, stop PREVIOUS TryActivate buffer or reset its timer.
            //So we have only 1 checking loop for it at any given time.
            float finalTime = Time.realtimeSinceStartup + inputBufferDurationSeconds;
            while (!ga.isActive && Time.realtimeSinceStartup < finalTime &&
                   !ga.CanActivateAbility(this, target, activationGUID, false))
            {
                await Task.Delay(10);
            }
        }

        public GameplayEffect ApplyGameplayEffect(AbilitySystemComponent source, AbilitySystemComponent target,
            GameplayEffect ge, string applicationGUID = null)
        {
            if (logging)
                Debug.Log(
                    $"ASC ApplyGameplayEffect {ge.name} {this.name} applicationGUID: {applicationGUID} data: {JsonUtility.ToJson(ge, true)}");
            ge.source = source;
            ge.target = target;
            ge.applicationGUID = applicationGUID;

            if (!TagProcessor.CheckApplicationTagRequirementsGE(this, ge, tags))
            {
                if (logging)
                    Debug.Log($"GE: {ge.name} couldnt be applied on this ASC. Failed application tag requirements");
                return null;
            }

            if (ge.chanceToApply < 1f)
            {
                if (!(UnityEngine.Random.Range(0f, 1f) <= ge.chanceToApply)) return null;
            }

            GameplayEffect geCopy = ge.Instantiate();
            if (geCopy.durationType != EGameplayEffectDurationType.Instant) appliedGameplayEffects.Add(geCopy);

            switch (ge.durationType)
            {
                case EGameplayEffectDurationType.Infinite:
                case EGameplayEffectDurationType.Duration:
                    Debug.Log(
                        "[FREE VERSION] Duration and Infinite GameplayEffects are not fully available on the free version. Check GASify on the Assetstore for more options.");
                    RemoveDurationGameplayEffect(geCopy);
                    ApplyInstantGameplayEffect(geCopy);
                    break;
                case EGameplayEffectDurationType.Instant:
                    ApplyInstantGameplayEffect(geCopy);
                    break;
            }

            // 효과 처리
            geCopy.ApplyEffect(source, target);
            
            if (invokeEventsGE) OnGameplayEffectApplied?.Invoke(geCopy);
            return geCopy;
        }

        public async void RemoveDurationGameplayEffect(GameplayEffect ge)
        {
            await Task.Delay((int)(ge.durationValue * 1000)); // 밀리초 단위로 변환
            if (appliedGameplayEffects.Contains(ge))
            {
                ge.RemoveEffect(ge.source, ge.target);
                appliedGameplayEffects.Remove(ge);
                OnGameplayEffectRemoved?.Invoke(ge);
                Debug.Log($"Duration effect {ge.name} removed.");
            }
            appliedGameplayEffects.Remove(ge);
            OnGameplayEffectRemoved?.Invoke(ge);
        }

        List<Modifier> modifiersToProcess = new List<Modifier>();

        
        public void ApplyInstantGameplayEffect(GameplayEffect ge)
        {
            modifiersToProcess.Clear();
            modifiersToProcess.AddRange(ge.modifiers);

            foreach (var attribute in attributes)
            {
                foreach (var modifier in modifiersToProcess)
                {
                    if (attribute.attributeName == modifier.attributeName)
                    {
                        attribute.ApplyModifierAsResource(modifier, ge);
                        attribute.ApplyModifiers(ge);
                    }
                }
            }
        }
        
    }
}