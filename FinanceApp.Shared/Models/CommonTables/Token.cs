﻿namespace FinanceApp.Shared.Models.CommonTables;

public class Token
{
    public Token(string value)
    {
        Value = value;
    }

    public string Value { get; }
}
