using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    public enum PARTS
    {
        SHOULDER, ARM, ELBOW, FOREARM, HAND
    }

    List<GameObject> parts;
	List<Vector3[]> originals;
	List<Vector3> sizes;
	List<Vector3> places;

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
        parts = new List<GameObject>();
        originals = new List<Vector3[]>();
        sizes = new List<Vector3>();
        places = new List<Vector3>();

        // shoulder
        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.SHOULDER].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(0.1f, .3f, 0.2f));
        places.Add(new Vector3(0, 0, 0));

        // arm
        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.ARM].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(0.3f, 1f, 0.2f));
        places.Add(new Vector3(.2f, -.35f, 0));

        // elbow
        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.ELBOW].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(0.4f, .2f, 0.2f));
        places.Add(new Vector3(0, -.6f, 0));

        // forearm
        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.FOREARM].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(0.3f, 1f, 0.2f));
        places.Add(new Vector3(0, -.6f, 0));

        // hand
        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.HAND].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(0.4f, .3f, 0.2f));
        places.Add(new Vector3(0, -.65f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        // shoulder
        List<Matrix4x4> matrices = new List<Matrix4x4>();
		Matrix4x4 tShoulder = Transformations.TranslateM(places[(int)PARTS.SHOULDER].x, places[(int)PARTS.SHOULDER].y, places[(int)PARTS.SHOULDER].z);
		Matrix4x4 sShoulder = Transformations.ScaleM(sizes[(int)PARTS.SHOULDER].x, sizes[(int)PARTS.SHOULDER].y, sizes[(int)PARTS.SHOULDER].z);
		matrices.Add(tShoulder * sShoulder);

        // arm
		Matrix4x4 tArm = Transformations.TranslateM(places[(int)PARTS.ARM].x, places[(int)PARTS.ARM].y, places[(int)PARTS.ARM].z);
		Matrix4x4 sArm = Transformations.ScaleM(sizes[(int)PARTS.ARM].x, sizes[(int)PARTS.ARM].y, sizes[(int)PARTS.ARM].z);
		matrices.Add(tShoulder * tArm * sArm);

        // elbow
		Matrix4x4 tElbow = Transformations.TranslateM(places[(int)PARTS.ELBOW].x, places[(int)PARTS.ELBOW].y, places[(int)PARTS.ELBOW].z);
		Matrix4x4 sElbow = Transformations.ScaleM(sizes[(int)PARTS.ELBOW].x, sizes[(int)PARTS.ELBOW].y, sizes[(int)PARTS.ELBOW].z);
		matrices.Add(tShoulder * tArm * tElbow * sElbow);

        // FOREARM
		Matrix4x4 tForearm = Transformations.TranslateM(places[(int)PARTS.FOREARM].x, places[(int)PARTS.FOREARM].y, places[(int)PARTS.FOREARM].z);
		Matrix4x4 sForearm = Transformations.ScaleM(sizes[(int)PARTS.FOREARM].x, sizes[(int)PARTS.FOREARM].y, sizes[(int)PARTS.FOREARM].z);
		matrices.Add(tShoulder * tArm * tElbow * tForearm * sForearm);

        // HAND
		Matrix4x4 tHand = Transformations.TranslateM(places[(int)PARTS.HAND].x, places[(int)PARTS.HAND].y, places[(int)PARTS.HAND].z);
		Matrix4x4 sHand = Transformations.ScaleM(sizes[(int)PARTS.HAND].x, sizes[(int)PARTS.HAND].y, sizes[(int)PARTS.HAND].z);
		matrices.Add(tShoulder * tArm * tElbow * tForearm * tHand * sHand);
        

        for(int i = 0; i < matrices.Count; i++)
		{
			parts[i].GetComponent<MeshFilter>().mesh.vertices = ApplyTransform(originals[i], matrices[i]);
		}
    }
}