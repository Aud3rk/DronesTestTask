using System;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class GizmoForAgents : MonoBehaviour
    {
        [SerializeField] private Color color;
        private NavMeshAgent _navMeshAgent;
        private LineRenderer lineRenderer;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
            lineRenderer.startWidth = 0.25f;
            lineRenderer.endWidth = 0.25f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }

        private void Update()
        {
            DrawLine();
        }

        private void DrawLine()
        {
            if (_navMeshAgent.hasPath)
            {
                lineRenderer.positionCount = _navMeshAgent.path.corners.Length;

                for (int i = 0; i < _navMeshAgent.path.corners.Length; i++)
                    lineRenderer.SetPosition(i, _navMeshAgent.path.corners[i]);
            }
            else
                lineRenderer.positionCount = 0;
        }
    }
}