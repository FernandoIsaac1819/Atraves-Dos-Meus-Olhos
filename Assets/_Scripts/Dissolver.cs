using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    private Transformation m_Transformation;

    public enum MeshesDetection
    {
        GetComponents,GetComponentsInChildren,GetComponentsInParents
    }

    public enum MeshRenderType
    {
        MeshRenderer,SkinnedMeshRenderer
    }

    private List<MeshRenderer> m_MeshRenderers;
    private List<SkinnedMeshRenderer> m_SkinnedMeshRenderers;
    private List<Material> m_Materials;

    [SerializeField] private float m_Duration = 2.7f;
    [SerializeField] private MeshesDetection m_MeshesDetection;
    [SerializeField] private MeshRenderType m_MeshRenderType;

    private bool m_Materialized = false;
    private bool m_Dissolved = true;
    private float m_DissolveAmount;
    private bool m_Finished = true;
    public bool m_FinishedMaterializing;
    public bool m_FinishedDissolving;


    public float Duration {get {return m_Duration;} set {m_Duration = value;}}
    public bool FinishedDissolve {get {return m_FinishedDissolving;} set {m_FinishedDissolving = value;}}
    public bool FinishedMaterial {get {return m_FinishedMaterializing;} set {m_FinishedMaterializing = value;}}
    //Queue Coroutines
    public Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

    void Start()
    {

        StartCoroutine(CoroutineCoordinator());

        if (m_MeshRenderType == MeshRenderType.MeshRenderer)
        {
            switch (m_MeshesDetection)
            {
                case MeshesDetection.GetComponents:
                    m_MeshRenderers = new List<MeshRenderer>(GetComponents<MeshRenderer>());
                    break;
                case MeshesDetection.GetComponentsInChildren:
                    m_MeshRenderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
                    break;
                case MeshesDetection.GetComponentsInParents:
                    m_MeshRenderers = new List<MeshRenderer>(GetComponentsInParent<MeshRenderer>());
                    break;
            }

            m_Materials = new List<Material>();
            foreach (var renderer in m_MeshRenderers)
            {
                m_Materials.AddRange(renderer.materials);
            }

        }

        else
        {
            switch (m_MeshesDetection)
            {
                case MeshesDetection.GetComponents:
                    m_SkinnedMeshRenderers = new List<SkinnedMeshRenderer>(GetComponents<SkinnedMeshRenderer>());
                    break;
                case MeshesDetection.GetComponentsInChildren:
                    m_SkinnedMeshRenderers = new List<SkinnedMeshRenderer>(GetComponentsInChildren<SkinnedMeshRenderer>());
                    break;
                case MeshesDetection.GetComponentsInParents:
                    m_SkinnedMeshRenderers = new List<SkinnedMeshRenderer>(GetComponentsInParent<SkinnedMeshRenderer>());
                    break;
            }

            m_Materials = new List<Material>();
            foreach (var renderer in m_SkinnedMeshRenderers)
            {
                m_Materials.AddRange(renderer.materials);
            }
        }    
    }

    private void OnTransformPressed(object sender, EventArgs e)
    {
        StartCoroutine(TransformationEffect());
    }

    IEnumerator TransformationEffect() 
    {
        StartCoroutine(Materialize(m_Duration));
        yield return new WaitForSeconds(m_Duration);
        StartCoroutine(Dissolve(m_Duration));
    }

    public bool MaterializeDissolve()
    {
        if (!m_Finished) return false;
        m_Finished = false;
        
        if (m_Dissolved)
            StartCoroutine(Materialize(m_Duration));
        else if (m_Materialized)
            StartCoroutine(Dissolve(m_Duration));

        return true;
    }

    public void QueueMaterializeDissolve()
    {
        coroutineQueue.Enqueue(QueueMaterializeDissolve(m_Duration));
    }

    IEnumerator CoroutineCoordinator()
    {
        while (true)
        {
            while (coroutineQueue.Count > 0)
                yield return StartCoroutine(coroutineQueue.Dequeue());
            yield return null;
        }
    }

    private IEnumerator QueueMaterializeDissolve(float fadeDuration)
    {
        float elapsedTime = 0f;

        if(m_Dissolved)
        {

            m_Materialized = true;
            m_Dissolved = false;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                m_DissolveAmount = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);

                foreach (var mat in m_Materials)
                {
                    mat.SetFloat("_Dissolve", m_DissolveAmount);
                }
                yield return null;
            }

            m_Finished = true;
        }

        else if(m_Materialized)
        {
            m_Materialized = false;
            m_Dissolved = true;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                m_DissolveAmount = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                foreach (var mat in m_Materials)
                {
                    mat.SetFloat("_Dissolve", m_DissolveAmount);
                }
                yield return null;
            }


            m_Finished = true;
        }

    }

    private IEnumerator Materialize(float fadeDuration)
    {
        float elapsedTime = 0f;

        m_Materialized = true;
        m_Dissolved = false;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            m_DissolveAmount = Mathf.Lerp(1, 0,elapsedTime / fadeDuration);

            foreach (var mat in m_Materials)
            {
                mat.SetFloat("_Dissolve", m_DissolveAmount);
            }
            yield return null;
        }
        
        m_Finished = true;
    }

    private IEnumerator Dissolve(float fadeDuration)
    {
        float elapsedTime = 0f;

        m_Materialized = false;
        m_Dissolved = true;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            m_DissolveAmount = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            foreach (var mat in m_Materials)
            {
                mat.SetFloat("_Dissolve", m_DissolveAmount);
            }
            yield return null;
        }
        
        m_Finished = true;
    }

    void Update() 
    {
        if(m_Materialized && m_DissolveAmount == 0) 
        {
            m_FinishedMaterializing = true;
            m_FinishedDissolving = false;
        } 

        if(m_Dissolved && m_DissolveAmount == 1) 
        {
            m_FinishedMaterializing = false;
            m_FinishedDissolving = true;
        }
    }
}
