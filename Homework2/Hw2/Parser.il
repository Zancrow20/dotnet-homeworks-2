
.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser
    extends [System.Runtime]System.Object
{
  .method public hidebysig static void
    ParseCalcArguments(
      string[] args,
      [out] float64& val1,
      [out] valuetype Hw2.CalculatorOperation& operation,
      [out] float64& val2
    ) cil managed
  {
    .maxstack 3
    .locals init (
      [0] bool V_0,
      [1] bool V_1,
      [2] bool V_2
    )
        
        ldarg.0
        call bool Hw2.Parser::IsArgLengthSupported(string[])
        ldc.i4.0
        ceq
        stloc.0
        ldloc.0
        brfalse.s parseVal1
        ldstr "Incorrect length"
        newobj instance void [System.Runtime]System.ArgumentException::.ctor(string) 
        throw
        
        parseVal1: 
        ldarg.0
        ldc.i4.0
        ldelem.ref
        ldarg.1
        call bool [System.Runtime]System.Double::TryParse(string,float64&)
        ldc.i4.0
        ceq
        stloc.1
        ldloc.1
        brfalse.s parseVal2
        ldstr "Incorrect first value"
        newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
        throw
    
        
        parseVal2: ldarg.0
        ldc.i4.2
        ldelem.ref
        ldarg.3
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        ldc.i4.0
        ceq
        stloc.2
        ldloc.2
        brfalse.s ParseOperation
        ldstr "Incorrect second value"
        newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
        throw
        
        ParseOperation: ldarg.2
        ldarg.0
        ldc.i4.1
        ldelem.ref
        call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
        stind.i4
        ret
  } // end of method Parser::ParseCalcArguments

  .method private hidebysig static bool
    IsArgLengthSupported(
      string[] args
    ) cil managed
  {
        ldarg.0
        ldlen
        conv.i4
        ldc.i4.3
        ceq
        ret

  } // end of method Parser::IsArgLengthSupported

  .method private hidebysig static valuetype Hw2.CalculatorOperation
    ParseOperation(
      string arg
    ) cil managed
  {
       .maxstack 3
       .locals init ([0] valuetype Hw2.CalculatorOperation)

       Plus:
       ldstr "+"
       ldarg.0
       call bool [System.Runtime]System.String::op_Equality(string, string)
       brfalse.s Minus
       ldc.i4 0
       ret
       
       Minus:
       ldstr "-"
       ldarg.0
       call bool [System.Runtime]System.String::op_Equality(string, string)
       brfalse.s Multiply
       ldc.i4 1
       ret
       
       Multiply:
       ldstr "*"
       ldarg.0
       call bool [System.Runtime]System.String::op_Equality(string, string)
       brfalse.s Divide
       ldc.i4 2
       ret
       
       Divide:
       ldstr "/"
       ldarg.0
       call bool [System.Runtime]System.String::op_Equality(string, string)
       brfalse.s InvalidOperationException
       ldc.i4 3
       ret
       
       InvalidOperationException:
       newobj instance void [System.Runtime]System.InvalidOperationException::.ctor()
       throw
       ret

  } // end of method Parser::ParseOperation
} // end of class Hw1.Parser
