using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.ViewModels
{
    public enum SortState
    {
        no,
        YearAcs,
        YearDecs,
        QuarterAcs,
        QuarterDecs,
        IncomeAcs,
        IncomeDecs
    }
    public class SortViewModel
    {
        public SortState CurrentState { get; set; }
        public SortState YearSort { get; set; }
        public SortState QuarterSort { get; set; }
        public SortState IncomeSort { get; set; }

        public SortViewModel(SortState sortState)
        {
            YearSort = sortState == SortState.YearAcs ? SortState.YearDecs : SortState.YearAcs;
            QuarterSort = sortState == SortState.QuarterAcs ? SortState.QuarterDecs : SortState.QuarterAcs;
            IncomeSort = sortState == SortState.IncomeAcs ? SortState.IncomeDecs : SortState.IncomeAcs;
            CurrentState = sortState;
        }
    }
}
