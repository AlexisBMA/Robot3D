using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torso : MonoBehaviour
{
public enum PARTS
    {
        HIPS, TORSO, NECK, HEAD
    }
	List<GameObject> parts;
	List<Vector3[]> originals;
	List<Vector3> sizes;
	List<Vector3> places;
    float rotL1, dir, delta;
	
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
        delta = .33f;
        dir = 1;

        parts = new List<GameObject>();
        originals = new List<Vector3[]>();
        sizes = new List<Vector3>();
        places = new List<Vector3>();
        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.HIPS].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(1, 0.2f, 0.7f));
        places.Add(new Vector3(0, 0, 0));

        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.TORSO].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(1, 1, 0.7f));
        places.Add(new Vector3(0, 0.6f, 0));

        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.NECK].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(0.4f, 0.2f, 0.2f));
        places.Add(new Vector3(0, 0.6f, 0));

        parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        originals.Add(parts[(int)PARTS.HEAD].GetComponent<MeshFilter>().mesh.vertices);
        sizes.Add(new Vector3(0.6f, 0.6f, 0.6f));
        places.Add(new Vector3(0, 0.4f, 0));

    }
    // Update is called once per frame
    void Update()
    {

        rotL1 += dir * delta;
        if(rotL1 > 5 || rotL1 < -5) dir = -dir;

    	List<Matrix4x4> matrices = new List<Matrix4x4>();
        Matrix4x4 zHips = Transformations.RotateM(rotL1, Transformations.AXIS.AX_Y);
		Matrix4x4 tHips = Transformations.TranslateM(places[(int)PARTS.HIPS].x, places[(int)PARTS.HIPS].y, places[(int)PARTS.HIPS].z);
		Matrix4x4 sHips = Transformations.ScaleM(sizes[(int)PARTS.HIPS].x, sizes[(int)PARTS.HIPS].y, sizes[(int)PARTS.HIPS].z);
		matrices.Add(zHips * tHips * sHips);
		
		Matrix4x4 tTorso = Transformations.TranslateM(places[(int)PARTS.TORSO].x, places[(int)PARTS.TORSO].y, places[(int)PARTS.TORSO].z);
		Matrix4x4 sTorso = Transformations.ScaleM(sizes[(int)PARTS.TORSO].x, sizes[(int)PARTS.TORSO].y, sizes[(int)PARTS.TORSO].z);
		matrices.Add(zHips * tHips * tTorso * sTorso);
        
		Matrix4x4 tNeck = Transformations.TranslateM(places[(int)PARTS.NECK].x, places[(int)PARTS.NECK].y, places[(int)PARTS.NECK].z);
		Matrix4x4 sNeck = Transformations.ScaleM(sizes[(int)PARTS.NECK].x, sizes[(int)PARTS.NECK].y, sizes[(int)PARTS.NECK].z);
		matrices.Add(tHips * tTorso * tNeck * sNeck);
		
		Matrix4x4 tHead = Transformations.TranslateM(places[(int)PARTS.HEAD].x, places[(int)PARTS.HEAD].y, places[(int)PARTS.HEAD].z);
		Matrix4x4 sHead = Transformations.ScaleM(sizes[(int)PARTS.HEAD].x, sizes[(int)PARTS.HEAD].y, sizes[(int)PARTS.HEAD].z);
		matrices.Add(tHips * tTorso * tNeck * tHead * sHead);
			
		for(int i = 0; i < matrices.Count; i++)
		{
			parts[i].GetComponent<MeshFilter>().mesh.vertices = ApplyTransform(originals[i], matrices[i]);
		}
    }
}
