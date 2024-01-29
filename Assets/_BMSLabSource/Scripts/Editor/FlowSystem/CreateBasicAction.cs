using _BMSLabSource.Scripts.Runtime.FlowSystem;
using UnityEditor;
using UnityEngine;

namespace _BMSLabSource.Scripts.Editor.FlowSystem
{
    public class CreateBasicAction
    {
        [MenuItem("GameObject/Flow System/Actions/Basic Action", false, 0)]
        static void CreateBasicActionObject(MenuCommand menuCommand)
        {
            GameObject basicActionObject = new("BasicAction");
            basicActionObject.AddComponent<BasicAction>();

            GameObjectUtility.SetParentAndAlign(basicActionObject, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(basicActionObject, "Create Basic Action");

            Selection.activeObject = basicActionObject;
        }
    }
}