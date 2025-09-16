namespace GameCreator.Core
{
    using UnityEngine;
    using System.Collections;
    using GameCreator;
    using GameCreator.Variables;
    using UnityEngine.AI;


    //[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    public class linkCompleter : MonoBehaviour
    {
        public void completeLink()
        {
            GameObject target = this.gameObject; // get offmeshlink
            string name = "agent"; // set variable name
            GameObject value = (GameObject)VariablesManager.GetLocal(target, name); // get target
            NavMeshAgent agent = value.GetComponent<NavMeshAgent>(); // get navmeshagent
            agent.CompleteOffMeshLink(); // complete offmeshlink
        }

        public void isOnOffMeshLink()
        {
            //Debug.Log(this.GetComponent<NavMeshAgent>().isOnOffMeshLink);
            //Debug.Log(this.gameObject);
            if (this.GetComponent<NavMeshAgent>().isOnOffMeshLink == true)
            {
                //Debug.Log("Bugfix001 (linkCompleter): Wait until agent is not on OffMeshLink.");
                VariablesManager.SetLocal(this.gameObject, "isOnOffMeshLink", true);
            }
            else
            {
                VariablesManager.SetLocal(this.gameObject, "isOnOffMeshLink", false);
            }
        }
    }
}