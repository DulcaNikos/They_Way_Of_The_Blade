using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum GameAudioClip
    {
        Fire,
        footstep1,
        footstep2,
        footstep3,
        footstep4,
        footstepGravel1,
        footstepGravel2,
        footstepGravel3,
        footstepGravel4,
        MoveCloth1,
        MoveCloth2,
        MoveCloth3,
        MoveWoosh1,
        MoveWoosh2,
        punchLayerShield1,
        punchLayerShield2,
        punchLayerTarget,
        river,
        swordHit1,
        swordHit2,
        swordHit3,
        swordHit4,
        swordHit5,
        swordHitWood1,
        swordHitWood2
    }
    public static SoundManager instance;
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private AudioSource ambienceSource2;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void PlaySound(GameAudioClip clip)
    {
        effectSource.PlayOneShot(audioClips[(int)clip]);
    }
    public void PlayFootStepSound()
    {
        int footSteps = UnityEngine.Random.Range(1, 4);
        PlaySound((GameAudioClip)footSteps);
    }
    public void PlayFootStepGravel()
    {
        int gravelFootSteps = UnityEngine.Random.Range(5, 8);
        PlaySound((GameAudioClip)gravelFootSteps);
    }
    public void PlayArmorSound()
    {
        int armorSounds = UnityEngine.Random.Range(9, 11);
        PlaySound((GameAudioClip)armorSounds);
    }
    public void PlayMissSound()
    {
        int missSound = UnityEngine.Random.Range(12, 13);
        PlaySound((GameAudioClip)missSound);
    }
    public void PlayShieldHitSound()
    {
        int shieldHitSound = UnityEngine.Random.Range(14, 15);
        PlaySound((GameAudioClip)shieldHitSound);
    }
    public void HitTarget()
    {
        int hitTargetSound = 16;
        PlaySound((GameAudioClip)hitTargetSound);
    }
    public void PlaySwordHit()
    {
        int swordHit = UnityEngine.Random.Range(18, 22);
        PlaySound((GameAudioClip)swordHit);
    }
    public void PlayWoodHitSound()
    {
        int woodHit = UnityEngine.Random.Range(23, 24);
        PlaySound((GameAudioClip)woodHit);
    }
}
