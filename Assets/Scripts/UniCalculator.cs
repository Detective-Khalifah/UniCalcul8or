using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NCalc;

public class UniCalculator : MonoBehaviour
{
    [SerializeField]
    private Button btZero, btOne, btTwo, btThree, btFour, btFive,
        btSix, btSeven, btEight, btNine, btLePar, btRePar,
        btMinus, btPlus, btPlusMinus, btDivision, btMultiplication,
        btEqual, btDecimal, btDel;
    [SerializeField]
    private TextMeshProUGUI textExpression, textResult;

    private static StringBuilder expression = new StringBuilder();

    // Start is called before the first frame update
    void Start()
    {
        Button[] buttonsArr = {btZero, btOne, btTwo, btThree, btFour, btFive,
        btSix, btSeven, btEight, btNine, btLePar, btRePar,
        btMinus, btPlus, btPlusMinus, btDivision, btMultiplication,
        btEqual, btDecimal};
        foreach (Button button in buttonsArr) {
            // button.GetComponent<Button>();
            button.onClick.AddListener(
                () => Touched(button)
            );
        }

        btDel.onClick.AddListener(Del);

        DisplayExpression();
    }

    /**
     * Ascertain the button clicked, and trigger the appropriate response.
     *
     * @param button clicked
     */
    void Touched(Button b)
    {
        Debug.Log("Button " + b + " touched.");
        string id = b.gameObject.tag;
        switch(id)
        {
            case "BtnDecimal":
                AppendChar("deciPoint");
                break;
            case "BtnMinus":
                AppendChar("minus");
                break;
            case "BtnPlus":
                AppendChar("plus");
                break;
            case "BtnMultiplication":
                AppendChar("multi");
                break;
            case "BtnDivision":
                AppendChar("divi");
                break;
            case "BtnPlus-Minus":
                AppendChar("plusMinus");
                break;
            case "BtnEqual":
                AppendChar("equals?");
                break;
            case "BtnZero":
                AppendChar("0");
                break;
            case "BtnOne":
                AppendChar("1");
                break;
            case "BtnTwo":
                AppendChar("2");
                break;
            case "BtnThree":
                AppendChar("3");
                break;
            case "BtnFour":
                AppendChar("4");
                break;
            case "BtnFive":
                AppendChar("5");
                break;
            case "BtnSix":
                AppendChar("6");
                break;
            case "BtnSeven":
                AppendChar("7");
                break;
            case "BtnEight":
                AppendChar("8");
                break;
            case "BtnNine":
                AppendChar("9");
                break;
            case "BtnLePar":
                AppendChar("(");
                break;
            case "BtnRePar":
                AppendChar(")");
                break;
        }

    }

    private void AppendChar(string btn)
    {
        char firstChar = 'a', lastChar = 'z';
        int lastPosition = 0;
        if (expression != null)
        {
            if (expression.Length > 1)
            {
                lastPosition = expression.Length - 1;
                firstChar = expression[0];
                lastChar = expression[lastPosition];
            }
        }
        else
        {
            expression = new StringBuilder();
        }
        string eq = expression.ToString();
        switch (btn)
        {
            case "deciPoint":
                // check if a decimal point is contained in the equation
                // If there's one, replace it at the end; otherwise, append '.' to end of expression
                if (eq.Contains("."))
                    expression.Remove(expression.ToString().IndexOf("."), 1);
                    // expression.deleteCharAt(expression.indexOf("."));
                expression.Append('.');
                DisplayExpression();
                break;
            case "divi":
                if (eq.EndsWith("/"))
                    break;
                else if (lastChar == '+' || lastChar == '-' || lastChar == '*')
                    expression.Replace(oldChar: lastChar, newChar: '/', startIndex: lastPosition, count: 1);
                else
                    expression.Append("/");
                DisplayExpression();
                break;
            case "minus":
                if (eq.EndsWith("-"))
                    break;
                else if (lastChar == '+' || lastChar == '*' || lastChar == '/')
                    expression.Replace(oldChar: lastChar, newChar: '-', startIndex: lastPosition, count: 1);
                else
                    expression.Append('-');
                DisplayExpression();
                break;
            case "multi":
                if (eq.EndsWith("*"))
                    break;
                else if (lastChar == '+' || lastChar == '-' || lastChar == '/')
                    expression.Replace(oldChar: lastChar, newChar: '*', startIndex: lastPosition, count: 1);
                else
                    expression.Append("*");
                DisplayExpression();
                break;
            case "plus":
                if (eq.EndsWith("+"))
                {
                    break;
                }
                else if (lastChar == '-' || lastChar == '*' || lastChar == '/')
                    expression.Replace(oldChar: lastChar, newChar: '+', startIndex: lastPosition, count: 1);
                // expression.Replace(lastPosition, expression.Length, "+");
                else
                {
                    expression.Append("+");
                }
                DisplayExpression();
                break;
            case "plusMinus":
                // If expression is empty, simply skip using plus-minus sign the process of finding
                // last sign, and return to last point/stack
                if (eq.Length == 0)
                    return;

                // If a "-" sign already exists at the beginning, skip the process of finding last
                // sign, and return to last point/stack
                if (eq[0] == '-')
                    return;
                int lastSignIndex = CheckSign();

                Debug.Log("lastSignIndex: " + lastSignIndex);

                // Add a "-" sign to the part of expression after a sign (+, -, / or *) if one exists
                if (lastSignIndex != -1)
                {
                    string rem = expression.ToString().Substring(lastSignIndex + 1, 1);

                    // When there are no numbers after the last sign, return to stack
                    if (!rem.Equals(""))
                        return;

                    // When there already exists a plus-minus expression segment
                    // TODO: Use a regex here; would be easier to find functions already having
                    //  "(-function)" at different segments to avoid repeating at same
                    //  segment, and toggle deletion of "(-) without function part, if one already
                    //  exits. Also find out the plus-minus code; that'd make things easier without
                    //  confusing minus sign
                    if (eq.Contains("(-"))
                    {
                        Debug.Log("expression in eq.contains(\"(-\")) check:: " + expression);
                        return;
                    }
                    expression = new StringBuilder(eq.Substring(0, (lastSignIndex - 0)) + eq[lastSignIndex] + "(-" + rem + ")");
                    // expression = new StringBuilder(eq.Substring(0, lastSignIndex) + eq[lastSignIndex] + "(-" + rem + ")");
                }
                else
                {
                    // add a "-" sign at the beginning and append the expression if no sign is found
                    expression = new StringBuilder("-").Append(eq);
                }

                DisplayExpression();
                break;
            case "equals?":
                if (eq.EndsWith("*") || eq.EndsWith("+") || eq.EndsWith("-") || eq.EndsWith("/"))
                    expression.Remove(lastPosition, 1);
                DisplayResult();
                break;
            // TODO: Add logic and layout tricks to handle "dangling parentheses"
            case "(":
                expression.Append("(");
                DisplayExpression();
                break;
            case ")":
                expression.Append(")");
                DisplayExpression();
                break;
            case "0":
                if (expression != null)
                {
                    if (!(expression.Length == 1 && expression[0] == '0'))
                        expression.Append(btn);
                }
                else
                {
                    expression.Append(btn);
                }
                DisplayExpression();
                break;
            default:
                if (expression != null)
                    expression.Append(btn);
                DisplayExpression();
                break;
        }
    }

    /**
     * Traverse the expression from right, and return the index (if found) of the first
     * arithmetic sign encountered
     *
     * @return the sign's index (if one exists)
     */
    private int CheckSign()
    {
        int lastSignIndex = -1;

        for (int z = expression.Length - 1; z > 1; z--)
        {
            switch (expression[z])
            {
                case '-':
                case '+':
                case '/':
                case '*':
                    lastSignIndex = z;
                    break;
                default:
                    break;
            }

            if (lastSignIndex != -1)
                break;
        }

        return lastSignIndex;

    }

    void Del()
    {
        if (expression.Equals("") || expression == null)
            return;
        else if (expression.Length > 0)
        {
            // expression.Remove(expression.Length - 1, 1);
            expression.Clear();
        }

        DisplayExpression();
        DisplayResult();
    }

    /**
     * Utility method that displays the expression's current state in the #textExpression
     * Text
     */
    private void DisplayExpression()
    {
        if (expression != null)
            textExpression.SetText(expression);
    }

    /**
     * This method evaluates the expression on #textExpression and displays it on #textResult
     */
    private void DisplayResult()
    {
        string evalResult; // evaluated result

        /**
         * Do not evaluate expression if it's empty!
         * Simply set the expression & result #Text UIs to the null string ""
         */
        if (expression.ToString().Equals("") || expression == null)
        {
            textExpression.SetText("");
            textResult.SetText("");
            return;
        }

        try
        {
            Expression exp = new Expression(expression.ToString());
            evalResult = exp.Evaluate().ToString();
            Debug.Log("Result: " + evalResult);

            // Remove ".0" if that's the last string at end of evaluated expression
            // TODO: Check type of number and use it to determine result display format
            if (evalResult.Length >= 2)
            {
                if (evalResult.Substring(evalResult.Length - 2, 2).Equals(".0"))
                    evalResult = evalResult.Substring(0, evalResult.Length - 2);
            }

            textResult.text = (evalResult);

            // reset #expression to the calculated #evalResult, so further calculation can be made using it.
            expression = new StringBuilder(evalResult);
        }
        catch (Exception se)
        {
            Debug.LogError(se.Message);
            // textResult.SetText(se.Message);
            textResult.SetText("Syntax Error!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
