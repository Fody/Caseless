using System;
using System.Linq.Expressions;

public class ClassWithExpression 
{
    public ClassWithExpression()
    {
        var s = "a";
        // ReSharper disable UnusedVariable
        Expression<Func<ClassWithExpression, bool>> expressionFunc = x => s == "A";
        Func<ClassWithExpression, bool> func = x => s == "A";
        // ReSharper restore UnusedVariable
    }

}