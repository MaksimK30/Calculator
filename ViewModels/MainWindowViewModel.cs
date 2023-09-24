using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;

namespace Calculator.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _Result;
    private LinkedList<double> MemoryList = new LinkedList<double>();
    private List<string> SqrtList = new List<string>();
    private int MemoryPointer;

    public MainWindowViewModel()
    {
        Result = "";
        MemoryPointer = 0;
    }
    public ICommand PlusCmd()
    {
        Result += "+";
        return null;
    }
    public ICommand MinusCmd()
    {
        if (Result.Length == 0 || ! ".-+/*√".Contains(Result[Result.Length - 1]))
            Result += "-";
        return null;
    }
    public ICommand MultiplyCmd()
    {
        Result += "*";
        return null;
    }
    public ICommand SplitCmd()
    {
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
        if (MemoryList.Count == 0)
        {
            return null;
        }
        
        if (MemoryList.Count > MemoryPointer)
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
            && !Result.Contains("√") && Result.Length != 0)
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
        
        return null;
    }
    public ICommand ClearMemoryCmd()
    {
        MemoryList.Clear();
        return null;
    }
    public ICommand DotCmd()
    {
        if (Result?.Length == 0 || !Char.IsDigit(Result[Result.Length - 1]))
            return null;

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