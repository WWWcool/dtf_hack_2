using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    Delay,
    Move,
    Attack,
}

public enum ActionMoveType
{
    Free,
    Line,
    Loop,
    None,
}

public enum ActionDelayType
{
    Const,
    Range,
    None,
}

public enum ActionAttackType
{
    Melee,
    Range,
    None,
}

public enum ActionAttackProjectileType
{
    Bomb,
    Dust,
    Arrow,
    None,
}

public enum ActionAttackCountType
{
    Const,
    Range,
    None,
}

public class AIActionAttackContext
{
    public ActionAttackType type = ActionAttackType.None;
    public ActionAttackProjectileType projectileType = ActionAttackProjectileType.None;
    public ActionAttackCountType countType = ActionAttackCountType.None;
    public int min = 0;
    public int maxOrCount = 1;
    public System.Action onFinished = null;
}

[System.Serializable]
public class AIActionAttack
{
    [SerializeField] public ActionAttackType type = ActionAttackType.None;
    [SerializeField] public ActionAttackProjectileType projectileType = ActionAttackProjectileType.None;
    [SerializeField] public ActionAttackCountType countType = ActionAttackCountType.None;
    [SerializeField] public int min = 0;
    [SerializeField] public int maxOrCount = 1;

    [SerializeField] private UnityEventAIActionAttackContext m_onStartAttack;

    private bool m_started = false;
    private bool m_finished = false;

    public bool RunAction()
    {
        if (!m_started)
        {
            m_started = true;
            m_onStartAttack.Invoke(new AIActionAttackContext
            {
                type = type,
                projectileType = projectileType,
                countType = countType,
                min = min,
                maxOrCount = maxOrCount,
                onFinished = OnFinish,
            });
            Reinit();
        }
        return m_finished;
    }

    public void Reinit()
    {
        m_started = false;
        m_finished = false;
    }

    private void OnFinish()
    {
        m_finished = true;
    }
}

public class AIActionMoveContext
{
    public ActionMoveType type = ActionMoveType.None;
    public List<Transform> loopPositions;
    public int length;
    public System.Action onFinished = null;
}

[System.Serializable]
public class AIActionMove
{
    [SerializeField] public ActionMoveType type = ActionMoveType.None;
    [SerializeField] public List<Transform> loopPositions;
    [SerializeField] public int length;

    [SerializeField] private UnityEventAIActionMoveContext m_onStartMove;

    private bool m_started = false;
    private bool m_finished = false;

    public bool RunAction()
    {
        var res = m_finished;
        if (m_finished)
            Reinit();

        if (!m_started)
        {
            m_started = true;
            m_onStartMove.Invoke(new AIActionMoveContext
            {
                type = type,
                loopPositions = loopPositions,
                length = length,
                onFinished = OnFinish,
            });
        }
        return res;
    }

    public void Reinit()
    {
        m_started = false;
        m_finished = false;
    }

    private void OnFinish()
    {
        m_finished = true;
    }
}

public class AIActionDelayContext
{
    public ActionDelayType type = ActionDelayType.None;
    public float min = 0;
    public float maxOrConst = 1;
    public System.Action onFinished = null;
}

[System.Serializable]
public class AIActionDelay
{
    [SerializeField] public ActionDelayType type = ActionDelayType.None;
    [SerializeField] public float min = 0;
    [SerializeField] public float maxOrConst = 1;

    private float m_timer = 0;
    private float m_timerTarget = 0;

    public bool RunAction()
    {
        bool res = true;
        m_timer += Time.deltaTime;
        if (m_timer >= m_timerTarget)
        {
            res = true;
            Reinit();
        }
        else
        {
            res = false;
        }

        return res;
    }

    public void Reinit()
    {
        m_timer = 0;
        switch (type)
        {
            case ActionDelayType.Const:
                m_timerTarget = maxOrConst;
                break;
            case ActionDelayType.Range:
                m_timerTarget = Random.Range(min, maxOrConst);
                break;
        }
    }
}

[System.Serializable]
public class AIAction
{
    [SerializeField] public ActionType type = ActionType.Move;
    [SerializeField] public AIActionMove move;
    [SerializeField] public AIActionDelay delay;
    [SerializeField] public AIActionAttack attack;
    [SerializeField] public int chance = 1;
}

public enum StateType
{
    Idle,
    Move,
    Attack,
}

public class AIQueue : MonoBehaviour
{
    [SerializeField] private List<AIAction> m_actions;

    private AIAction m_curent_action;
    private StateType m_state = StateType.Idle;

    void Start()
    {
        GetNextAction();
        m_actions.ForEach(action => { action.delay.Reinit(); action.move.Reinit(); action.attack.Reinit(); });
    }

    void Update()
    {
        // TODO mb switch anim here
        switch (m_state)
        {
            case StateType.Idle:
            case StateType.Attack:
            case StateType.Move:
                // wait for action ends
                if (RunAction())
                {
                    GetNextAction();
                    // print("[AIQueue][Update] Action ends, next action: " + m_curent_action.type.ToString());
                }
                break;
        }
    }

    public bool RunAction()
    {
        bool res = true;
        switch (m_curent_action.type)
        {
            case ActionType.Delay:
                res = m_curent_action.delay.RunAction();
                break;
            case ActionType.Attack:
                res = m_curent_action.attack.RunAction();
                break;
            case ActionType.Move:
                res = m_curent_action.move.RunAction();
                break;
        }
        return res;
    }

    public AIAction GetNextAction()
    {
        m_curent_action = m_actions[GetRandomAction()];
        return m_curent_action;
    }

    private int GetRandomAction()
    {
        int sum = 0;
        m_actions.ForEach(action => sum += action.chance);
        int rnd = Random.Range(0, sum + 1);
        int index = 0;
        for (var i = 0; i < m_actions.Count; i++)
        {
            int val = m_actions[i].chance;
            if (rnd <= index + val)
            {
                index = i;
                break;
            }
            else
            {
                index += val;
            }
        }

        if (index == sum)
        {
            index = Random.Range(0, m_actions.Count);
            Debug.LogWarning("[AIQueue][GetRandomAction] Find action without chance: " + index.ToString());
        }

        return index;
    }
}
