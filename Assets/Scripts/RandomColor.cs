using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomColor : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private float changeInterval = 0.075f;
    private float localChangeInterval;

    // Start is called before the first frame update
    void Start()
    {
        localChangeInterval = 0.0f;
        m_image.color = new Color(Random.value, Random.value, Random.value);
    }

    // Update is called once per frame
    void Update()
    {
        localChangeInterval += Time.deltaTime;

        if (localChangeInterval < changeInterval) return;

        localChangeInterval -= changeInterval;
        m_image.color = new Color(Random.value, Random.value, Random.value);
        m_image.rectTransform.localScale = new Vector3(Random.value, Random.value, Random.value);
        m_image.rectTransform.localRotation = new Quaternion(Random.value, Random.value, Random.value, Random.value);
        //Random.Range(-100, 100);

    }
}
