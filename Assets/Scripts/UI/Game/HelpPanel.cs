using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    #region 序列化私有变量
    //四个完成度标识
    [SerializeField]
    private Image[] rightIcon;
    //上方三个具体数值
    [SerializeField]
    private TextMeshProUGUI foodCount;
    
    [SerializeField]
    private TextMeshProUGUI coinCount;
    
    [SerializeField] 
    private TextMeshProUGUI popuCount;

    //三个目标成就
    [SerializeField]
    private TextMeshProUGUI targetOne;

    [SerializeField]
    private TextMeshProUGUI targetTwo;

    [SerializeField]
    private TextMeshProUGUI targetThree;

    //关卡详情
    [SerializeField]
    private TextMeshProUGUI stageSetTitle;
    [SerializeField]
    private TextMeshProUGUI[] stageSetDes;                                                   
    #endregion
    private Stage stageType = Stage.Defend;
    
    private void Start()
    {
        foreach(Image image in rightIcon)
        {
            image.enabled = false;
        }
    }


    private void OnEnable()
    {
        //记得删注释stageType = ConfirmedCardsManager.MyInstance.confirmStageType;
        foodCount.text = UIManager.MyInstance.totalFood.ToString();
        coinCount.text = UIManager.MyInstance.totalCoin.ToString();
        popuCount.text = UIManager.MyInstance.totalPopulation.ToString();

        targetOne.text = string.Format("总得分到达{0}：{1}/{2}", StageManager.MyInstance.stages[stageType].starsTarget[0],(UIManager.MyInstance.totalFood + UIManager.MyInstance.totalCoin), StageManager.MyInstance.stages[stageType].starsTarget[0]);
        if((UIManager.MyInstance.totalFood + UIManager.MyInstance.totalCoin) == StageManager.MyInstance.stages[stageType].starsTarget[0])
        {
            rightIcon[0].enabled = true;
        }
        targetTwo.text = string.Format("总人口到达{0}：{1}/{2}", StageManager.MyInstance.stages[stageType].starsTarget[1], UIManager.MyInstance.totalPopulation, StageManager.MyInstance.stages[stageType].starsTarget[1]);
        if (UIManager.MyInstance.totalPopulation == StageManager.MyInstance.stages[stageType].starsTarget[1])
        {
            rightIcon[1].enabled = true;
        }
        targetThree.text = string.Format("总得分到达{0}：{1}/{2}", StageManager.MyInstance.stages[stageType].starsTarget[2], (UIManager.MyInstance.totalFood + UIManager.MyInstance.totalCoin), StageManager.MyInstance.stages[stageType].starsTarget[2]);
        if ((UIManager.MyInstance.totalFood + UIManager.MyInstance.totalCoin) == StageManager.MyInstance.stages[stageType].starsTarget[2])
        {
            rightIcon[2].enabled = true;
        }

        stageSetTitle.text = StageManager.MyInstance.stages[stageType].stageSetTitle;
        stageSetDes[0].text = StageManager.MyInstance.stages[stageType].stageSetDescription[0];
        stageSetDes[1].text = StageManager.MyInstance.stages[stageType].stageSetDescription[1];

    }
}
