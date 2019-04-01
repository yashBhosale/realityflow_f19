﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class FlowObject : MonoBehaviour {

	public bool selected = false;
	public FlowTransform ft;
	FlowTransformCommand cmd = new FlowTransformCommand();
	
	// Use this for initialization
	public void Start () {
	ft = new FlowTransform(gameObject);
	ft.id = "1";
	ft._id = "1";
	cmd.transform = ft;
	FlowObject.fo = this;
	//ft.RegisterTransform();
	}
	public static FlowObject fo;
	public static void registerObject() {
		fo.ft.RegisterTransform();
	}
	
	// Update is called once per frame
	public void Update () {
		if (selected)
		{
			if(FlowNetworkManager.connection_established) 
			{
				((FlowTransform)cmd.transform).Read(gameObject);
				CommandProcessor.sendCommand(cmd);
			}
		}
	}
}