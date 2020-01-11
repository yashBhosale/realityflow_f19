﻿using Assets.RealityFlow.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A new editor window to handle changing a unity object's parameters
/// </summary>
public class ObjectSettings : EditorWindow
{
    static ObjectSettings window;

    // TODO: What does this do?
    public static void OpenWindow()
    {
        window = (ObjectSettings)GetWindow(typeof(ObjectSettings));
        window.minSize = new Vector2(200, 200);
        window.Show();
    }

    private void OnGUI()
    {
        DrawSettings((ObjectData)RealityFlowWindow.ObjectInfo);
    }

    void DrawSettings(ObjectData objData)
    {

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        objData.objectName = EditorGUILayout.TextField(objData.objectName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Mesh");
        objData.mesh = (Mesh)EditorGUILayout.ObjectField(objData.mesh, typeof(Mesh), false);
        EditorGUILayout.EndHorizontal();

        // Textures are currently not supported on the live server due to lag issues
        //-----------------------------------------------------------------------------------------------------
        // EditorGUILayout.BeginHorizontal();
        // GUILayout.Label("Texture");
        // objData.texture = (Texture2D)EditorGUILayout.ObjectField(objData.texture, typeof(Texture2D), false);
        // EditorGUILayout.EndHorizontal();
        //-----------------------------------------------------------------------------------------------------

        objData.position = EditorGUILayout.Vector3Field("Position", objData.position);

        objData.rotation = EditorGUILayout.Vector3Field("Rotation", objData.rotation);

        objData.scale = EditorGUILayout.Vector3Field("Scale", objData.scale);

        objData.color = EditorGUILayout.ColorField("Color", objData.color);

        if (objData.mesh == null)
        {
            EditorGUILayout.HelpBox("This object needs a [Mesh] before it can be created.", MessageType.Warning);
        }
        else if (objData.objectName == null || objData.objectName.Equals(""))
        {
            EditorGUILayout.HelpBox("This object needs a [Name] before it can be created.", MessageType.Warning);
        }
        // TODO: Add extra else if case to check if the name already exists in the project
        else
        {
            if (GUILayout.Button("Create", GUILayout.Height(30)))
            {
                SaveObjectData(objData);
                window.Close();
            }
        }
    }

    void SaveObjectData(ObjectData objData)
    {
        string prefabPath; //path to the base prefab
        string s = "ObjManager";

        // Check if there is already an object manager
        // if not, create one
        GameObject manager = GameObject.FindGameObjectWithTag(s);
        bool managerExists = true;

        if (manager == null)
        {
            manager = new GameObject("ObjManager");
            manager.tag = s;
            managerExists = false;
        }

        // If the manager was just created add the necessary components
        if (managerExists == false)
        {
            manager.AddComponent(typeof(ObjectManager));
            manager.AddComponent(typeof(FlowNetworkManager));
            manager.GetComponent<FlowNetworkManager>().LocalServer = Config.LOCAL_HOST;
            manager.GetComponent<FlowNetworkManager>().mainGameCamera = GameObject.FindGameObjectWithTag("MainCamera");
            manager.AddComponent(typeof(DoOnMainThread));
        }

        FlowTObject obj = new FlowTObject();
        obj.vertices = RealityFlowWindow.ObjectInfo.mesh.vertices;
        obj.uv = RealityFlowWindow.ObjectInfo.mesh.uv;
        obj.triangles = RealityFlowWindow.ObjectInfo.mesh.triangles;
        obj.x = RealityFlowWindow.ObjectInfo.position.x;
        obj.y = RealityFlowWindow.ObjectInfo.position.y;
        obj.z = RealityFlowWindow.ObjectInfo.position.z;
        Quaternion rot = Quaternion.Euler(RealityFlowWindow.ObjectInfo.rotation);
        obj.q_x = rot.x;
        obj.q_y = rot.y;
        obj.q_z = rot.z;
        obj.q_w = rot.w;
        obj.s_x = RealityFlowWindow.ObjectInfo.scale.x;
        obj.s_y = RealityFlowWindow.ObjectInfo.scale.y;
        obj.s_z = RealityFlowWindow.ObjectInfo.scale.z;
        obj.type = "BoxCollider";
        obj.name = RealityFlowWindow.ObjectInfo.objectName;
        obj.color = RealityFlowWindow.ObjectInfo.color;

        // Textures are currently not supported on the live server due to lag
        //-------------------------------------------------------------------------
        // obj.texture = RealityFlowWindow.ObjectInfo.texture.GetRawTextureData();
        // obj.textureHeight = RealityFlowWindow.ObjectInfo.texture.height;
        // obj.textureWidth = RealityFlowWindow.ObjectInfo.texture.width;
        // obj.textureFormat = (int)RealityFlowWindow.ObjectInfo.texture.format;
        // obj.mipmapCount = RealityFlowWindow.ObjectInfo.texture.mipmapCount;
        //-------------------------------------------------------------------------

        ObjectCreationEvent createObject = new ObjectCreationEvent();
        createObject.Send(obj);
    }
}
