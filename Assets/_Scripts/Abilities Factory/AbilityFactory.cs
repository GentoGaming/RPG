using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace _Scripts.Abilities_Factory
{
    public static class AbilityFactory
    {
        public static readonly Dictionary<string, Type> AbilitiesByName = new Dictionary<string, Type>();
        private static bool IsInitialized => AbilitiesByName.Count>0;

        private static void InitializeFactory()
        {

            if(IsInitialized)return;
            var abilityTypes = Assembly.GetAssembly(typeof(Ability)).GetTypes().Where(mytype=>mytype.IsClass && !mytype.IsAbstract && mytype.IsSubclassOf(typeof(Ability)));

            var enumerable = abilityTypes.ToList();

            foreach (var type in enumerable)
            {
                if (ScriptableObject.CreateInstance(type) is Ability temp)
                {
                    AbilitiesByName.Add(temp.Name, type);
                }
            }
        }

        public static Ability GetAbility(string abilityType)
        {

            InitializeFactory();
            if (AbilitiesByName.ContainsKey(abilityType))
            {
                Type type = AbilitiesByName[abilityType];
                var ability = ScriptableObject.CreateInstance(type) as Ability;
                return ability;
            }

            return null;
        }

        internal static IEnumerable<string> GetAllAbilities()
        {
            InitializeFactory();
            return AbilitiesByName.Keys;
        }
    }
}