﻿using UnityEngine;
using System.Collections;

public class FollowGrid : MonoBehaviour {

	public VectorGrid grid;

	Vector3 lastPos;

	void Start()
	{
		grid.m_GridWidth = Mathf.Min(Screen.width/10, 100);
		grid.m_GridHeight = Mathf.Min(Screen.height/10, 100);
	}

	void LateUpdate()
	{
		grid.Scroll(lastPos-transform.position);
		lastPos = transform.position;
	}
}
