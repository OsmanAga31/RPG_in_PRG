using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatator : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private float changeInterval = 0.075f;
    private float localChangeInterval;
    private float localChangeInterval2;

    // Start is called before the first frame update
    void Start()
    {
        localChangeInterval = 0.0f;
        localChangeInterval2 = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

        localChangeInterval += Time.deltaTime;

        if (localChangeInterval < changeInterval) return;

        localChangeInterval -= changeInterval;

        cube.transform.localPosition = new Vector3(Random.Range(-3f, 3f), Random.Range(-1.5f, 1.5f), Random.Range(-5f, 0f));
        
        localChangeInterval2 += Time.deltaTime;
        if (localChangeInterval2 < changeInterval*1.5) return;
        localChangeInterval2 -= changeInterval;
        cube.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
    }
}
