using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class TaShiftSaveModel
    {
        public string ShiftName { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftFinishTime { get; set; }
        public int? DuratOfBreak { get; set; }
        public int? LateAllowed { get; set; }
        public int? BreakMinInterval { get; set; }
        public int? DuratOfBreakOvertime { get; set; }
        public int? BreakMinIntervalOvertime { get; set; }
        public int? Presence { get; set; }
        public bool? FirstEntryLastExit { get; set; }
        public int? OvertimeStartLater { get; set; }
        public int? OvertimeStartsEarlier { get; set; }
        public List<string> TaShiftIntervalNamesList { get; set; }
        public List<TimeSpan> TaShiftIntervalStartTimeList { get; set; }
        public List<TimeSpan> TaShiftIntervalEndTimeList { get; set; }
        public List<int> TaShiftIntervalTaReportLabelsIdList { get; set; }
    }
}