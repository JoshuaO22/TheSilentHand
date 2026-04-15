using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionObjectiveUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject missionObjectivesPanel;
    [SerializeField] private GameObject objectiveTemplate;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failureColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;

    private readonly Dictionary<string, GameObject> objectives = new Dictionary<string, GameObject>();

    public void AddObjective(string text)
    {
        if (missionObjectivesPanel == null)
        {
            Debug.LogWarning("MissionObjectiveUIHandler is missing a mission objectives panel reference.");
        }

        if (objectiveTemplate == null)
        {
            Debug.LogWarning("MissionObjectiveUIHandler is missing an objective template reference.");
        }

        GameObject objectiveInstance = Instantiate(objectiveTemplate);
        objectiveInstance.transform.SetParent(missionObjectivesPanel.transform);
        objectiveInstance.GetComponent<TMP_Text>().text = text;
        objectiveInstance.SetActive(true);

        objectives.Add(text, objectiveInstance);
        objectiveInstance.transform.SetSiblingIndex(objectives.Count);
    }

    public void ResetObjectives()
    {
        foreach (var kvp in objectives)
        {
            if (kvp.Value != null)
            {
                Destroy(kvp.Value);
            }
        }

        objectives.Clear();

        if (missionObjectivesPanel != null)
        {
            missionObjectivesPanel.SetActive(false);
        }
    }

    public void MarkObjectiveSucceeded(string text)
    {
        SetObjectiveState(text, successColor);
    }

    public void MarkObjectiveFailed(string text)
    {
        SetObjectiveState(text, failureColor);
    }

    public void ResetObjectiveState(string text)
    {
        SetObjectiveState(text, defaultColor);
    }

    private void SetObjectiveState(string text, Color color)
    {
        if (objectives.TryGetValue(text, out GameObject objective))
        {
            if (objective != null)
            {
                TMP_Text textComponent = objective.GetComponent<TMP_Text>();
                if (textComponent != null)
                {
                    textComponent.color = color;
                }
            }
            else
            {
                Debug.LogWarning($"MissionObjectiveUIHandler objective for text '{text}' is null.");
            }
        }
        else
        {
            Debug.LogWarning($"MissionObjectiveUIHandler objective with text '{text}' not found.");
        }
    }
}
