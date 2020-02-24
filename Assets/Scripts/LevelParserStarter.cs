using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelParserStarter : MonoBehaviour
{
    public string filename;
    public GameObject Rock;
    public GameObject Brick;
    public GameObject QuestionBox;
    public GameObject Stone;
    public GameObject GoalPole;
    public GameObject Water;
    public Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        RefreshParse();
    }

    private void FileParser()
    {
        string fileToParse = string.Format("{0}{1}{2}.txt", Application.dataPath, "/Resources/", filename);

        using (StreamReader sr = new StreamReader(fileToParse))
        {
            string line = "";
            int row = 0;
            while ((line = sr.ReadLine()) != null)
            {

                int column = 0;
                char[] letters = line.ToCharArray();
                foreach (var letter in letters)
                {
                    //Call SpawnPrefab
                    SpawnPrefab(letter,new Vector3(column, -row,-.5f));
                    column += 1;
                }
                row += 1;
            }
            sr.Close();
        }
    }

    private void SpawnPrefab(char spot, Vector3 positionToSpawn)
    {
        GameObject ToSpawn;
        switch (spot)
        {
            case 'b': Debug.Log("Spawn Brick");
                ToSpawn = GameObject.Instantiate(Brick, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                break;
            case 'B': Debug.Log("Spawn Background Brick");
                ToSpawn = GameObject.Instantiate(Brick, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn + new Vector3(0, 0, 1.0f);
                break;
            case '?': Debug.Log("Spawn QuestionBox");
                ToSpawn = GameObject.Instantiate(QuestionBox, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                ToSpawn.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case 'x': Debug.Log("Spawn Rock");
                ToSpawn = GameObject.Instantiate(Rock, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                break;
            case 's': Debug.Log("Spawn Stone");
                ToSpawn = GameObject.Instantiate(Stone, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                break;
            case 'S':
                Debug.Log("Spawn Background Stone");
                ToSpawn = GameObject.Instantiate(Stone, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn + new Vector3(0, 0, 1.0f);
                break;
            case 'g': Debug.Log("Spawn GoalPole");
                ToSpawn = GameObject.Instantiate(GoalPole, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                break;
            case 'w': Debug.Log("Spawn Water");
                ToSpawn = GameObject.Instantiate(Water, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                break;
            //default: Debug.Log("Default Entered"); break;
            default: return;
        }
    }

    public void RefreshParse()
    {
        GameObject newParent = new GameObject();
        newParent.name = "Environment";
        newParent.transform.position = parentTransform.position;
        newParent.transform.parent = this.transform;
        if (parentTransform)
            Destroy(parentTransform.gameObject);
        parentTransform = newParent.transform;
        FileParser();
    }
}
