using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Object = UnityEngine.Object;

public class SpriteSheetWizard : ScriptableWizard
{
    [MenuItem("Tools/Sprite Sheet Wizard")]
    static private void Display()
    {
        ScriptableWizard.DisplayWizard<SpriteSheetWizard>("Sprite Sheet Wizard", "Reset", "Create Clip");
    }

    [MenuItem("Tools/Log Serialized Properties")]
    static private void LogSerializedProperties()
    {
        List<Object> allObjects = new List<Object>();
        
        foreach(Object selected in Selection.objects)
        {
            allObjects.Add(selected);
            if(selected is GameObject)
            {
                allObjects.AddRange((selected as GameObject).GetComponents<Component>());
            }
        }

        foreach(Object clip in allObjects)
        {
            SerializedObject sObj = new SerializedObject(clip);
            SerializedProperty sProp = sObj.GetIterator();
            while(sProp.Next(true))
            {
                Debug.Log(clip + " => " + sProp.propertyPath);
            }
        }
    }

    [MenuItem("Tools/Create sprite animation clip from selection")]
    static private void CreateSpriteAnimationClipFromSelection()
    {
        CreateClip("New Clip", Selection.objects.ToList().OfType<Sprite>().ToArray());
    }

    static private void CreateClip(string clipName, Sprite[] sprites)
    {
        AnimationClip animClip = new AnimationClip();

        // First you need to create e Editor Curve Binding
        EditorCurveBinding curveBinding = new EditorCurveBinding();

        // I want to change the sprites of the sprite renderer, so I put the typeof(SpriteRenderer) as the binding type.
        curveBinding.type = typeof(SpriteRenderer);
        // Regular path to the gameobject that will be changed (empty string means root)
        curveBinding.path = "";
        // This is the property name to change the sprite of a sprite renderer
        curveBinding.propertyName = "m_Sprite";

        // An array to hold the object keyframes
        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            keyFrames[i] = new ObjectReferenceKeyframe();
            // set the time
            keyFrames[i].time = (float)i * 1f / animClip.frameRate;
            // set reference for the sprite you want
            keyFrames[i].value = sprites[i];

        }

        AnimationUtility.SetObjectReferenceCurve(animClip, curveBinding, keyFrames);

        AssetDatabase.CreateAsset(animClip, $"Assets/{clipName}.anim");
        AssetDatabase.SaveAssets();
    }

    [System.Serializable]
    public struct AnimInfo
    {
        public string name;
        public int[] frames;
    }

    public List<AnimInfo> animInfos = new List<AnimInfo>();
    public string namePrefix;

    private void OnEnable()
    {
        animInfos.Clear();
        animInfos.Add(new AnimInfo() { name = "IdleN", frames = new int[] {249} });
        animInfos.Add(new AnimInfo() { name = "IdleE", frames = new int[] {236} });
        animInfos.Add(new AnimInfo() { name = "IdleS", frames = new int[] {262} });
        animInfos.Add(new AnimInfo() { name = "IdleW", frames = new int[] {275} });
        animInfos.Add(new AnimInfo() { name = "WalkN", frames = new int[] {249,250,251,252,253,254,255,256,257,258,259,260,261} });
        animInfos.Add(new AnimInfo() { name = "WalkE", frames = new int[] {236,237,238,239,240,241,242,243,244,245,246,247,248} });
        animInfos.Add(new AnimInfo() { name = "WalkS", frames = new int[] {262,263,264,265,266,267,268,269,270,271,272,273,274} });
        animInfos.Add(new AnimInfo() { name = "WalkW", frames = new int[] {275,276,277,278,279,280,281,282,283,284,285,286,287} });
        animInfos.Add(new AnimInfo() { name = "AttackN", frames = new int[] {21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41} });
        animInfos.Add(new AnimInfo() { name = "AttackE", frames = new int[] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20} });
        animInfos.Add(new AnimInfo() { name = "AttackS", frames = new int[] {42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62} });
        animInfos.Add(new AnimInfo() { name = "AttackW", frames = new int[] {63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83} });
        animInfos.Add(new AnimInfo() { name = "DeathN", frames = new int[] {103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121} });
        animInfos.Add(new AnimInfo() { name = "DeathE", frames = new int[] {84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102} });
        animInfos.Add(new AnimInfo() { name = "DeathS", frames = new int[] {122,123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140} });
        animInfos.Add(new AnimInfo() { name = "DeathW", frames = new int[] {141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159} });
        animInfos.Add(new AnimInfo() { name = "RageN", frames = new int[] {179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194,195,196,197} });
        animInfos.Add(new AnimInfo() { name = "RageE", frames = new int[] {160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178} });
        animInfos.Add(new AnimInfo() { name = "RageS", frames = new int[] {198,199,200,201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216} });
        animInfos.Add(new AnimInfo() { name = "RageW", frames = new int[] {217,218,219,220,221,222,223,224,225,226,227,228,229,230,231,232,233,234,235} });
    }

    private void OnWizardCreate()
    {
        Close();
        Display();
    }

    private void OnWizardOtherButton()
    {
        Sprite[] sprites = Selection.objects.ToList().OfType<Sprite>().ToArray();
        foreach(AnimInfo animInfo in animInfos)
        {
            List<Sprite> sps = new List<Sprite>();
            foreach(int f in animInfo.frames)
            {
                sps.Add(sprites[f]);
            }

            CreateClip(namePrefix + animInfo.name, sps.ToArray());
        }
    }
}
