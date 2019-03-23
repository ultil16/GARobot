using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legspin : MonoBehaviour
{
    // Start is called before the first frame update
    int frame = 0;
    float x = 0, z = 0, y = 0, r = 0;
    public int[] code = new int[Main.genum];
    HingeJoint joint;//hinge joint
    JointMotor motor;//1 motor per 1 leg
    public int mode = 0;//state: body -> leg direction
    public int machinenum = 0;
    public float deg = 0;//option
    public GameObject oya;
    void Start()
    {
        oya = new GameObject();
        joint = GetComponent<HingeJoint>();
        motor = joint.motor;
        motor.freeSpin = false;//use force to stop the motor
        motor.targetVelocity = 0;
        //motor.force = 10000000;//power of motor, default is infinity(3.4e+38)
        joint.motor = motor;
    }

    // Update is called once per frame
    void Update()
    {
        joint.motor = motor;
        if (frame%6==0)//change motor order 60/n times per second
        {
            x = transform.position.x-oya.transform.position.x - 10 * machinenum;
            z = transform.position.z - oya.transform.position.z;
            y = transform.position.y;
            r = Mathf.Atan2(x,z)*Mathf.Rad2Deg;//body -> leg direction
            r += deg-(180/Main.genum);
            if (r<0)
            {
                r += 360;
            }
            r = r % 360;
            mode = (int)Mathf.Floor(r/(360/Main.genum));
            motor.targetVelocity = code[mode%Main.genum] * 720;
            
            //motor.targetVelocity = code[(frame / 20)%30] * 180;//move by frame
            //motor.targetVelocity =  (Random.Range(0, 5) - 2) * 180;
        }
        /*
        if (frame%180==0)
        {
            Debug.Log(machinenum+": "+x+" "+z);
        }
        */
        
        frame++;
    }
}
