using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public class CharactersStats : MonoBehaviour
    {
        [Header("Health")]
        public int maxHealth;
        public int currentHealth;
        public bool isDead;

        [Header("Weapon")]
        public int weaponDamage;
    }
}