

namespace GameCreator.Core
{
    using UnityEngine;
    using System.Collections;
    using GameCreator;
    using GameCreator.Variables;

    public enum OffMeshLinkMoveMethod
    {
        Teleport,
        NormalSpeed,
        Parabola
    }

    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    public class AgentLinkMover : MonoBehaviour
    {
        public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;
        IEnumerator Start()
        {
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.autoTraverseOffMeshLink = false;
            //agent.updateRotation = false;

            while (true)
            {
                string linkType;
                linkType = agent.currentOffMeshLinkData.linkType.ToString();

                if (agent.isOnOffMeshLink && linkType == "LinkTypeManual")
                {
                    Manual(agent, 2.0f, 0.5f);
                    //agent.CompleteOffMeshLink();
                }
                else if (agent.isOnOffMeshLink)
                {
                    if (method == OffMeshLinkMoveMethod.NormalSpeed)
                        yield return StartCoroutine(NormalSpeed(agent));
                    else if (method == OffMeshLinkMoveMethod.Parabola)
                        yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                    agent.CompleteOffMeshLink();
                }
                yield return null;
            }
        }
        IEnumerator NormalSpeed(UnityEngine.AI.NavMeshAgent agent)
        {
            //Debug.Log("Normal offmesh");
            UnityEngine.AI.OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            while (agent.transform.position != endPos)
            {
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
                yield return null;
            }
        }
        IEnumerator Parabola(UnityEngine.AI.NavMeshAgent agent, float height, float duration)
        {
            //Debug.Log("Parabola offmesh");
            agent.gameObject.GetComponent<Actions>().Execute();
            UnityEngine.AI.OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            float normalizedTime = 0.0f;
            while (normalizedTime < 1.0f)
            {
                float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }
        public void Manual(UnityEngine.AI.NavMeshAgent agent, float height, float duration)
        {
            //Debug.Log("Manual offmesh");
            GameObject target = agent.currentOffMeshLinkData.offMeshLink.gameObject; // get offmeshlink
            string name = "agent"; // set variable name
            object value = agent.gameObject; // get character
            VariablesManager.SetLocal(target, name, value); // setting the character to a local variable on the offmeshlink

            agent.transform.rotation = target.transform.rotation;

            target.GetComponent<Actions>().Execute(); // execute offmeshlink actions
        }
    }
}