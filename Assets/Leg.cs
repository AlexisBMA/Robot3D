using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    public enum PARTS
    {
        THIGH, CALF, FOOT
    }

    List<GameObject> parts;
	List<Vector3[]> originals;
	List<Vector3> sizes;
	List<Vector3> places;
    float x, rotL1, dir, delta;

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
        rotL1 = 0;
        delta = 2;
        parts = new List<GameObject>();
        originals = new List<Vector3[]>();
        sizes = new List<Vector3>();

        // thigh
        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.THIGH].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(.4f, 1, 0.2f));

        // calf
        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.CALF].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(.4f, .8f, 0.2f));

        // foot
        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.FOOT].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(.4f, .2f, 0.5f));
    }
    public void Init(bool isRight)
    {
        places = new List<Vector3>();
        if(isRight){
            x = 0.3f;
            dir = 1;
        }else{
            x = -0.3f;
            dir = -1;
        }
        places.Add(new Vector3(x, -.6f, 0));
        places.Add(new Vector3(0, -.9f, 0));
        places.Add(new Vector3(0, -.45f, .15f));
    }
    // Update is called once per frame
    void Update()
    {
        rotL1 += dir * delta;
        if(rotL1 > 30 || rotL1 < -30) dir = -dir;
        // thigh
        List<Matrix4x4> matrices = new List<Matrix4x4>();
        Matrix4x4 zThigh = Transformations.RotateM(rotL1, Transformations.AXIS.AX_X);
		Matrix4x4 tThigh = Transformations.TranslateM(places[(int)PARTS.THIGH].x, places[(int)PARTS.THIGH].y, places[(int)PARTS.THIGH].z);
		Matrix4x4 sThigh = Transformations.ScaleM(sizes[(int)PARTS.THIGH].x, sizes[(int)PARTS.THIGH].y, sizes[(int)PARTS.THIGH].z);
		matrices.Add(zThigh * tThigh * sThigh);

        // calf
        
        Matrix4x4 zCalf = Transformations.RotateM(rotL1 * 1.3f, Transformations.AXIS.AX_X);
		Matrix4x4 tCalf = Transformations.TranslateM(places[(int)PARTS.CALF].x, places[(int)PARTS.CALF].y, places[(int)PARTS.CALF].z);
		Matrix4x4 sCalf = Transformations.ScaleM(sizes[(int)PARTS.CALF].x, sizes[(int)PARTS.CALF].y, sizes[(int)PARTS.CALF].z);
		matrices.Add(zCalf * tThigh * tCalf * zThigh * sCalf);

        // FOOT
		Matrix4x4 tFoot = Transformations.TranslateM(places[(int)PARTS.FOOT].x, places[(int)PARTS.FOOT].y, places[(int)PARTS.FOOT].z);
		Matrix4x4 sFoot = Transformations.ScaleM(sizes[(int)PARTS.FOOT].x, sizes[(int)PARTS.FOOT].y, sizes[(int)PARTS.FOOT].z);
		matrices.Add(zCalf * tThigh * tCalf * zThigh * tFoot  * sFoot);

        for(int i = 0; i < matrices.Count; i++)
		{
			parts[i].GetComponent<MeshFilter>().mesh.vertices = ApplyTransform(originals[i], matrices[i]);
		}
    }
}
