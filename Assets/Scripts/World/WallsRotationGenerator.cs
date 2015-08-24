using UnityEngine;
using System.Collections;

public class WallsRotationGenerator : MonoBehaviour
{
    
	
	void Start ()
    {
	    for (int i = 0; i < transform.childCount; i++)
        {
            Transform wall = transform.GetChild(i);
            float rotation = 0f;
            if (Random.value > 0.5f)
            {
                rotation = 90f;
            }
            wall.rotation = Quaternion.Euler(new Vector3(wall.eulerAngles.x, rotation, wall.eulerAngles.z));
        }
	}


    void Update()
    {
	
	}
}
