using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using Expression = System.Linq.Expressions.Expression;

namespace Calculator.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _Result;
    private LinkedList<double> MemoryList = new LinkedList<double>();
    private List<string> SqrtList = new List<string>();
    private int MemoryPointer;
    private Dictionary<char, int> OperationsPriopity = new Dictionary<char, int>()
    {
        {'-', 0}, {'+', 0},
        {'*', 1}, {'/', 1},
        {'√', 2}
    };

    public MainWindowViewModel()
    {
        Result = "√16+8*2-4+√25/7";
        Result = "1/5-4*√9/15-32";
        MemoryPointer = 0;
    }
    public ICommand PlusCmd()
    {
        if (Result.Length != 0 && ! ".-+/*√".Contains(Result[Result.Length - 1]))
            Result += "+";
        return null;
    }
    public ICommand MinusCmd()
    {
        if (Result.Length != 0 && ! ".-+/*√".Contains(Result[Result.Length - 1]))
            Result += "-";
        return null;
    }
    public ICommand MultiplyCmd()
    {
        if (Result.Length != 0 && ! ".-+*/√".Contains(Result[Result.Length - 1]))
            Result += "*";
        return null;
    }
    public ICommand SplitCmd()
    {
        if (Result.Length != 0 && ! ".-+*/√".Contains(Result[Result.Length - 1]))
            Result += "/";
        return null;
    }
    public ICommand NumInput(object Parameter)
    {
        int Number = Convert.ToInt32(Parameter);
        Result += Number;
        return null;
    }
    public ICommand SqrtCmd()
    {
        if (Result.Length == 0 || "/*+-".Contains(Result[Result.Length - 1]))
        {
            Result += "√";   
        }
        
        return null;
    }
    public ICommand MemoryCmd()
    {
        if (MemoryList.Count != 0 && MemoryList.Count > MemoryPointer)
        {
            Result = MemoryList.ElementAt(MemoryPointer).ToString();
            MemoryPointer++;
        }
        else
        {
            MemoryPointer = 0;
            Result = MemoryList.ElementAt(MemoryPointer).ToString();
            MemoryPointer++;
        }
        return null;
    }
    public ICommand MemorySaveCmd()
    {
        if (!Result.Contains("*") && !Result.Contains("/") && !Result.Contains("+") && !Result.Contains("-")
            && !Result.Contains("√") && Result.Length != 0 && Result != "Error")
        {
            MemoryList.AddFirst(Convert.ToDouble(Result));
            if (MemoryList.Count > 3)
            {
                MemoryList.Remove(3);
            }
        }
        
        return null;
    }
    public ICommand CalculateCmd()
    {
        if (Result.Length == 0)
        {
            Result = "0";
        }

        if ("/*-+.√".Contains(Result[Result.Length-1]))
        {
            Result = "Error";
        }

        int NumIndex = 0;
        List<string> OutputStr = new List<string>();
        List<char> Stack = new List<char>();
        string TmpDigit = "";
        for (int i = 0; i < Result.Length; i++)
        {
            if (Char.IsDigit(Result[i]) || Result[i] == '.')
            {
                TmpDigit += Result[i];
            }
            else
            {
                if (TmpDigit != "")
                {
                    OutputStr.Add(TmpDigit);
                    TmpDigit = "";
                }

                if (Stack.Count == 0 || 
                OperationsPriopity[Stack.Last()] < OperationsPriopity[Result[i]])
                {
                    Stack.Add(Result[i]);
                }
                else
                {
                    bool Flag = true;
                    while (Flag)
                    {
                        if (Stack.Count != 0 && OperationsPriopity[Stack.Last()] >= OperationsPriopity[Result[i]])
                        {
                            OutputStr.Add(Stack.Last().ToString());
                            Stack.Remove(Stack.Last());
                        }
                        else
                        {
                            Stack.Add(Result[i]);
                            Flag = false;
                        }
                    }
                }
            }

            if (i == Result.Length - 1)
            {
                OutputStr.Add(TmpDigit);
                for (int j = 0; j < Stack.Count;)
                {
                    OutputStr.Add(Stack.Last().ToString());
                    Stack.Remove(Stack.Last());
                }
            }
        }

        Result = String.Join("", OutputStr.ToArray());
        return null;
    }
    public ICommand ClearMemoryCmd()
    {
        MemoryList.Clear();
        return null;
    }
    public ICommand DotCmd()
    {
        if (Result.Length == 0 || !Char.IsDigit(Result[Result.Length - 1]))
        {
            return null;
        }
            

        for (int i = Result.Length - 1; i >= 0; i--)
        {
            if(Result[i] == '.')
            {
                return null;
            }

            if ("/*-+√".Contains(Result[i]))
            {
                break;
            }
        } 
        
        Result += ".";
        return null;
    }
    public ICommand ClearCmd()
    {
        Result = "";
        return null;
    }
    public string Result
    {
        get { return _Result; }
        set { this.RaiseAndSetIfChanged(ref _Result, value); }
    }

}