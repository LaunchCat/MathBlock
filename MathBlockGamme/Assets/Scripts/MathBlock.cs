using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Random = Unity.Mathematics.Random;


public class MathBlock : TurnTakerBase
{
    public enum Operation {None, Add, Subtract, Multiply, Divide };

    public Operation operation;

    public float value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private AudioSource pushSound;
    [SerializeField] private AudioSource CombineSound;
    private Vector3 currentDir;
    private bool bMoving = false;
    Vector3 originalScale;
    void OnValidate()
    {
        UpdateTextDisplay();
    }

    private void Start()
    {
        GridManager.gridManager.SnapToGrid(gameObject);
        originalScale = transform.localScale;
    }

    private bool HandleCollisionWithBlock(GameObject other)
    {

        switch (other.tag)
        {
            case "Answer":
            {
                AnswerZone answerZone = other.GetComponent<AnswerZone>();
                return answerZone.HandleMathBlockCollision(this);
            }

            case "MathBlock":
            {
                   MathBlock otherMathBlock = other.GetComponent<MathBlock>();
                   if ((otherMathBlock.operation == Operation.None && this.operation == Operation.None) ||
                       (otherMathBlock.operation != Operation.None && this.operation != Operation.None)) return false;

                   if (operation == Operation.None)
                   {
                       if (otherMathBlock.value == 0)
                       {
                           operation = otherMathBlock.operation;
                       }
                       else
                       {
                           switch (otherMathBlock.operation)
                           {
                               case Operation.Add:
                                   value += otherMathBlock.value;
                                   break;
                               case Operation.Subtract:
                                   value -= otherMathBlock.value;
                                   if (value <= 0)
                                   {
                                       operation = Operation.Subtract;
                                       value = Mathf.Abs(value);
                                   }
                                   break;
                               case Operation.Multiply:
                                   value *= otherMathBlock.value;
                                   break;
                               case Operation.Divide:
                                   value /= otherMathBlock.value;
                                   value = (float)Math.Round(value, 1);
                                   break;

                           }
                       }
                       CombineSound.pitch = UnityEngine.Random.Range(0.6f, 1.2f);
                       CombineSound.Play();
                       StartCoroutine(GrowShrinkVFX());
                       Destroy(otherMathBlock.gameObject);
                       UpdateTextDisplay();
                       return true;
                   }

                   if (otherMathBlock.operation == Operation.None)
                   {
                       if (value == 0)
                       {
                           value = otherMathBlock.value;
                       }
                       else
                       {
                           switch (operation)
                           {
                               case Operation.Add:
                                   otherMathBlock.value += value;
                                   value = otherMathBlock.value;
                                   operation = Operation.None;
                                   break;
                               case Operation.Subtract:
                                   otherMathBlock.value -= value;
                                   value = otherMathBlock.value;
                                   if (value <= 0)
                                   {
                                       operation = Operation.Subtract;
                                       value = Mathf.Abs(value);
                                       break;
                                   }
                                   operation = Operation.None;
                                   break;
                               case Operation.Multiply:
                                   otherMathBlock.value *= value;
                                   value = otherMathBlock.value;
                                   operation = Operation.None;
                                   break;
                               case Operation.Divide:
                                   otherMathBlock.value /= value;
                                   value = (float)Math.Round(otherMathBlock.value, 1);
                                   operation = Operation.None;
                                   break;
                           }

                           
                       }

                       Destroy(otherMathBlock.gameObject);
                       StartCoroutine(GrowShrinkVFX());
                       UpdateTextDisplay();
                       return true;
                   }
                   break;
            }

           case "Pit":
           {
               //Destroy(gameObject, 1);
               StartCoroutine(GrowShrinkDestroyVFX());
               return true;
           }
        }

        return false;
    }


    void UpdateTextDisplay()
    {
        switch (operation)
        {
            case Operation.None:
                textDisplay.text = " ";
                break;
            case Operation.Add:
                textDisplay.text = "+";
                break;
            case Operation.Subtract:
                textDisplay.text = "-";
                break;
            case Operation.Multiply:
                textDisplay.text = "X";
                break;
            case Operation.Divide:
                
                textDisplay.text = "\u00F7";
                break;  
            
        }

        if (value == 0)  textDisplay.text  += "";
        else textDisplay.text += " " + value;
        
        
    }

    public void SuperPush(Vector3 direction)
    {
        currentDir = direction;
        bMoving = true;
    }

    public bool Push(Vector3 direction)
    {
        Vector3 desiredMove = transform.position + direction * GridManager.gridManager.gridSize;
        if(GridManager.gridManager.GetNode(desiredMove) == null)
            return false;
        if (GridManager.gridManager.GetNode(desiredMove).gameObj)
        {
            if (HandleCollisionWithBlock(GridManager.gridManager.GetNode(desiredMove).gameObj) == false)
            {
                if(GridManager.gridManager.GetNode(desiredMove).gameObj.GetComponent<MathBlock>().Push(direction) == false)
                    return false;
            }
        }
        pushSound.pitch = UnityEngine.Random.Range(.7f, 1.0f);
        pushSound.PlayOneShot(pushSound.clip,1);
        transform.position = desiredMove;
        if(gameObject)
            GridManager.gridManager.SnapToGrid(gameObject);
        return true;
    }
    
    public override bool TakeTurn()
    {
        if (!bMoving) return true;
        bMoving = Push(currentDir);
        return true;
    }
    
    private IEnumerator GrowShrinkVFX()
    {
        Vector3 desiredScale = new Vector3(1.5f, 1.5f, 1.5f);
        float lerpSpeed = 18f;
        while (transform.localScale.x < desiredScale.x - 0.1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        while (transform.localScale.x >= originalScale.x + 0.1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = originalScale;
        yield return null;
    }
    
    private IEnumerator GrowShrinkDestroyVFX()
    {
        LevelManager.instance.RemoveTurnTaker(this);
        Vector3 desiredScale = new Vector3(1.5f, 1.5f, 1.5f);
        float lerpSpeed = 9f;
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


