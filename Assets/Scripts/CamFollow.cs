using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject player;
    public UI cameraScript;

    [Header("Settings")]
    public float easing = 1.0f;

    [Header("Other")]
    public float camX;
    public float camZ;
    public Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        cameraScript = GameObject.Find("Main Camera").GetComponent<UI>();
        /*
        this.transform.position = new Vector3()
        {
            x = this.player.transform.position.x,
            y = this.transform.position.y,
            z = this.player.transform.position.z - 20,
        };
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && cameraScript.gameState == 1)
        {
            Vector3 target = new Vector3()
            {
                x = this.player.transform.position.x,
                y = this.transform.position.y,
                z = this.player.transform.position.z - 20,
            };

            Vector3 pos = Vector3.Lerp(this.transform.position, target, easing * Time.deltaTime);

            this.transform.position = pos;
        }
    }

}
