
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Reusing : MonoBehaviour
{
    public static Reusing Main;
    public float FreeFrame = 60;
    private void Awake()
    {
        Main = this;
    }
    public Dictionary<GameObject, List<GameObject>> Objects = new Dictionary<GameObject, List<GameObject>>();

    private void LateUpdate()
    {
        if(1 / Time.deltaTime > FreeFrame)
        {
            if (Objects.Count == 0) return;
            int x = Objects.Count;
            while (x > 0)
            {
                x--;
                List<GameObject> list = Objects.Values.ToList()[x];
                int y = list.Count;
                while (y > 0)
                {
                    y--;
                    GameObject obj = list[y];
                    if (!obj.active)
                    {
                        Objects[Objects.Keys.ToList()[x]].RemoveAt(y);
                        Destroy(obj);
                        break;
                    }
                }
            }
        }
    }

    public GameObject New(GameObject @object)
    {
        if (!Objects.ContainsKey(@object)) Objects.Add(@object, new List<GameObject>());
        GameObject allow = null;
        foreach (var obj in Objects[@object])
        {
            if (!obj.active)
            {
                allow = obj;
                allow.SetActive(true);
                break;
            }
        }
        if(allow == null)
        {
            allow = Instantiate(@object);
            Objects[@object].Add(allow);
        }
        return allow;
    }

    public void Dame(float Amount)
    {
        
    }
}
