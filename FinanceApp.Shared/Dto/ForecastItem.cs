﻿namespace FinanceApp.Shared.Dto
{
    public class ForecastItem
    {        
        public DateTime DateReference { get; set;}
        public double NominalLiquidValue { get; set;}
        public double RealLiquidValue { get; set;}   
        public double NominalNotLiquidValue { get; set; }
        public double RealNotLiquidValue { get; set; }
        public double NominalCumulatedAmount { get; set;}
        public double RealCumulatedAmount { get; set;}
        public double NominalValue => NominalLiquidValue + NominalNotLiquidValue;
        public double RealValue => RealLiquidValue + RealNotLiquidValue;
    }
}
