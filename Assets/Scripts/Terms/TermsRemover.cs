using GoogleTrends.GameManagers;
using UnityEditor;
using UnityEngine;

namespace GoogleTrends.Terms
{
    public class TermsRemover : MonoBehaviour
    {
        public void RemoveTerm()
        {
            GetComponentInParent<GameSettingsManager>().RemoveTerm(transform.GetSiblingIndex());
        }
    }
}