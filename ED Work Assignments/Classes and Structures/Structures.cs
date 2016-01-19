using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    public struct OffClocking
    {
        public object id;
        public object date;
    }
    public struct SupervisorClocking
    {
        public object id;
        public object start;
        public object end;
    }
    public struct Shift
    {
        public TimeSpan shiftTimeSpan;
        public DateTime startTime;
    }
    public enum SchedulingMode {Type1, Type2};
    public struct MarkAsAbsentShift
    {
        public object Name;
        public object Seat;
        public object Start;
        public object End;
    }
}
