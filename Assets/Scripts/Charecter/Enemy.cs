using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	#region Editor

	[Range(0.1f, 10.0f)]
	public float upEdge;
	[Range(0.1f, 10.0f)]
	public float bottomEdge;
	[Range(0.1f, 10.0f)]
	public float bottomRange;
	[Range(0.1f, 10.0f)]
	public float forwardRange;
	[Range(0.1f, 10.0f)]
	public float bottomRight;

	#endregion

	#region 移动机制

	RaycastHit2D bottomHit, bottomRightHit, upHit;

	private bool IsGround()
	{
		bool flag = false;

		if (bottomRightHit && bottomRightHit.collider.tag == "Lift")
			flag = true;
		if (bottomHit && bottomHit.collider.tag == "Untagged")
			flag = true;

		return flag;
	}

	#endregion

	private enum Direction
	{
		right,
		left
	};
	Direction direction;

	Vector3 Forward
	{
		get
		{
			return (direction == Direction.right) ? Vector3.right : Vector3.left;
		}
	}

	void Start()
    {
		    
    }

    void Update()
    {
		//发射射线
		Vector3 bottomPos = new Vector3(0, -bottomEdge) + transform.position;
		Vector3 upPos = new Vector3(0, upEdge) + transform.position;
		bottomHit = Physics2D.Raycast(bottomPos, Vector3.down, bottomRange);
		bottomRightHit = Physics2D.Raycast(bottomPos + Forward * bottomRight, Vector3.down, bottomRange);
		upHit = Physics2D.Raycast(upPos, Forward, forwardRange);

		if (IsGround())
		{
			//校准脚下坐标
			if (bottomRightHit && bottomRightHit.collider.tag == "Lift")
			{

			}
			else if (bottomHit && bottomHit.collider.tag == "Untagged")
			{
				
			}
		}

    }

	private void OnDrawGizmos()
	{
		Vector3 bottomPos = new Vector3(0, -bottomEdge) + transform.position;
		Vector3 upPos = new Vector3(0, upEdge) + transform.position;
		Gizmos.color = Color.red;

		Gizmos.DrawLine(bottomPos + bottomRight * Vector3.right, bottomPos + bottomRight * Forward + Vector3.down * bottomRange);
		Gizmos.DrawLine(bottomPos, bottomPos + Vector3.down * bottomRange);
		Gizmos.DrawLine(upPos, upPos + Forward * forwardRange);
	}

	

}
