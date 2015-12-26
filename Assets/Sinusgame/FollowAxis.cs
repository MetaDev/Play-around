using UnityEngine;
using System.Collections;

public class FollowAxis : MonoBehaviour {

    public GameObject target;
    public float xOffset = 0;
    public float yOffset = 0;
    public float zOffset = 0;
    public bool X=false;
    public bool Y = false;
    public bool Z = false;
    void LateUpdate()
    {
        float x = this.transform.position.x;
        if (X)
        {
            x = target.transform.position.x + xOffset;
        }
        float y = this.transform.position.y;
        if (Y)
        {
            y = target.transform.position.y + yOffset;
        }
        float z = this.transform.position.z;
        if (Z)
        {
            z = target.transform.position.z + zOffset;
        }
        this.transform.position = new Vector3(x, y,z);
                                             
    }
}
