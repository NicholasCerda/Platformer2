using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GuiScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public int score, coins;
    private bool failed;
    public bool lockCoin,lockWin;
    public float timeStart,timeLapse;
    // Start is called before the first frame update
    void Start()
    {
        failed = false;
        lockCoin = false;
        lockWin = false;
        score = 0;
        coins = 0;
        timeStart = 100;
        timeLapse = 0;
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeLapse += Time.deltaTime;
        scoreText.text = score.ToString("D6");
        coinText.text = "x"+coins.ToString("D2");
        int temp = (int)(timeStart - timeLapse);
        if (temp>0)
            timerText.text = temp.ToString("D3");
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.gameObject.name == "Brick(Clone)")
                {
                    Destroy(hit.transform.gameObject,.03f);
                }
                if (hit.collider.gameObject.name == "Question(Clone)")
                {
                    //StartCoroutine(BlockHit(hit.transform));
                    onAddCoin(hit);
                }
            }
        }
        if (temp <= 0 && !failed)
        {
            failed = true;
            //lockWin=true;
            Debug.Log("Player failed to beat level in time");
        }
    }
    IEnumerator BlockHit(Transform Tran)
    {
        Tran.localScale *= 1.2f;
        yield return new WaitForSeconds(0.03f);
        Tran.localScale /= 1.2f;
        lockCoin = false;
    }
    void onAddCoin(RaycastHit hit)
    {
        coins += 1;
        addScore(100);
        StartCoroutine(BlockHit(hit.transform));
    }
    public void onAddCoin(GameObject Question)
    {
        if (lockCoin)
        {
            return;
        }
        lockCoin = true;
        coins += 1;
        addScore(100);
        StartCoroutine(BlockHit(Question.transform));
    }
    public void addScore(int val)
    {
        score += val;
    }
    public void gameOver(string reason)
    {
        Debug.Log("Game Over, "+reason);
    }
    public void setWin()
    {
        if (!lockWin)
        {
            lockWin = true;
            //failed=true;
            int temp = (int)(timeStart - timeLapse);
            gameOver("Player Won! Time remaining: "+temp);
            addScore(50 * temp);
        }
    }
}
