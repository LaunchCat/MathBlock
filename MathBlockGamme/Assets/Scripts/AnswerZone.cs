
using System;
using UnityEngine;
using TMPro;
public class AnswerZone : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI answerText;
    [SerializeField] private float answer;
    [SerializeField] private AudioSource OpenSFX;
    private void Start()
    {
        GridManager.gridManager.SnapToGrid(gameObject);
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
            GridManager.gridManager.SnapToGrid(gameObject);
            if (answer.Equals(0))
            {
                OpenSFX.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                OpenSFX.PlayOneShot(OpenSFX.clip);
                //Destroy(gameObject);
                transform.position = new Vector3(-1000, -1000f, 1000);
            }

            return true;
    }

    private void UpdateAnswerText()
    {
        answerText.text = answer.ToString();
    }
}
