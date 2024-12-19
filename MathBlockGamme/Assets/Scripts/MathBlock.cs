using System;
using UnityEngine;
using TMPro;


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
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("MathBlock"))
        {
            MathBlock otherMathBlock = other.gameObject.GetComponent<MathBlock>();
            if ((otherMathBlock.operation == Operation.None && this.operation == Operation.None) ||
                (otherMathBlock.operation != Operation.None && this.operation != Operation.None)) return;

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
            }
 
        }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
