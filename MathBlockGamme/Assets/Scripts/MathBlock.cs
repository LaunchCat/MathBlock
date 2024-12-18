using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;


public class MathBlock : MonoBehaviour
{
    public enum Operation {None, Add, Subtract, Multiply, Divide };

    public Operation operation;

    public float value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI textDisplay;
    void Start()
    {
        UpdateTextDisplay();
        GridManager.gridManager.SnapToGrid(gameObject);
    }

    private bool HandleCollisionWithBlock(GameObject other)
    {
        if (other.CompareTag("Answer"))
        {
            AnswerZone answerZone = other.GetComponent<AnswerZone>();
            return answerZone.HandleMathBlockCollision(this);
        }
        
        if (other.CompareTag("MathBlock"))
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
                            break;
                        case Operation.Multiply:
                            value *= otherMathBlock.value;
                            break;
                        case Operation.Divide:
                            value /= otherMathBlock.value;
                            value =(float)Math.Round(value, 1);
                            break;
                        
                    }
                }
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
                            break;
                        case Operation.Subtract:
                            otherMathBlock.value -= value;
                            value = otherMathBlock.value;
                            break;
                        case Operation.Multiply:
                            otherMathBlock.value *= value;
                            value = otherMathBlock.value;
                            break;
                        case Operation.Divide:
                            otherMathBlock.value /= value;
                            value =(float)Math.Round(otherMathBlock.value, 1);
                            break;
                    }

                    operation = Operation.None;
                }
                Destroy(otherMathBlock.gameObject);
                UpdateTextDisplay();
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

    public bool Push(Vector3 direction)
    {
        Vector3 desiredMove = transform.position + direction * GridManager.gridSize;
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
        transform.position = desiredMove;
        if(gameObject)
            GridManager.gridManager.SnapToGrid(gameObject);
        return true;
    }
}
