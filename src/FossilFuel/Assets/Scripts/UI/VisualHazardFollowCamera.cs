using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualHazardFollowCamera : MonoBehaviour
{
    Transform mainCamTrans;

    // Start is called before the first frame update
    void Start()
    {
        mainCamTrans = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(mainCamTrans.position.x, -4f, mainCamTrans.position.z) + (mainCamTrans.forward * 0.6f);

        this.transform.rotation = Quaternion.Euler(0f, mainCamTrans.rotation.eulerAngles.y, 0f);
    }
}
