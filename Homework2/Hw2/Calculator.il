.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator 
{
    .method static public float64 
        Calculate(
        float64 value1,
        valuetype Hw2.CalculatorOperation operation,
        float64 value2
        ) cil managed
    {
        .maxstack 2
        .locals init (
        [0] float64 V_1, 
        [1] float64 V_2)
        
        IL_0000: nop
        
        IL_0001: ldc.i4.1
        IL_0002: brtrue.s       IL_0005
        
        IL_0004: nop
        
        IL_0005: ldarg.1
        IL_0006: switch         (IL_000a, IL_000s, IL_000m, IL_000d)
        IL_000e: br.s           IL_00ae
        
        //CalculatorOperation.Plus
        IL_000a: ldarg.0
        ldarg.2
        add
        ret
        
        //CalculatorOperation.Minus
        IL_000s: ldarg.0
        ldarg.2
        sub
        ret
        
        //CalculatorOperation.Multiply
        IL_000m: ldarg.0
        ldarg.2
        mul
        ret
        
        //CalculatorOperation.Divide
        IL_000d: ldarg.0
        ldarg.2
        div
        ret
        
        //throw new ArgumentOutOfRangeException()
        IL_00ae: newobj         instance void [System.Runtime]System.ArgumentOutOfRangeException::.ctor()
        throw
        ret
        
    }
}



