namespace Rundeck
{
    public class Job
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Href { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        public string Permalink { get; set; }
        public bool ScheduleEnabled { get; set; }
        public bool Scheduled { get; set; }
        public bool Enabled { get; set; }
        
//        "href": "http://192.168.1.160:4440/api/33/job/8fb632b6-f647-46f6-9bb9-218cb3eeee60",
//        "id": "8fb632b6-f647-46f6-9bb9-218cb3eeee60",
//        "scheduleEnabled": true,
//        "scheduled": false,
//        "enabled": true,
//        "permalink": "http://192.168.1.160:4440/project/CSGLabs/job/show/8fb632b6-f647-46f6-9bb9-218cb3eeee60",
//        "group": null,
//        "description": "",
//        "project": "CSGLabs",
//        "name": "List VMs"
    }
}