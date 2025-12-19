using UnityEngine;
using UnityEditor;


[InitializeOnLoad]
public static class BrickMapCreator 
{
    private const float SnapStep = 0.5f;

    static BrickMapCreator()
    {
        Undo.postprocessModifications += OnPostProcessModifications;
    }

    private static UndoPropertyModification[] OnPostProcessModifications(UndoPropertyModification[] mods)
    {
        foreach(var mod in mods)
        {
            if(mod.currentValue.propertyPath == "m_LocalPosition.x" ||  mod.currentValue.propertyPath == "m_LocalPosition.y")
            {
                Transform targetTransform = mod.currentValue.target as Transform;
                if (targetTransform == null) continue;

                Vector3 snappedPosition = SnapPosition(targetTransform.position);

                if(targetTransform.localPosition != snappedPosition)
                {
                    Undo.RecordObject(targetTransform, "Snap to Grid");
                    targetTransform.localPosition = snappedPosition;
                }

            }
        }
        return mods;
    }

    private static Vector3 SnapPosition(Vector3 originPosition)
    {
        float x = Mathf.Round(originPosition.x / SnapStep) * SnapStep; 
        float y = Mathf.Round(originPosition.y / SnapStep) * SnapStep;
        float z = 0;
        return new Vector3(x, y, z);
        //float z = Mathf.Round(originPosition.x / SnapStep) * SnapStep;
    }
}
