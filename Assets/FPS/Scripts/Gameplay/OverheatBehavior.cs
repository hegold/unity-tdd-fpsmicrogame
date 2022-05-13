using UnityEngine;
using System.Collections.Generic;
using Unity.FPS.Game;
using System;

namespace Unity.FPS.Gameplay
{
    public class OverheatBehavior : MonoBehaviour
    {
        [System.Serializable]
        public struct RendererIndexData
        {
            public Renderer Renderer;
            public int MaterialIndex;

            public RendererIndexData(Renderer renderer, int index)
            {
                this.Renderer = renderer;
                this.MaterialIndex = index;
            }
        }

        [Header("Visual")] [Tooltip("The VFX to scale the spawn rate based on the ammo ratio")]
        public ParticleSystem SteamVfx;

        [Tooltip("The emission rate for the effect when fully overheated")]
        public float SteamVfxEmissionRateMax = 8f;

        //Set gradient field to HDR
        [GradientUsage(true)] [Tooltip("Overheat color based on ammo ratio")]
        public Gradient OverheatGradient;

        [Tooltip("The material for overheating color animation")]
        public Material OverheatingMaterial;

        [Header("Sound")] [Tooltip("Sound played when a cell are cooling")]
        public AudioClip CoolingCellsSound;

        [Tooltip("Curve for ammo to volume ratio")]
        public AnimationCurve AmmoToVolumeRatioCurve;


        WeaponController m_Weapon;
        AudioSource m_AudioSource;
        List<RendererIndexData> m_OverheatingRenderersData;
        MaterialPropertyBlock m_OverheatMaterialPropertyBlock;
        float m_LastAmmoRatio;
        ParticleSystem.EmissionModule m_SteamVfxEmissionModule;

        void Awake()
        {
            var emissionModule = SteamVfx.emission;
            emissionModule.rateOverTimeMultiplier = 0f;

            m_OverheatingRenderersData = new List<RendererIndexData>();
            foreach (var renderer in GetComponentsInChildren<Renderer>(true))
            {
                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    if (renderer.sharedMaterials[i] == OverheatingMaterial)
                        m_OverheatingRenderersData.Add(new RendererIndexData(renderer, i));
                }
            }

            m_OverheatMaterialPropertyBlock = new MaterialPropertyBlock();
            m_SteamVfxEmissionModule = SteamVfx.emission;

            m_Weapon = GetComponent<WeaponController>();
            DebugUtility.HandleErrorIfNullGetComponent<WeaponController, OverheatBehavior>(m_Weapon, this, gameObject);

            m_AudioSource = gameObject.AddComponent<AudioSource>();
            m_AudioSource.clip = CoolingCellsSound;
            m_AudioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.WeaponOverheat);



            m_AmmoRatioChangeTracker = new ValueChangeTracker<float>(
                () => m_Weapon.CurrentAmmoRatio,
                newAmmoRatio => UpdateVisualSmoke(newAmmoRatio)
            );
    }

        public class ValueChangeTracker<T> where T : IEquatable<T>
        {
            // Note: could pass the new value to Update vs having a query, then it's even simpler but more code at call site
            private readonly Func<T> QueryCurrentValue;
            private readonly Action<T> OnChangeAction;
            private T LastValue;

            public ValueChangeTracker(Func<T> queryForValue, Action<T> action)
            {
                QueryCurrentValue = queryForValue;
                OnChangeAction = action;
                LastValue = QueryCurrentValue();
            }

            public void Update()
            {
                var newValue = QueryCurrentValue();
                if (!LastValue.Equals(newValue))
                {
                    OnChangeAction(newValue);
                    LastValue = newValue;
                }
            }
        }

        private ValueChangeTracker<float> m_AmmoRatioChangeTracker;

        void Update()
        {
            m_AmmoRatioChangeTracker.Update();

            // cooling sound
            if (CoolingCellsSound)
            {
                if (!m_AudioSource.isPlaying
                    && m_Weapon.CurrentAmmoRatio != 1
                    && m_Weapon.IsWeaponActive
                    && m_Weapon.IsCooling)
                {
                    m_AudioSource.Play();
                }
                else if (m_AudioSource.isPlaying
                         && (m_Weapon.CurrentAmmoRatio == 1 || !m_Weapon.IsWeaponActive || !m_Weapon.IsCooling))
                {
                    m_AudioSource.Stop();
                    return;
                }

                m_AudioSource.volume = AmmoToVolumeRatioCurve.Evaluate(1 - m_Weapon.CurrentAmmoRatio);
            }

        }

        private void UpdateVisualSmoke(float currentAmmoRatio)
        {
            m_OverheatMaterialPropertyBlock.SetEmissionColor(
                GetOverheatColorFromAmmoRatio(currentAmmoRatio));

            foreach (var data in m_OverheatingRenderersData)
            {
                data.Renderer.SetPropertyBlock(m_OverheatMaterialPropertyBlock, data.MaterialIndex);
            }

            m_SteamVfxEmissionModule.rateOverTimeMultiplier = SteamVfxEmissionRateMax * (1f - currentAmmoRatio);
        }

        private Color GetOverheatColorFromAmmoRatio(float currentAmmoRatio)
        {
            return OverheatGradient.Evaluate(1f - currentAmmoRatio);
        }
    }

    public static class OverheatExts
    {
        public static void SetEmissionColor(this MaterialPropertyBlock block, Color color)
        {
            block.SetColor("_EmissionColor", color);
        }
    }
}