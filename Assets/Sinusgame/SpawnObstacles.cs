using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections.Generic;

public class SpawnObstacles : MonoBehaviour
{
    public Vector3 SpawnStartOffset;
    public Vector3 SpawnMaxOffset;
    private List<GameObject> ObstacleObjects=new List<GameObject>();
    public GameObject obstaclePrefab;
    // Use this for initialization
    void Start()
    {
        MathNet.Numerics.Random.CryptoRandomSource rn = new MathNet.Numerics.Random.CryptoRandomSource();
        this.gameObject.FixedUpdateAsObservable().Buffer(TimeSpan.FromMilliseconds(100)).Subscribe(x =>
        {
            if (rn.NextDouble() > 0.9d)
            {
                //set position
                GameObject cube = Instantiate(obstaclePrefab);
                ObstacleObjects.Add(cube);
                cube.transform.position = new Vector3(this.transform.position.x + SpawnStartOffset.x, this.transform.position.y + SpawnStartOffset.y, this.transform.position.z + SpawnStartOffset.z);
                //set color
                cube.GetComponent<Renderer>().material.color = Color.cyan;
                // destroy if out of bounds
                cube.FixedUpdateAsObservable().Buffer(TimeSpan.FromMilliseconds(100)).Subscribe(t =>
                {
                    float offsetX=Math.Abs(cube.transform.position.x - this.transform.position.x);
                    float offsetY = Math.Abs(cube.transform.position.y - this.transform.position.y);
                    float offsetZ = Math.Abs(cube.transform.position.z - this.transform.position.z);
                    if (offsetX>SpawnMaxOffset.x || offsetY>SpawnMaxOffset.y || offsetZ> SpawnMaxOffset.z)
                    {
                        GameObject.Destroy(cube);
                        ObstacleObjects.Remove(cube);
                    }
                }).AddTo(this);
            }

        }).AddTo(this);
        //clean up after exit
        Observable.OnceApplicationQuit().Subscribe((exit) => {
            foreach (GameObject obstacleObject in ObstacleObjects)
            {
                if (obstacleObject != null)
                {
                    GameObject.DestroyImmediate(obstacleObject);
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
