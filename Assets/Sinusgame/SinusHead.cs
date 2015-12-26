using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;

public class SinusHead : MonoBehaviour
{

    //TODO reuse tail objects instead of destroying them
    float t = 0;
    float A = 1;
    float B = 1;
    float ConstantSpeed=0.1f;
    float ChangementSpeed = 0.02f;
    public Vector3 TailMaxOffset;
    private List<GameObject> TailObjects=new List<GameObject>();
    public GameObject tailPrefab;
    // Use this for initialization
    void Start()
    {
        var update = this.gameObject.FixedUpdateAsObservable();
        //input and movement is every fixed frame
        update.Subscribe((knie) => {
            //make sinus movement
            t += ConstantSpeed;
            this.gameObject.transform.position = new Vector3(t, (float)(A * Math.Sin(B * t)), 0);

            //handle input
            if (Input.GetKey("up"))
            {
                A += ChangementSpeed;
                //B += 0.1f;
            }

            if (Input.GetKey("down"))
            {
                A -= ChangementSpeed;
                //B -= 0.1f;
            }
        }).AddTo(this);
        //draw tail with spheres and colliders
        //drop collider every so often to simulate collision
        update.Buffer(TimeSpan.FromMilliseconds(100)).Subscribe((knie) => {
            var sphere = Instantiate(tailPrefab);
            TailObjects.Add(sphere);
            sphere.transform.position = this.transform.position;
            sphere.FixedUpdateAsObservable().Buffer(TimeSpan.FromMilliseconds(100)).Subscribe(t =>
            {
                float offsetX = Math.Abs(sphere.transform.position.x - this.transform.position.x);
                float offsetY = Math.Abs(sphere.transform.position.y - this.transform.position.y);
                float offsetZ = Math.Abs(sphere.transform.position.z - this.transform.position.z);
                if (offsetX > TailMaxOffset.x || offsetY > TailMaxOffset.y || offsetZ > TailMaxOffset.z)
                {
                    GameObject.Destroy(sphere);
                    TailObjects.Remove(sphere);
                }
            }).AddTo(this);
            //trigger on collider
            sphere.OnCollisionEnterAsObservable().Subscribe((collision) =>
            {
                Debug.Log(collision.impulse);
            });

        }).AddTo(this);
        //clean up after exit
        Observable.OnceApplicationQuit().Subscribe((exit)=> {
            foreach(GameObject tailObject in TailObjects)
            {
                if (tailObject != null)
                {
                    GameObject.DestroyImmediate(tailObject);
                }
            }
        });
    }

   


}
