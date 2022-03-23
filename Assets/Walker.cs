//Bryan Alexis Monroy Alvarez A01026848
//Georgina Alejandra Gámez Melgar A01656818
//Mauricio Acosta Hernández A01339392
//Pedro Demian Godinez Cruz A01657025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    Vector3[] ApplyTransform(Vector3[] verts, Matrix4x4 m)
    {
        int number = verts.Length;
        Vector3[] result = new Vector3[number];
        for (int i = 0; i < number; i++)
        {
            Vector3 v = verts[i];
            Vector4 temp = new Vector4(v.x, v.y, v.z, 1);
            result[i] = m * temp;
        }
        return result;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject armL = new GameObject();
        GameObject armR = new GameObject();
        GameObject legL = new GameObject();
        GameObject legR = new GameObject();
        GameObject torso = new GameObject();
        torso.AddComponent<Torso>();
        armL.AddComponent<Arm>();
        armR.AddComponent<Arm>();
        legL.AddComponent<Leg>();
        legR.AddComponent<Leg>();

        legR.GetComponent<Leg>().Init(true);
        legL.GetComponent<Leg>().Init(false);
        armR.GetComponent<Arm>().Init(true);
        armL.GetComponent<Arm>().Init(false);
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}