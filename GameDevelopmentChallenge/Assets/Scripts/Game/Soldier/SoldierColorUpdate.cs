using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierColorUpdate : MonoBehaviour
{
    [SerializeField] List<Renderer> renderers;
    [SerializeField] Material playerMaterial;
    [SerializeField] Material enemy1Material;
    [SerializeField] Material enemy2Material;



    public void ColorUpdate(EnumArmyType armyType)
    {
        Material material = null;
        switch (armyType)
        {
            case EnumArmyType.player:
                material = playerMaterial;
                break;
            case EnumArmyType.enemy:
                material = enemy1Material;
                break;
            case EnumArmyType.enemy2:
                material = enemy2Material;
                break;
        }

        foreach (var item in renderers)
        {
            Material[] tempMaterials = item.sharedMaterials;

            tempMaterials[0] = material;

            item.sharedMaterials = tempMaterials;
        }
        
    }

}
