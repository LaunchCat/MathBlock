using UnityEngine;
using TMPro;


public class MathBlock : MonoBehaviour
{
    public enum Operation { Add, Sub, Multiply, Divide };

    public Operation operation;

    public float value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI textDisplay;
    void Start()
    {
        switch (operation)
        {
            case Operation.Add:
                textDisplay.text = "+";
                break;
            case Operation.Sub:
                textDisplay.text = "-";
                break;
            case Operation.Multiply:
                textDisplay.text = "X";
                break;
            case Operation.Divide:
                
                textDisplay.text = "\u00F7";
                break;  
            
        }
        
        textDisplay.text += " " + value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
