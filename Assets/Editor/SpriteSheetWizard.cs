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
        animInfos.Add(new AnimInfo() { name = "IdleN", frames = new int[] {0} });
        animInfos.Add(new AnimInfo() { name = "IdleE", frames = new int[] {8} });
        animInfos.Add(new AnimInfo() { name = "IdleS", frames = new int[] {16} });
        animInfos.Add(new AnimInfo() { name = "IdleW", frames = new int[] {24} });
        animInfos.Add(new AnimInfo() { name = "WalkN", frames = new int[] {0,1,2,3} });
        animInfos.Add(new AnimInfo() { name = "WalkE", frames = new int[] {8,9,10,11} });
        animInfos.Add(new AnimInfo() { name = "WalkS", frames = new int[] {16,17,18,19} });
        animInfos.Add(new AnimInfo() { name = "WalkW", frames = new int[] {24,25,26,27} });
        animInfos.Add(new AnimInfo() { name = "AttackN", frames = new int[] {6,5,4} });
        animInfos.Add(new AnimInfo() { name = "AttackE", frames = new int[] {14,13,12} });
        animInfos.Add(new AnimInfo() { name = "AttackS", frames = new int[] {22,21,20} });
        animInfos.Add(new AnimInfo() { name = "AttackW", frames = new int[] {30,29,28} });
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
