using UnityEngine;
using System.Collections.Generic;


public class StageManager : MonoBehaviour {
    public enum Stage { Library, EvelynsAdventure, NigelsAdventure }

    public static StageManager Instance;

    public List<StageObjectKeyValue> stages = new List<StageObjectKeyValue>();

    int currentStageId = -1;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        ParchmentSection.InitializeStatics();
        ChangeStage(Stage.Library);
    }

    public void ChangeStage(Stage s)
    {
        if (currentStageId > -1)
        {
            stages[currentStageId].StageObject.SetActive(false);
        }

        currentStageId = stages.FindIndex(x => x.Stage == s);
        stages[currentStageId].StageObject.SetActive(true);
    }

    [System.Serializable]
    public class StageObjectKeyValue
    {
        public Stage Stage;
        public GameObject StageObject;
    }
}


