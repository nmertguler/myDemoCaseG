using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ColorChanger : MonoBehaviour
{

    [Space(5)]
    [SerializeField] List<ClassMaterialVariables> materialVariables;

    [Space(5)]
    [SerializeField] List<ClassColorChanger> colorChangeVariables;

    [Serializable]
    public class ClassColorChanger
    {
        public Renderer meshRenderer;
        public int materialIndex;
    }

    [Serializable]
    public class ClassMaterialVariables
    {
        public string materialName;
        public Material material;
    }


    public void ColorChangeFunc(string materialName)
    {
        ClassMaterialVariables cmv = materialVariables.Find(a => a.materialName == materialName);
        Material material = cmv.material;

        for (int i = 0; i < colorChangeVariables.Count; i++)
        {
            Renderer renderer = colorChangeVariables[i].meshRenderer;
            
            Material[] tempMaterials = renderer.sharedMaterials;

            tempMaterials[colorChangeVariables[i].materialIndex] = material;
            renderer.sharedMaterials = tempMaterials;
        }
    }
}
