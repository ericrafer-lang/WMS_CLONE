using practice_for_wms.Models.Entities;

namespace practice_for_wms.Models
{
    public class MyTaskIndexViewModel
    {
        public List<MyTask> ActiveTasks { get; set; } = new();
        public List<MyTask> CompletedTasks { get; set; } = new();
    }
}