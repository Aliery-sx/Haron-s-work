using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoynancyObject : MonoBehaviour
{
    public Transform[] floaters;
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;

    WaterManager waterManager;
    Rigidbody rigidBody;
    int floatersUnderwater;
    bool underwater;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        waterManager = FindObjectOfType<WaterManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        floatersUnderwater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float difference = floaters[i].position.y - waterManager.WaterHeightAtPosition(floaters[i].position);
            if (difference < 0)
            {
                rigidBody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floaters[i].position, ForceMode.Force);
                if (!underwater) 
                {
                    floatersUnderwater += 1;
                    underwater = true;
                    SwitchState(true);
                }
            }
            if (underwater && floatersUnderwater == 0) 
            {
                underwater = false;
                SwitchState(false);
                
            }
        }
    }

    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            rigidBody.drag = underWaterDrag;
            rigidBody.angularDrag = underWaterAngularDrag;
        }
        else
        {
            rigidBody.drag = airDrag;
            rigidBody.angularDrag = airAngularDrag;   
        }
    }
}
