
using System;
using System.Collections;
using UnityEngine;
using TMPro;
public class AnswerZone : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI answerText;
    [SerializeField] private float answer;
    [SerializeField] private AudioSource OpenSFX;

    private Vector3 originalScale;
    private Vector3 desiredScale;
    float lerpPercentage = 0;
    float lerpSpeed = 10f;
    private void Start()
    {
        GridManager.gridManager.SnapToGrid(gameObject);
        originalScale = transform.localScale;
        desiredScale = originalScale * 1.5f;
    }

    private void OnValidate()
    {
        answerText.text = answer.ToString();
    }

    private void Update()
    {
        GridManager.gridManager.SnapToGrid(gameObject);
    }

    public bool HandleMathBlockCollision(MathBlock other)
    {
        
            MathBlock mathBlock = other.gameObject.GetComponent<MathBlock>();
            if (mathBlock.value == 0) return false;
            switch (mathBlock.operation)
            {
                case MathBlock.Operation.None:
                    return false;
                case MathBlock.Operation.Add:
                    answer += mathBlock.value;
                    break;
                case MathBlock.Operation.Subtract:
                    answer -= mathBlock.value;
                    break;
                case MathBlock.Operation.Multiply:
                    answer *= mathBlock.value;
                    break;
                case MathBlock.Operation.Divide:
                    if (mathBlock.value == 0)
                    {
                        break;
                    }

                    answer /= mathBlock.value;
                    break;
            }


            UpdateAnswerText();
            Destroy(other.gameObject);
            LevelManager.instance.QueueRemoveTurnTaker(other.gameObject.GetComponent<MathBlock>());
            GridManager.gridManager.SnapToGrid(gameObject);
            if (answer.Equals(0))
            {
                OpenSFX.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                OpenSFX.PlayOneShot(OpenSFX.clip);
                StartCoroutine(GrowShrinkDestroyVFX());
            }
            else
            {
                StartCoroutine(GrowShrinkVFX());
            }

            return true;
    }

    private void UpdateAnswerText()
    {
        answerText.text = answer.ToString();
    }

    private IEnumerator GrowShrinkVFX()
    {
        while (transform.localScale.x < desiredScale.x - 0.1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        while (transform.localScale.x >= originalScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = originalScale;
        yield return null;
    }
    
    private IEnumerator GrowShrinkDestroyVFX()
    {
        while (transform.localScale.x < desiredScale.x - 0.1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        while (transform.localScale.x >= 0.1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        GridManager.gridManager.GetNode(transform.position).SetObj(null);
        transform.position = new Vector3(-1000, -1000f, 1000);
        yield return null;
    }
}
