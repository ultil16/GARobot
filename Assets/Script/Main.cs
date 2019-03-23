using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] obj;
    public GameObject robot;
    public static int N =200;//number of machines per generation, lessthan 800
    public static int genum=4;//the length of gene
    public static int stage = 2;//power stage of machine motor, not include 0
    //example: 2 -> -2, -1, 0, 1, 2
    public static int elitenum=10;//num of elite, <N, more than 3
    public static int remain = 0;//num of continueing elite robot, <=elitenum
    public static float mutarate = 0.1f;//mutation rate, !!! apply to each gene !!!
    public static float mutarate_large = 0.8f;//large mutation rate
    public static int mutagene_large = 20;//1 large mutation per n generation
    public static int genetypes = 4;//4 legs have...each gene -> 4 ,  same gene -> 1
    public static int[,,] mastercode = new int[N, genetypes, genum];//all codes
    public static int[,] bestcode = new int[genetypes, genum];
    public int bestscore = 0;
    public static int[,,] elitecode = new int[elitenum, genetypes, genum];//codes of elite
    public static int limit = 10;//timelimit(sec)
    public static int frame = 0;//60fps
    public static int generation = 0;//sedai
    public static float[] result = new float[N];//score
    public static int[] soeji = new int[N];
    string str;
    public void Start()
    {
        Application.targetFrameRate = 60;//fps
        obj = new GameObject[N];//objects
        robot = (GameObject)Resources.Load("Robot");//Prefab of robot
        for (int i=0;i<N;i++)
        {
            for (int j=0;j<genetypes;j++)
            {
                for (int k=0;k<genum;k++)
                {
                    mastercode[i,j,k]=Random.Range(0, stage*2+1) -stage;//create N gene random
                }
            }
        }

        /*
        if use original code
        edit mastercode here
        */
        /*
        for (int i=0;i<N;i++)
        {
            for (int j = 0; j < 4; j++)
            {
                mastercode[i, j, 0] = -2;
            }
            for (int j = 0; j < 4; j++)
            {
                mastercode[i, j, 1] = 0;
            }
            for (int j = 0; j < 4; j++)
            {
                mastercode[i, j, 2] = 2;
            }
            for (int j = 0; j < 4; j++)
            {
                mastercode[i, j, 3] = 0;
            }
        }
        */

        for (int i = 0; i < N; i++)//create object
        {
            obj[i] = Instantiate(robot, new Vector3(10 * i, 2, 0), Quaternion.identity /*Quaternion.Euler(45, 0, 0)*/);
            for (int j = 0; j < genum; j++)
            {
                obj[i].transform.Find("Leg1").GetComponent<Legspin>().code[j] = mastercode[i, 0 % genetypes, j];
                obj[i].transform.Find("Leg2").GetComponent<Legspin>().code[j] = mastercode[i, 1 % genetypes, j];
                obj[i].transform.Find("Leg3").GetComponent<Legspin>().code[j] = mastercode[i, 2 % genetypes, j];
                obj[i].transform.Find("Leg4").GetComponent<Legspin>().code[j] = mastercode[i, 3 % genetypes, j];
                obj[i].transform.Find("Leg1").GetComponent<Legspin>().oya = obj[i];
                obj[i].transform.Find("Leg2").GetComponent<Legspin>().oya = obj[i];
                obj[i].transform.Find("Leg3").GetComponent<Legspin>().oya = obj[i];
                obj[i].transform.Find("Leg4").GetComponent<Legspin>().oya = obj[i];
                obj[i].transform.Find("Leg1").GetComponent<Legspin>().machinenum = i;
                obj[i].transform.Find("Leg2").GetComponent<Legspin>().machinenum = i;
                obj[i].transform.Find("Leg3").GetComponent<Legspin>().machinenum = i;
                obj[i].transform.Find("Leg4").GetComponent<Legspin>().machinenum = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (frame==(limit*60))//time up
        {
            Evolve(frame);
            frame = 0;
        }
        else//count up
        {
            frame++;
        }
    }

    void Evolve(int checkfirst)//function to connect the next generation
    {
        /*
        Get score & Select gene
        */

        for (int i = 0; i < N; i++)
        {
            result[i] = obj[i].transform.Find("Goishi").transform.position.z;//get result
        }


        for (int i = 0; i < N; i++)
        {
            soeji[i] = i;//for sort
        }

        System.Array.Sort(result, soeji);//sort
        System.Array.Reverse(result);
        System.Array.Reverse(soeji);
        Debug.Log(generation+"gen:"+soeji[0].ToString("D3") + ": " + result[0]);//display the best score of the generation

        if (result[0] > bestscore)//update the best score of the simuration
        {
            for (int j = 0; j < genetypes; j++) 
            {
                for (int k=0;k<genum;k++)
                {
                    bestcode[j, k] = mastercode[soeji[0],j,k];
                }
            }
        }
        
        if (generation%50==0 && generation>0)//option for output
        {
            for (int j = 0; j < genetypes; j++)
            {
                str = j+": ";
                for (int k = 0; k < genum; k++)
                {
                    //str +=System.String.Format("{0,2}",mastercode[soeji[0],j,k]);
                    str += ", " + bestcode[j,k];
                }
                Debug.Log(str);
            }
        }
        

        for (int i=0;i<elitenum;i++)//record elite
        {
            for (int j=0;j<genetypes;j++)
            {
                for (int k=0;k<genum;k++)
                {
                    elitecode[i, j, k] = mastercode[soeji[i],j,k];
                }
            }
        }


        /*
        Crossing & Create children
        */

        for (int i=0;i<remain;i++)//remain & continue
        {
            for (int j=0;j<genetypes;j++)
            {
                for (int k=0;k<genum;k++)
                {
                    mastercode[i, j, k] = elitecode[i, j, k];
                }
            }
        }


        for (int i = remain; i < N; i++)
        {
            for (int j = 0; j < genetypes; j++)
            {
                for (int k = 0; k < genum; k++)
                {
                    if (((generation+1)%mutagene_large==0 && Random.Range(0.0f, 1.0f) < mutarate_large) || ((generation + 1) % mutagene_large != 0 && Random.Range(0.0f, 1.0f) < mutarate))
                    {//if next generation have large mutation, mutarate->mutarate_large 
                        mastercode[i, j, k] = Random.Range(0, stage*2+1) - stage;
                    }
                    else
                    {
                        mastercode[i, j, k] = elitecode[System.Math.Min(Random.Range(0, elitenum-1), Random.Range(0, elitenum-1)), j, k];
                    }
                }
            }
        }
        
        
        for (int i = 0; i < N; i++)//destroy object
        {
            Destroy(obj[i]);
        }
        

        for (int i = 0; i < N; i++)
        {
            obj[i] = Instantiate(robot, new Vector3(10 * i, 2, 0), Quaternion.identity /*Quaternion.Euler(45, 0, 0)*/);
            for (int j = 0; j < genum; j++)
            {
                obj[i].transform.Find("Leg1").GetComponent<Legspin>().code[j] = mastercode[i, 0 % genetypes, j];
                obj[i].transform.Find("Leg2").GetComponent<Legspin>().code[j] = mastercode[i, 1 % genetypes, j];
                obj[i].transform.Find("Leg3").GetComponent<Legspin>().code[j] = mastercode[i, 2 % genetypes, j];
                obj[i].transform.Find("Leg4").GetComponent<Legspin>().code[j] = mastercode[i, 3 % genetypes, j];
                obj[i].transform.Find("Leg1").GetComponent<Legspin>().oya = obj[i];
                obj[i].transform.Find("Leg2").GetComponent<Legspin>().oya = obj[i];
                obj[i].transform.Find("Leg3").GetComponent<Legspin>().oya = obj[i];
                obj[i].transform.Find("Leg4").GetComponent<Legspin>().oya = obj[i];
                obj[i].transform.Find("Leg1").GetComponent<Legspin>().machinenum = i;
                obj[i].transform.Find("Leg2").GetComponent<Legspin>().machinenum = i;
                obj[i].transform.Find("Leg3").GetComponent<Legspin>().machinenum = i;
                obj[i].transform.Find("Leg4").GetComponent<Legspin>().machinenum = i;
                /*format leg position -> 0 degree ---  without this, X axis plus direction -> 0 degree
                obj[i].transform.Find("Leg1").GetComponent<Legspin>().deg = 0;
                obj[i].transform.Find("Leg2").GetComponent<Legspin>().deg = 90;
                obj[i].transform.Find("Leg3").GetComponent<Legspin>().deg = 180;
                obj[i].transform.Find("Leg4").GetComponent<Legspin>().deg = 270;
                */
            }
        }
        generation++;
    }
    
}
