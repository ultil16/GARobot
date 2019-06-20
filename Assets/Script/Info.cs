using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;

public class Info : MonoBehaviour
{
    // Start is called before the first frame update
    public static Text InfoText;
    public static Text InfoGene;
    int frame = 0;
    string score = "";
    double bestscore = 0.0;
    string bestgene = "";
    string[] str = new string[Main.genetypes];
    void Start()
    {
        InfoText = GetComponent<Text>();
        InfoText.text = "success";
        File.WriteAllText(@".\Assets\Result\log.txt", "score log\n");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InfoText.text = "第" + Main.generation + "世代   " + (600-frame) + "\n" + "score: " + score + "\n";
        for (int j=0; j<Main.genetypes; j++)
        {
            InfoText.text += str[j] + "\n";
        }
        if (frame==Main.limit*60)
        {
            frame = 0;
        }
        else
        {
            if (frame==1)
            {
                score = Main.result[0].ToString("F3").PadLeft(7, ' ');
                File.AppendAllText(@".\Assets\Result\log.txt", score + "    ");
                for (int i = 0; i < Main.result[0]; i += 5)
                {
                    File.AppendAllText(@".\Assets\Result\log.txt", "x");
                }
                File.AppendAllText(@".\Assets\Result\log.txt", "\n");
                if (Main.result[0] > bestscore)
                {
                    bestscore = Main.result[0];
                    bestgene = "";
                    for (int j = 0; j < Main.genetypes; j++)
                    {
                        for (int k = 0; k < Main.genum; k++)
                        {
                            bestgene += Main.elitecode[0, j, k] + "\n";
                        }
                    }
                    bestgene += score + "\n";
                    File.WriteAllText(@".\Assets\Result\gen.txt", bestgene);
                }
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
