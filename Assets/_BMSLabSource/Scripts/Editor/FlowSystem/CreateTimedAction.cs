using _BMSLabSource.Scripts.Runtime.FlowSystem;
using UnityEditor;
using UnityEngine;

namespace _BMSLabSource.Scripts.Editor.FlowSystem
{
    public class CreateTimedAction
    {
        [MenuItem("GameObject/Flow System/Actions/Timed Action", false, 0)]
        static void CreateBasicActionObject(MenuCommand menuCommand)
        {
            GameObject timedActionObject = new("TimedAction");
            timedActionObject.AddComponent<TimedAction>();

            GameObjectUtility.SetParentAndAlign(timedActionObject, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(timedActionObject, "Create Timed Action");

            Selection.activeObject = timedActionObject;
        }
    }
}