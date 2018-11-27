using UnityEngine;
using TMPro;

[System.Obsolete]
public class PFFactory : MonoBehaviour{

    //Singleton
    public static PFFactory Instance { get; private set; }

    //Assign singleton
    private void Awake()
    {
        Instance = this;
    }

    [Tooltip("The Prefab for any PF")]
    public GameObject pfPrefab;

    /// <summary>
    /// Creates a new PGF
    /// </summary>
    /// <returns></returns>
    public ParametricFirearm CreatePF(PFData data = null)
    {
        ParametricFirearm pf = Instantiate(pfPrefab).GetComponent<ParametricFirearm>();

        //IF Data is null, the data will be randomly created
        pf.Data = data ?? new PFData();

        return pf;
    }
}
