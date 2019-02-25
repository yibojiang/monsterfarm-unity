using System.Collections.Generic;
using UnityEngine;
public class MonSlimePawn : MonsterPawn
{
    private const int spawnCount = 2;
    private string[] slimePrefabPaths = {"Prefab/Monster/mon_slime_tiny", "Prefab/Monster/mon_slime", "Prefab/Monster/mon_slime_large"};

    MonSlimePawn()
    {
        favouriteItem = "apple";
    }

    public override void AddAge()
    {
        base.AddAge();
        Debug.Log(this.gameObject.name + " age: " + Age);
        if (Age == 1)
        {
            var monsterObj = Instantiate(Resources.Load(slimePrefabPaths[1], typeof(GameObject))) as GameObject;
            monsterObj.transform.position = transform.position;
            monsterObj.GetComponent<MonsterPawn>().Age = Age;
            Destroy(this.gameObject);
        }
        
        
        if (Age == 2)
        {
            var monsterObj = Instantiate(Resources.Load(slimePrefabPaths[2], typeof(GameObject))) as GameObject;
            monsterObj.transform.position = transform.position;
            monsterObj.GetComponent<MonsterPawn>().Age = Age;
            Destroy(this.gameObject);
        }

        if (Age >= 3)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                var monsterObj = Instantiate(Resources.Load(slimePrefabPaths[0], typeof(GameObject))) as GameObject;
                monsterObj.transform.position = transform.position;
                monsterObj.GetComponent<MonsterPawn>().Age = 0;
            }
            Destroy(this.gameObject);
        }
        
        
    }
}