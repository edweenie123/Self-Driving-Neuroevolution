using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHolder : MonoBehaviour
{
    public Material bestCarMat_;
    public static Material bestCarMat;

    void Awake()
    {
        bestCarMat = bestCarMat_;
    }

}
