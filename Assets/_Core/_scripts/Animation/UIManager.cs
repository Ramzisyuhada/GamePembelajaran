using UnityEngine;

public class UIManager : MonoBehaviour
{



    void Start()
    {
        
    }

    public GameObject test;

    public void Test()
    {
        // Instantiate prefab
        GameObject obj = Instantiate(test);

        // Ambil component SceneTransitioner
        SceneTransitioner transition = obj.GetComponent<SceneTransitioner>();

       // transition.Show(true);
        // Panggil load scene
       transition.LoadScene("SampleScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
