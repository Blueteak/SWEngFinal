using UnityEngine;
using System.Collections;

public class FollowGrid : MonoBehaviour {

	public VectorGrid grid;

	Vector3 lastPos;

	void LateUpdate()
	{
		grid.Scroll(lastPos-transform.position);
		lastPos = transform.position;
	}
}
