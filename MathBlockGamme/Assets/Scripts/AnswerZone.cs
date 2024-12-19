
using UnityEngine;
using TMPro;
public class AnswerZone : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI answerText;
    [SerializeField] private float answer;
    
   

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("MathBlock"))
        {
            MathBlock mathBlock = other.gameObject.GetComponent<MathBlock>();
            switch (mathBlock.operation)
            {
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
            GameManager.Get().AnswerUpdated(mathBlock, answer);
            
         
        }
    }

    private void UpdateAnswerText()
    {
        answerText.text = answer.ToString();
    }
}
