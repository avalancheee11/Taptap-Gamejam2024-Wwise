using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

public class TaskTrigger
{
    public int Type { get; set; }

    public string Condition { get; set; }
}

public class TaskState
{
    public int ID { get; set; }
    public int TaskID { get; set; }
    public int State { get; set; }
    public int Progress { get; set; }
}

public class Task
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Type { get; set; }
    public int Goal { get; set; }
    public int Reward { get; set; }
}


public class TaskManager : MonoBehaviour
{
    public List<Task> Tasks { get; set; }
    public List<TaskState> TaskStates { get; set; }
    public List<TaskTrigger> TaskTriggers { get; set; }


    void LoadTasks()
    {

    }

    void UpdateTaskState(TaskState state)
    {

    }

    void Start()
    {
        LoadTasks();
    }

    void Update()
    {
        foreach (TaskTrigger trigger in TaskTriggers)
        {
            if (CheckTrigger(trigger))
            {
                TaskState state = GetTaskState(trigger);
                if (state == null)
                {
                    state = new TaskState();
                    state.ID = TaskStates.Count + 1;
                    state.TaskID = GetTaskID(trigger);
                    state.State = 0;
                    state.Progress = 0;
                    TaskStates.Add(state);
                }
                if (state.State == 0)
                {
                    state.State = 1;
                    UpdateTaskState(state);
                }
            }
        }
    }

    bool CheckTrigger(TaskTrigger trigger)
    {
        // ������񴥷���
        switch (trigger.Type)
        {
            case 1:
                return EvaluateCondition(trigger.Condition);
            default:
                return false;
        }
    }

    bool EvaluateCondition(string condition)
    {
        // �����������ʽ
        return true;
    }

    int GetTaskID(TaskTrigger trigger)
    {
        // ��ȡ���� ID
        switch (trigger.Type)
        {
            case 1:
                return 1;
            default:
                return 0;
        }
    }

    TaskState GetTaskState(TaskTrigger trigger)
    {
        // ��ȡ����״̬
        int taskID = GetTaskID(trigger);
        foreach (TaskState state in TaskStates)
        {
            if (state.TaskID == taskID)
            {
                return state;
            }
        }
        return null;
    }
}