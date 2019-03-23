using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    // Start is called before the first frame update
    public static Text InfoText;
    int frame = 0;
    string score = "";
    string[] str = new string[Main.genetypes];
    void Start()
    {
        InfoText = GetComponent<Text>();
        InfoText.text = "success";
    }

    // Update is called once per frame
    void Update()
    {
        InfoText.text = "第" + Main.generation + "世代   " + (600-frame) + "\n" + "score: " + score + "\n";
        for (int j=0; j<Main.genetypes; j++)
        {
            InfoText.text += str[j]+"\n";
        }
        if (frame==Main.limit*60)
        {
            frame = 0;
        }
        else
        {
            if (frame==1)
            {
                score = Main.result[0].ToString("F3");
                for (int j = 0; j < Main.genetypes; j++)
                {
                    str[j] = j + ": ";
                    for (int k = 0; k < Main.genum; k++)
                    {
                        if (Main.elitecode[0, j, k]>=0)
                        {
                            str[j] += " ";
                        }
                        str[j] += Main.elitecode[0, j, k];
                        if (k + 1 < Main.genum)
                        {
                            str[j] += ",";
                        }
                    }
                }
            }
            frame++;
        }
    }
}
