using UnityEngine;

namespace Scriptables
{
    public enum MonsterType
    {
        GHOST,
        POLTERGEIST,
        ZOMBIE
    }

    [CreateAssetMenu(fileName = "new GhostStatsSO", menuName = "Scriptables/GhostsSO/GhostStatsSO", order = 2)]
    public class GhostStatsSO: ScriptableObject
    {
        public float MaxHealth;
        public float MaxVeil;
        public float VeilRegen;
        public float VeilRegenCD;
        public float MoveSpeed;
        public float StunDuration;
        public int AttackDamage;
        public float AttackRange;
        public float AttackRadius;
        public float AttackCD;
        public int InteractionRange;
        public int InteractionCD;
        public string AttackKey;
        public MonsterType Key;
    }   
}