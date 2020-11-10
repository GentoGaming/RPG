using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Abilities_Factory
{

    public abstract class Ability : ScriptableObject
    {
        public abstract string Name { get; }
        public abstract void Process(GameObject selectedNpc, GameObject targetNpc, AbilityInfo abilityInfo);
    }
        
  
        
    
        

        
   
        
   
        

        
        

        
        
        
        
        
    
}
